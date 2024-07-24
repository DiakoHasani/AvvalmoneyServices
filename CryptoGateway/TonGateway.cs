using AS.BL.Services;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.ReservationWallet;
using AS.Model.TransactionId;
using AS.Model.Wallet;
using AS.Utility.Helpers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGateway
{
    public class TonGateway : ITonGateway
    {
        private readonly ILogger _logger;
        private readonly ITonScanService _tonScanService;
        private readonly IReservationWalletApiService _reservationWalletApiService;
        private readonly IWalletApiService _walletApiService;
        private readonly ITransactionIdApiService _transactionIdApiService;
        private readonly IDealRequestApiService _dealRequestApiService;
        private readonly IMapper _mapper;

        List<ReservationWalletModel> reservationWallets;
        WalletModel wallet;
        DealRequestModel dealRequest;

        public TonGateway(ILogger logger,
            ITonScanService tonScanService,
            IReservationWalletApiService reservationWalletApiService,
            IWalletApiService walletApiService,
            ITransactionIdApiService transactionIdApiService,
            IDealRequestApiService dealRequestApiService,
            IMapper mapper)
        {
            _logger = logger;
            _tonScanService = tonScanService;
            _reservationWalletApiService = reservationWalletApiService;
            _walletApiService = walletApiService;
            _transactionIdApiService = transactionIdApiService;
            _dealRequestApiService = dealRequestApiService;
            _mapper = mapper;
        }

        public async Task Call(string token)
        {
            try
            {
                reservationWallets = await _reservationWalletApiService.GetReservations(DateTime.Now.AddMinutes(-20), DateTime.Now, CryptoType.Ton, token);

                foreach (var reservationWallet in reservationWallets)
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);

                    _logger.Information("get reservationWallet", new { reservationWallet = reservationWallet });

                    wallet = await _walletApiService.GetWalletById(reservationWallet.Wal_Id, token);

                    var lastTon = await _tonScanService.GetLastTon(wallet.Address);
                    if (lastTon is null)
                    {
                        _logger.Error("lastTon is null");
                        continue;
                    }
                    _logger.Information("get lastTon", lastTon);

                    if (await _transactionIdApiService.CheckExistTransactionIdCode(lastTon.EventId, token))
                    {
                        continue;
                    }

                    var resultAddTransactionId = await _transactionIdApiService.Add(new TransactionIdModel { TransactionIdCode = lastTon.EventId, Wal_Id = wallet.Wal_Id }, token);

                    dealRequest = await _dealRequestApiService.DepositDealRequest(lastTon.Amount.DivisionBy9Zero(), wallet.Wal_Id, ServiceKeys.AmountDifferenceTon, token);
                    if (dealRequest is null)
                    {
                        _logger.Error("dealRequest is null");
                        continue;
                    }
                    _logger.Information("get dealRequest", new { dealRequest = dealRequest });

                    dealRequest.Drq_Status = DealRequestStatus.Done;
                    dealRequest.Txid = lastTon.EventId;
                    dealRequest.Drq_Amount = lastTon.Amount.DivisionBy9Zero();

                    var resultUpdateDealRequest = await _dealRequestApiService.UpdateGateway(_mapper.Map<DealRequestGatewayModel>(dealRequest), token);
                    if (resultUpdateDealRequest is null)
                    {
                        continue;
                    }
                    _logger.Information("updated dealRequest");

                    var resultUpdateWallet = await _walletApiService.UpdateLastTransaction(wallet.Wal_Id, token);
                    if (resultUpdateWallet)
                    {
                        _logger.Information("updated wallet");
                    }

                    var resultApproveStatus = await _reservationWalletApiService.ApproveStatus(reservationWallet.Rw_Id, token);
                    if (resultApproveStatus)
                    {
                        _logger.Information("updated reservationWallet");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }
    public interface ITonGateway
    {
        Task Call(string token);
    }
}

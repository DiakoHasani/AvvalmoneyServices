using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.ReservationWallet;
using AS.Model.Transaction;
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
    public class TronGateway : ITronGateway
    {
        private readonly ILogger _logger;
        private readonly ITronScanService _tronScanService;
        private readonly IReservationWalletApiService _reservationWalletApiService;
        private readonly IWalletApiService _walletApiService;
        private readonly ITransactionIdApiService _transactionIdApiService;
        private readonly IDealRequestApiService _dealRequestApiService;
        private readonly IMapper _mapper;

        List<ReservationWalletModel> reservationWallets;
        WalletModel wallet;
        DealRequestModel dealRequest;
        public TronGateway(ILogger logger,
            ITronScanService tronScanService,
            IReservationWalletApiService reservationWalletApiService,
            IWalletApiService walletApiService,
            ITransactionIdApiService transactionIdApiService,
            IDealRequestApiService dealRequestApiService,
            IMapper mapper)
        {
            _logger = logger;
            _tronScanService = tronScanService;
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
                reservationWallets = await _reservationWalletApiService.GetReservations(DateTime.Now.AddMinutes(-20), DateTime.Now, CryptoType.Tron, token);
                foreach (var reservationWallet in reservationWallets)
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);

                    _logger.Information("get reservationWallet", new { reservationWalletId = reservationWallet.Rw_Id });
                    wallet = await _walletApiService.GetWalletById(reservationWallet.Wal_Id, token);

                    var lastTrx = await _tronScanService.GetLastTRX(wallet.Address);
                    if (lastTrx is null)
                    {
                        _logger.Error("lastTrx is null");
                        continue;
                    }
                    _logger.Information("get lastTrx", lastTrx);

                    if (await _transactionIdApiService.CheckExistTransactionIdCode(lastTrx.TransactionHash,token))
                    {
                        continue;
                    }

                    var resultAddTransactionId=await _transactionIdApiService.Add(new TransactionIdModel { TransactionIdCode = lastTrx.TransactionHash, Wal_Id = wallet.Wal_Id }, token);

                    if (reservationWallet is null)
                    {
                        continue;
                    }

                    dealRequest =await _dealRequestApiService.DepositDealRequest(lastTrx.Amount.DivisionBy6Zero(), wallet.Wal_Id, ServiceKeys.AmountDifferenceTron, token);

                    if (dealRequest is null)
                    {
                        _logger.Error("dealRequest is null");
                        continue;
                    }
                    _logger.Information("get dealRequest", new { dealRequestId = dealRequest.Drq_Id });

                    if (lastTrx.ContractRet != "SUCCESS")
                    {
                        continue;
                    }

                    dealRequest.Drq_Status = DealRequestStatus.Done;
                    dealRequest.Txid = lastTrx.TransactionHash;
                    dealRequest.Drq_Amount = lastTrx.Amount.DivisionBy6Zero();
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
    public interface ITronGateway
    {
        Task Call(string token);
    }
}

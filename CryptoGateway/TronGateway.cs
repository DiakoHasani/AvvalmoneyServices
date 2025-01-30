using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.ReservationWallet;
using AS.Model.Transaction;
using AS.Model.TransactionId;
using AS.Model.TronGrid;
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
        private readonly IReservationWalletApiService _reservationWalletApiService;
        private readonly IWalletApiService _walletApiService;
        private readonly ITransactionIdApiService _transactionIdApiService;
        private readonly IDealRequestApiService _dealRequestApiService;
        private readonly IMapper _mapper;
        private readonly ITronGridServices _tronGridServices;
        private readonly IWebhookApiService _webhookApiService;
        private readonly IAESServices _aesServices;

        List<ReservationWalletModel> reservationWallets;
        WalletModel wallet;
        DealRequestModel dealRequest;
        List<SummaryResponseTrxTronGridModel> responseTrx;
        List<TransactionIdModel> transactonIds;

        public TronGateway(ILogger logger,
            ITronGridServices tronGridServices,
            IReservationWalletApiService reservationWalletApiService,
            IWalletApiService walletApiService,
            ITransactionIdApiService transactionIdApiService,
            IDealRequestApiService dealRequestApiService,
            IMapper mapper,
            IWebhookApiService webhookApiService,
            IAESServices aesServices)
        {
            _logger = logger;
            _tronGridServices = tronGridServices;
            _reservationWalletApiService = reservationWalletApiService;
            _walletApiService = walletApiService;
            _transactionIdApiService = transactionIdApiService;
            _dealRequestApiService = dealRequestApiService;
            _mapper = mapper;
            _webhookApiService = webhookApiService;
            _aesServices = aesServices;
        }

        public async Task Call(string token)
        {
            reservationWallets = await _reservationWalletApiService.GetReservations(DateTime.Now.AddMinutes(-20), DateTime.Now, CryptoType.Tron, token);
            foreach (var reservationWallet in reservationWallets)
            {
                try
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);
                    _logger.Information("get reservationWallet", new { reservationWallet = reservationWallet });

                    responseTrx = await _tronGridServices.GetTrx(reservationWallet.WalletAddress);
                    responseTrx = responseTrx.Where(o => o.ToAddress.ToBase58() == reservationWallet.WalletAddress).Take(5).ToList();

                    transactonIds = await _transactionIdApiService.GetTransactionIds(reservationWallet.Wal_Id, 5, token);

                    foreach (var transaction in responseTrx)
                    {
                        if (!transactonIds.Any(o => o.TransactionIdCode == transaction.Txid))
                        {
                            var response = await _webhookApiService.Tron(ServiceKeys.WithdrawKey, transaction.Txid, transaction.Amount, reservationWallet.Wal_Id, reservationWallet.Rw_Id, token);
                            if (response.IsValid)
                            {
                                _logger.Information(response.Message);
                            }
                            else
                            {
                                _logger.Error(response.Message);
                            }
                            await Task.Delay(ServiceKeys.DelayCryptoGateway);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                }
            }
        }
    }

    public interface ITronGateway
    {
        Task Call(string token);
    }
}
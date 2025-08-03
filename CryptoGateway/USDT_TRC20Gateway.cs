using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.PaymentWithdrawBot;
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
    public class USDT_TRC20Gateway : IUSDT_TRC20Gateway
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
        DealRequestModel dealRequest;
        ResponseTrc20TronGridModel responseTrc20;
        List<TransactionIdModel> transactonIds;
        public USDT_TRC20Gateway(ILogger logger,
            IReservationWalletApiService reservationWalletApiService,
            IWalletApiService walletApiService,
            ITransactionIdApiService transactionIdApiService,
            IDealRequestApiService dealRequestApiService,
            IMapper mapper,
            ITronGridServices tronGridServices,
            IWebhookApiService webhookApiService,
            IAESServices aesServices)
        {
            _logger = logger;
            _reservationWalletApiService = reservationWalletApiService;
            _walletApiService = walletApiService;
            _transactionIdApiService = transactionIdApiService;
            _dealRequestApiService = dealRequestApiService;
            _mapper = mapper;
            _tronGridServices = tronGridServices;
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

                    responseTrc20 = await _tronGridServices.GetTrc20(reservationWallet.WalletAddress);
                    if(responseTrc20 is null)
                    {
                        _logger.Error("responseTrc20 is null");
                        continue;
                    }

                    responseTrc20.Data = responseTrc20.Data.Where(o => o.TokenInfo.Symbol == "USDT").ToList();
                    responseTrc20.Data = responseTrc20.Data.Where(o => o.To == reservationWallet.WalletAddress).Take(5).ToList();

                    transactonIds = await _transactionIdApiService.GetTransactionIds(reservationWallet.Wal_Id, 5, token);

                    foreach (var transaction in responseTrc20.Data)
                    {
                        if (!transactonIds.Any(o => o.TransactionIdCode == transaction.TransactionId))
                        {
                            var response = await _webhookApiService.Usdt(ServiceKeys.WithdrawKey, transaction.TransactionId, transaction.Value.ToDouble(), reservationWallet.Wal_Id, reservationWallet.Rw_Id, token);
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

    public interface IUSDT_TRC20Gateway
    {
        Task Call(string token);
    }
}

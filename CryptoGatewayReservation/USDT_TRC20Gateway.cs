using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Transaction;
using AS.Model.TransactionId;
using AS.Model.TronScan;
using AS.Model.UserWalletReservation;
using AS.Model.Wallet;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGatewayReservation
{
    public class USDT_TRC20Gateway : IUSDT_TRC20Gateway
    {
        private readonly ILogger _logger;
        private readonly ITronScanService _tronScanService;
        private readonly IUserWalletReservationApiService _userWalletReservationApiService;
        private readonly IWalletApiService _walletApiService;
        private readonly ITransactionIdApiService _transactionIdApiService;
        private readonly IDealRequestApiService _dealRequestApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;

        List<UserWalletReservationModel> userWalletReservations;
        TronScanTrc20TokenTransferModel lastTrc20;
        WalletModel wallet;
        int cur_Id = 40;

        public USDT_TRC20Gateway(ILogger logger,
            ITronScanService tronScanService,
            IUserWalletReservationApiService userWalletReservationApiService,
            IWalletApiService walletApiService,
            ITransactionIdApiService transactionIdApiService,
            IDealRequestApiService dealRequestApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService)
        {
            _logger = logger;
            _tronScanService = tronScanService;
            _userWalletReservationApiService = userWalletReservationApiService;
            _walletApiService = walletApiService;
            _transactionIdApiService = transactionIdApiService;
            _dealRequestApiService = dealRequestApiService;
            _currencyPriceHistoryApiService= currencyPriceHistoryApiService;
        }

        public async Task Call(string token)
        {
            try
            {
                userWalletReservations = await _userWalletReservationApiService.GetUserWalletReservations(CurrencyType.USDT_TRC20, token);
                if (userWalletReservations.Count == 0)
                {
                    _logger.Information("userWalletReservations.Count is 0");
                    return;
                }

                _logger.Information("get userWalletReservations");

                foreach (var userWalletReservation in userWalletReservations)
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);
                    wallet = await _walletApiService.GetWalletById(userWalletReservation.Wal_Id, token);
                    _logger.Information("get wallet " + wallet.Address);

                    lastTrc20 = await _tronScanService.GetLastTRC20(wallet.Address);

                    if (lastTrc20 is null)
                    {
                        _logger.Information("lastTrc20 is null");
                        continue;
                    }

                    _logger.Information("get lastTrc20", lastTrc20);

                    if ((userWalletReservation.UWR_LastTxId ?? "") == lastTrc20.TransactionIdCode)
                    {
                        continue;
                    }

                    var resultAddTransactionId = await _transactionIdApiService.Add(new TransactionIdModel { TransactionIdCode = lastTrc20.TransactionIdCode, Wal_Id = wallet.Wal_Id }, token);

                    if(resultAddTransactionId != null)
                    {
                        _logger.Information("added transactionId to Database", resultAddTransactionId);
                    }

                    if (userWalletReservation.UWR_TransactionCount != 0)
                    {
                        var currencyPriceHistory = await _currencyPriceHistoryApiService.GetByCur_Id(cur_Id,token);
                        if(currencyPriceHistory is null)
                        {
                            _logger.Error("currencyPriceHistory is null");
                            continue;
                        }
                        _logger.Information("get currencyPriceHistory", currencyPriceHistory);

                        var dealRequest = await _dealRequestApiService.Add(new AS.Model.DealRequest.RequestDealModel
                        {
                            Aff_Id = wallet.Aff_Id,
                            CPH_Id = currencyPriceHistory.CPH_Id,
                            Cur_Id = cur_Id,
                            Drq_Status = DealRequestStatus.Done,
                            Drq_Type = DealRequestType.SellToAM,
                            Drq_VerificationStatus = DealRequestVerificationStatus.Accepted,
                            Drq_VerificationType = DealRequestVerificationType.Auto,
                            Drq_Amount = lastTrc20.Quant.DivisionBy6Zero(),
                            Drq_Cur_Latest_Price = currencyPriceHistory.CPH_SellPrice,
                            Drq_TotalPrice = (long)(lastTrc20.Quant.DivisionBy6Zero() * currencyPriceHistory.CPH_SellPrice),
                            Usr_Id = wallet.Usr_Id.Value,
                            Wal_Id = wallet.Wal_Id,
                            Txid = lastTrc20.TransactionIdCode,
                            Drq_CreateDate = DateTime.Now
                        }, token);

                        if(dealRequest is null)
                        {
                            _logger.Error("error in add dealRequest");
                            continue;
                        }
                        _logger.Information("get dealRequest", dealRequest);
                    }

                    var resultUpdateTxid = await _userWalletReservationApiService.UpdateTxid(userWalletReservation.UWR_Id, lastTrc20.TransactionIdCode,token);
                    if (resultUpdateTxid)
                    {
                        _logger.Information("updated userWalletReservation");
                    }
                    else
                    {
                        _logger.Information("error in update userWalletReservation");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }
    public interface IUSDT_TRC20Gateway
    {
        Task Call(string token);
    }
}

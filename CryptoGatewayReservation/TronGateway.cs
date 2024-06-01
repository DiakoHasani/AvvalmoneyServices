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
    public class TronGateway : ITronGateway
    {
        private readonly ILogger _logger;
        private readonly ITronScanService _tronScanService;
        private readonly IUserWalletReservationApiService _userWalletReservationApiService;
        private readonly IWalletApiService _walletApiService;
        private readonly ITransactionIdApiService _transactionIdApiService;
        private readonly IDealRequestApiService _dealRequestApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;

        List<UserWalletReservationModel> userWalletReservations;
        ResponseTronScanTrxDataModel lastTrx;
        WalletModel wallet;
        int cur_Id = 50;

        public TronGateway(ILogger logger,
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
            _currencyPriceHistoryApiService = currencyPriceHistoryApiService;
        }

        public async Task Call(string token)
        {
            try
            {
                userWalletReservations = await _userWalletReservationApiService.GetUserWalletReservations(CurrencyType.Tron,token);
                if (userWalletReservations.Count == 0)
                {
                    _logger.Information("userWalletReservations.Count is 0");
                    return;
                }

                _logger.Information("get userWalletReservations");

                foreach (var userWalletReservation in userWalletReservations)
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);
                    wallet = await _walletApiService.GetWalletById(userWalletReservation.Wal_Id,token);
                    _logger.Information("get wallet "+wallet.Address);
                    lastTrx = await _tronScanService.GetLastTRX(wallet.Address);

                    if (lastTrx is null)
                    {
                        _logger.Information("lastTrx is null");
                        continue;
                    }

                    _logger.Information("get lastTrx", lastTrx);

                    if ((userWalletReservation.UWR_LastTxId ?? "") == lastTrx.TransactionHash)
                    {
                        continue;
                    }

                    var resultAddTransactionId = await _transactionIdApiService.Add(new TransactionIdModel { TransactionIdCode = lastTrx.TransactionHash, Wal_Id = wallet.Wal_Id }, token);

                    if (resultAddTransactionId != null)
                    {
                        _logger.Information("added transactionId to Database", resultAddTransactionId);
                    }

                    if (userWalletReservation.UWR_TransactionCount != 0)
                    {
                        var currencyPriceHistory = await _currencyPriceHistoryApiService.GetByCur_Id(cur_Id,token);
                        if (currencyPriceHistory is null)
                        {
                            _logger.Error("currencyPriceHistory is null");
                            continue;
                        }

                        var dealRequest = await _dealRequestApiService.Add(new AS.Model.DealRequest.RequestDealModel
                        {
                            Aff_Id = wallet.Aff_Id,
                            CPH_Id = currencyPriceHistory.CPH_Id,
                            Cur_Id = cur_Id,
                            Drq_Status = DealRequestStatus.Done,
                            Drq_Type = DealRequestType.SellToAM,
                            Drq_VerificationStatus = DealRequestVerificationStatus.Accepted,
                            Drq_VerificationType = DealRequestVerificationType.Auto,
                            Drq_Amount = lastTrx.Amount.DivisionBy6Zero(),
                            Drq_Cur_Latest_Price = currencyPriceHistory.CPH_SellPrice,
                            Drq_TotalPrice = (long)(lastTrx.Amount.DivisionBy6Zero() * currencyPriceHistory.CPH_SellPrice),
                            Usr_Id = wallet.Usr_Id.Value,
                            Wal_Id = wallet.Wal_Id,
                            Txid=lastTrx.TransactionHash,
                            Drq_CreateDate=DateTime.Now
                        },token);

                        if (dealRequest is null)
                        {
                            _logger.Error("error in add dealRequest");
                            continue;
                        }
                        _logger.Information("get dealRequest", dealRequest);
                    }

                    var resultUpdateTxid = await _userWalletReservationApiService.UpdateTxid(userWalletReservation.UWR_Id, lastTrx.TransactionHash, token);
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
    public interface ITronGateway
    {
        Task Call(string token);
    }
}

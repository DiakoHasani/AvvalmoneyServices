using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Transaction;
using AS.Model.TransactionId;
using AS.Model.TronGrid;
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
        private readonly ITronGridServices _tronGridServices;
        private readonly IUserWalletReservationApiService _userWalletReservationApiService;
        private readonly IWalletApiService _walletApiService;
        private readonly ITransactionIdApiService _transactionIdApiService;
        private readonly IDealRequestApiService _dealRequestApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;

        List<UserWalletReservationModel> userWalletReservations;
        List<SummaryResponseTrxTronGridModel> responseTrx;
        List<TransactionIdModel> transactonIds;
        WalletModel wallet;
        int cur_Id = 50;

        public TronGateway(ILogger logger,
            ITronGridServices tronGridServices,
            IUserWalletReservationApiService userWalletReservationApiService,
            IWalletApiService walletApiService,
            ITransactionIdApiService transactionIdApiService,
            IDealRequestApiService dealRequestApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService)
        {
            _logger = logger;
            _tronGridServices = tronGridServices;
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
                userWalletReservations = await _userWalletReservationApiService.GetUserWalletReservations(CurrencyType.Tron, token);
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
                    if (wallet is null)
                    {
                        _logger.Error($"this wallet id {userWalletReservation.Wal_Id} is null");
                        continue;
                    }
                    _logger.Information("get wallet " + wallet.Address);

                    responseTrx = await _tronGridServices.GetTrx(wallet.Address);
                    if (responseTrx is null)
                    {
                        _logger.Information("responseTrx is null");
                        continue;
                    }

                    responseTrx = responseTrx.Where(o => o.ToAddress.ToBase58() == wallet.Address).Take(5).ToList();
                    _logger.Information("get responseTrx", responseTrx);

                    if (responseTrx.Count == 0)
                    {
                        _logger.Information("responseTrx.Count == 0");
                        continue;
                    }

                    transactonIds = await _transactionIdApiService.GetTransactionIds(wallet.Wal_Id, 15, token);

                    foreach (var transaction in responseTrx)
                    {
                        if (!transactonIds.Any(o => o.TransactionIdCode == transaction.Txid))
                        {
                            var resultAddTransactionId = await _transactionIdApiService.Add(new TransactionIdModel { TransactionIdCode = transaction.Txid, Wal_Id = wallet.Wal_Id }, token);
                            if (resultAddTransactionId != null)
                            {
                                _logger.Information("added transactionId to Database", resultAddTransactionId);
                            }

                            var currencyPriceHistory = await _currencyPriceHistoryApiService.GetByCur_Id(cur_Id, token);
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
                                Drq_Amount = transaction.Amount.DivisionBy6Zero(),
                                Drq_Cur_Latest_Price = currencyPriceHistory.CPH_SellPrice,
                                Drq_TotalPrice = (long)(transaction.Amount.DivisionBy6Zero() * currencyPriceHistory.CPH_SellPrice),
                                Usr_Id = wallet.Usr_Id.Value,
                                Wal_Id = wallet.Wal_Id,
                                Txid = transaction.Txid,
                                Drq_CreateDate = DateTime.Now
                            }, token);
                            if (dealRequest is null)
                            {
                                _logger.Error("error in add dealRequest");
                                continue;
                            }
                            _logger.Information("get dealRequest", dealRequest);

                            var resultUpdateTxid = await _userWalletReservationApiService.UpdateTxid(userWalletReservation.UWR_Id, transaction.Txid, token);
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
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        public async Task FillWallets(string token)
        {
            try
            {
                userWalletReservations = await _userWalletReservationApiService.GetUserWalletReservations(CurrencyType.Tron, token);
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
                    if (wallet is null)
                    {
                        _logger.Error($"this wallet id {userWalletReservation.Wal_Id} is null");
                        continue;
                    }
                    _logger.Information("get wallet " + wallet.Address);

                    responseTrx = await _tronGridServices.GetTrx(wallet.Address);
                    if (responseTrx is null)
                    {
                        _logger.Information("responseTrx is null");
                        continue;
                    }

                    responseTrx = responseTrx.Where(o => o.ToAddress.ToBase58() == wallet.Address).Take(5).ToList();
                    _logger.Information("get responseTrx", responseTrx);

                    if (responseTrx.Count == 0)
                    {
                        _logger.Information("responseTrx.Count == 0");
                        continue;
                    }

                    transactonIds = await _transactionIdApiService.GetTransactionIds(wallet.Wal_Id, 15, token);

                    foreach (var transaction in responseTrx)
                    {
                        if (!transactonIds.Any(o => o.TransactionIdCode == transaction.Txid))
                        {
                            var resultAddTransactionId = await _transactionIdApiService.Add(new TransactionIdModel { TransactionIdCode = transaction.Txid, Wal_Id = wallet.Wal_Id }, token);
                            if (resultAddTransactionId != null)
                            {
                                _logger.Information("added transactionId to Database", resultAddTransactionId);
                            }

                            var resultUpdateTxid = await _userWalletReservationApiService.UpdateTxid(userWalletReservation.UWR_Id, transaction.Txid, token);
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
        Task FillWallets(string token);
    }
}

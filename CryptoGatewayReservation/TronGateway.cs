using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Transaction;
using AS.Model.TronScan;
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
        private readonly IUserWalletReservationService _userWalletReservationService;
        private readonly IWalletService _walletService;
        private readonly ITransactionIdService _transactionIdService;
        private readonly IDealRequestService _dealRequestService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;

        List<UserWalletReservation> userWalletReservations;
        ResponseTronScanTrxDataModel lastTrx;
        Wallet wallet;
        int cur_Id = 50;

        public TronGateway(ILogger logger,
            ITronScanService tronScanService,
            IUserWalletReservationService userWalletReservationService,
            IWalletService walletService,
            ITransactionIdService transactionIdService,
            IDealRequestService dealRequestService,
            IUserService userService,
            ITransactionService transactionService,
            ICurrencyPriceHistoryService currencyPriceHistoryService)
        {
            _logger = logger;
            _tronScanService = tronScanService;
            _userWalletReservationService = userWalletReservationService;
            _walletService = walletService;
            _transactionIdService = transactionIdService;
            _dealRequestService = dealRequestService;
            _userService = userService;
            _transactionService = transactionService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
        }

        public async Task Call()
        {
            try
            {
                await Task.Delay(ServiceKeys.DelayCryptoGateway);
                userWalletReservations = _userWalletReservationService.GetUserWalletReservations(CurrencyType.Tron);
                if (userWalletReservations.Count == 0)
                {
                    _logger.Information("userWalletReservations.Count is 0");
                    return;
                }

                _logger.Information("get userWalletReservations");

                foreach (var userWalletReservation in userWalletReservations)
                {
                    wallet = await _walletService.GetWalletById(userWalletReservation.Wal_Id);
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

                    await _transactionIdService.Add(lastTrx.TransactionHash, wallet.Wal_Id);

                    if (userWalletReservation.UWR_TransactionCount != 0)
                    {
                        var currencyPriceHistory = _currencyPriceHistoryService.GetByCur_Id(cur_Id);
                        var dealRequest = await _dealRequestService.Add(new AS.Model.DealRequest.RequestDealModel
                        {
                            Aff_Id = wallet.Aff_Id,
                            CPH_Id = currencyPriceHistory.CPH_Id,
                            Cur_Id = cur_Id,
                            DealRequestStatus = DealRequestStatus.Done,
                            DealRequestType = DealRequestType.SellToAM,
                            DealRequestVerificationStatus = DealRequestVerificationStatus.Accepted,
                            DealRequestVerificationType = DealRequestVerificationType.Auto,
                            Drq_Amount = lastTrx.Amount.DivisionBy6Zero(),
                            Drq_Cur_Latest_Price = currencyPriceHistory.CPH_SellPrice,
                            Drq_TotalPrice = (long)(lastTrx.Amount.DivisionBy6Zero() * currencyPriceHistory.CPH_SellPrice),
                            Usr_Id = wallet.Usr_Id.Value,
                            Wal_Id = wallet.Wal_Id,
                            Txid=lastTrx.TransactionHash
                        });

                        if (dealRequest != null)
                        {
                            await UpdateUserBalance(dealRequest.Drq_TotalPrice, dealRequest.Usr_Id.Value);
                        }
                    }

                    userWalletReservation.UWR_LastTxId = lastTrx.TransactionHash;
                    userWalletReservation.UWR_TransactionCount++;
                    await _userWalletReservationService.Update(userWalletReservation);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        private async Task<bool> UpdateUserBalance(double totalPrice, long userId)
        {
            var transaction = await _transactionService.AddTransacton(new RequestTransactionModel
            {
                AdminUserId = ServiceKeys.AdmUsr_Id,
                Amount = totalPrice,
                TransactionType = TransactionType.Sell,
                UserId = userId
            });
            _logger.Information("added Transaction");

            var user = await _userService.GetByIdAsync(userId);
            user.Usr_BLNC_Balance = transaction.Tns_After;
            await _userService.Update(user);
            _logger.Information("updated userBalance");

            return true;
        }
    }
    public interface ITronGateway
    {
        Task Call();
    }
}

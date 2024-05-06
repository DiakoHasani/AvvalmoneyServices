using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Transaction;
using AS.Model.TronScan;
using AS.Model.UserWalletReservation;
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
        private readonly IUserWalletReservationService _userWalletReservationService;
        private readonly IWalletService _walletService;
        private readonly ITransactionIdService _transactionIdService;
        private readonly IDealRequestService _dealRequestService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;

        List<UserWalletReservation> userWalletReservations;
        TronScanTrc20TokenTransferModel lastTrc20;
        Wallet wallet;
        int cur_Id = 40;

        public USDT_TRC20Gateway(ILogger logger,
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
                userWalletReservations = _userWalletReservationService.GetUserWalletReservations(CurrencyType.USDT_TRC20);
                if (userWalletReservations.Count == 0)
                {
                    _logger.Information("userWalletReservations.Count is 0");
                    return;
                }

                _logger.Information("get userWalletReservations");

                foreach (var userWalletReservation in userWalletReservations)
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);
                    wallet = await _walletService.GetWalletById(userWalletReservation.Wal_Id);
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

                    await _transactionIdService.Add(lastTrc20.TransactionIdCode, wallet.Wal_Id);

                    if (userWalletReservation.UWR_TransactionCount != 0)
                    {
                        var currencyPriceHistory = _currencyPriceHistoryService.GetByCur_Id(cur_Id);
                        var dealRequest= await _dealRequestService.Add(new AS.Model.DealRequest.RequestDealModel
                        {
                            Aff_Id = wallet.Aff_Id,
                            CPH_Id = currencyPriceHistory.CPH_Id,
                            Cur_Id = cur_Id,
                            DealRequestStatus = DealRequestStatus.Done,
                            DealRequestType = DealRequestType.SellToAM,
                            DealRequestVerificationStatus = DealRequestVerificationStatus.Accepted,
                            DealRequestVerificationType = DealRequestVerificationType.Auto,
                            Drq_Amount = lastTrc20.Quant.DivisionBy6Zero(),
                            Drq_Cur_Latest_Price = currencyPriceHistory.CPH_SellPrice,
                            Drq_TotalPrice = (long)(lastTrc20.Quant.DivisionBy6Zero() * currencyPriceHistory.CPH_SellPrice),
                            Usr_Id = wallet.Usr_Id.Value,
                            Wal_Id = wallet.Wal_Id,
                            Txid=lastTrc20.TransactionIdCode
                        });

                        if(dealRequest != null)
                        {
                            await UpdateUserBalance(dealRequest.Drq_TotalPrice,dealRequest.Usr_Id.Value);
                        }
                    }

                    userWalletReservation.UWR_LastTxId = lastTrc20.TransactionIdCode;
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
    public interface IUSDT_TRC20Gateway
    {
        Task Call();
    }
}

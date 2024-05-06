using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Transaction;
using AS.Utility.Helpers;
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
        private readonly IReservationWalletService _reservationWalletService;
        private readonly IWalletService _walletService;
        private readonly ITransactionIdService _transactionIdService;
        private readonly ITronScanService _tronScanService;
        private readonly IDealRequestService _dealRequestService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly IMenuNotificationService _menuNotificationService;

        List<ReservationWallet> reservationWallets;
        Wallet wallet;
        DealRequest dealRequest;
        public TronGateway(ILogger logger,
            IReservationWalletService reservationWalletService,
            IWalletService walletService,
            ITransactionIdService transactionIdService,
            ITronScanService tronScanService,
            IDealRequestService dealRequestService,
            IUserService userService,
            ITransactionService transactionService,
            IMenuNotificationService menuNotificationService)
        {
            _logger = logger;
            _reservationWalletService = reservationWalletService;
            _walletService = walletService;
            _transactionIdService = transactionIdService;
            _tronScanService = tronScanService;
            _dealRequestService = dealRequestService;
            _userService = userService;
            _transactionService = transactionService;
            _menuNotificationService = menuNotificationService;
        }

        public async Task Call()
        {
            try
            {
                reservationWallets = _reservationWalletService.GetReservations(DateTime.Now.AddMinutes(-20), DateTime.Now, CryptoType.Tron);
                foreach (var reservationWallet in reservationWallets)
                {
                    await Task.Delay(ServiceKeys.DelayCryptoGateway);

                    _logger.Information("get reservationWallet", new { reservationWalletId = reservationWallet.Rw_Id });
                    wallet = await _walletService.GetWalletById(reservationWallet.Wal_Id);

                    var lastTrx = await _tronScanService.GetLastTRX(wallet.Address);
                    if (lastTrx is null)
                    {
                        _logger.Error("lastTrx is null");
                        continue;
                    }
                    _logger.Information("get lastTrx", lastTrx);

                    if (_transactionIdService.CheckExistTransactionIdCode(lastTrx.TransactionHash))
                    {
                        continue;
                    }

                    await _transactionIdService.Add(lastTrx.TransactionHash, wallet.Wal_Id);

                    dealRequest = _dealRequestService.DepositDealRequest(lastTrx.Amount.DivisionBy6Zero(), wallet.Wal_Id);

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

                    dealRequest.Drq_Status = (int)DealRequestStatus.Done;
                    dealRequest.Txid = lastTrx.TransactionHash;
                    await _dealRequestService.Update(dealRequest);
                    _logger.Information("updated dealRequest");

                    wallet.LastTransaction = DateTime.Now;
                    await _walletService.Update(wallet);
                    _logger.Information("updated wallet");

                    await UpdateUserBalance(dealRequest.Drq_TotalPrice, dealRequest.Usr_Id ?? 0);

                    await _menuNotificationService.PushTransaction();

                    reservationWallet.RW_Status = true;
                    await _reservationWalletService.Update(reservationWallet);
                    _logger.Information("updated reservationWallet");
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

using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.TransactionId;
using AS.Utility.Helpers;
using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AS.WithdrawApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Webhook")]
    public class WebhookController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ITransactionIdService _transactionIdService;
        private readonly IAESServices _aesServices;
        private readonly IWalletService _walletService;
        private readonly IDealRequestService _dealRequestService;
        private readonly ISMSSenderService _smsSenderService;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IMenuNotificationService _menuNotificationService;
        private readonly IMapper _mapper;
        private readonly IReservationWalletService _reservationWalletService;

        DealRequestModel dealRequest;

        public WebhookController(ILogger logger,
            ITransactionIdService transactionIdService,
            IAESServices aesServices,
            IWalletService walletService,
            IDealRequestService dealRequestService,
            ISMSSenderService smsSenderService,
            ITransactionService transactionService,
            IUserService userService,
            IMenuNotificationService menuNotificationService,
            IMapper mapper,
            IReservationWalletService reservationWalletService)
        {
            _logger = logger;
            _transactionIdService = transactionIdService;
            _aesServices = aesServices;
            _walletService = walletService;
            _dealRequestService = dealRequestService;
            _smsSenderService = smsSenderService;
            _transactionService = transactionService;
            _userService = userService;
            _menuNotificationService = menuNotificationService;
            _mapper = mapper;
            _reservationWalletService = reservationWalletService;
        }

        [HttpGet]
        [Route("Tron/{fhlowk}/{transactionIdCode}/{value}/{wal_Id}/{rw_Id}")]
        public async Task<HttpResponseMessage> Tron(string fhlowk,string transactionIdCode,double value,int wal_Id,int rw_Id)
        {
            try
            {
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                value = value.DivisionBy6Zero();

                var transactionId = _transactionIdService.Add(new TransactionIdModel
                {
                    TransactionIdCode = transactionIdCode,
                    Wal_Id = wal_Id
                });

                if (transactionId is null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "can not register transactionId");
                }

                dealRequest = _dealRequestService.DepositDealRequest(value, wal_Id, ServiceKeys.AmountDifferenceTether);

                if (dealRequest is null)
                {
                    _smsSenderService.SendToSupports($"یک تراکنش {value} ترون به کیف پول واریز شده است ولی درخواست آن یافت نشد");
                    return Request.CreateResponse(HttpStatusCode.OK, "Added txid");
                }

                dealRequest.Drq_Status = DealRequestStatus.Done;
                dealRequest.Txid = transactionIdCode;
                dealRequest.Drq_Amount = value;

                dealRequest.Drq_Status = DealRequestStatus.Done;
                dealRequest.Txid = transactionIdCode;
                dealRequest.Drq_Amount = value;

                var resultUpdateDealRequest = await _dealRequestService.UpdateGateway(_mapper.Map<DealRequestGatewayModel>(dealRequest));
                await UpdateBalance(resultUpdateDealRequest.Drq_TotalPrice, dealRequest.Usr_Id.Value);
                var resultUpdateWallet = await _walletService.UpdateLastTransaction(wal_Id);
                var resultApproveStatus = await _reservationWalletService.ApproveStatus(rw_Id);

                return Request.CreateResponse(HttpStatusCode.OK, "The operation was successful");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpGet]
        [Route("USDT/{fhlowk}/{transactionIdCode}/{value}/{wal_Id}/{rw_Id}")]
        public async Task<HttpResponseMessage> USDT(string fhlowk, string transactionIdCode, double value, int wal_Id, int rw_Id)
        {
            try
            {
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                value = value.DivisionBy6Zero();

                var transactionId = _transactionIdService.Add(new TransactionIdModel
                {
                    TransactionIdCode = transactionIdCode,
                    Wal_Id = wal_Id
                });

                if (transactionId is null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "can not register transactionId");
                }

                dealRequest = _dealRequestService.DepositDealRequest(value, wal_Id, ServiceKeys.AmountDifferenceTether);

                if (dealRequest is null)
                {
                    _smsSenderService.SendToSupports($"یک تراکنش {value} تتری به کیف پول واریز شده است ولی درخواست آن یافت نشد");
                    return Request.CreateResponse(HttpStatusCode.OK, "Added txid");
                }

                dealRequest.Drq_Status = DealRequestStatus.Done;
                dealRequest.Txid = transactionIdCode;
                dealRequest.Drq_Amount = value;

                var resultUpdateDealRequest = await _dealRequestService.UpdateGateway(_mapper.Map<DealRequestGatewayModel>(dealRequest));
                await UpdateBalance(resultUpdateDealRequest.Drq_TotalPrice, dealRequest.Usr_Id.Value);
                var resultUpdateWallet = await _walletService.UpdateLastTransaction(wal_Id);
                var resultApproveStatus = await _reservationWalletService.ApproveStatus(rw_Id);

                return Request.CreateResponse(HttpStatusCode.OK, "The operation was successful");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        private async Task<bool> UpdateBalance(double totalPrice, long userId)
        {
            try
            {
                var transaction = await _transactionService.AddTransacton(new Model.Transaction.RequestTransactionModel
                {
                    AdminUserId = ServiceKeys.AdmUsr_Id,
                    Amount = (long)totalPrice,
                    TransactionType = TransactionType.Sell,
                    UserId = userId
                });

                _logger.Information("added Transaction");

                var user = await _userService.GetByIdAsync(userId);
                user.Usr_BLNC_Balance = transaction.Tns_After;
                await _userService.Update(user);
                _logger.Information("updated userBalance");

                await _menuNotificationService.PushTransaction();
                _smsSenderService.SendToSupports($"کیف پول {user.Usr_FullName} به مبلغ {transaction.Tns_After} تومان شارژ شد");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
}

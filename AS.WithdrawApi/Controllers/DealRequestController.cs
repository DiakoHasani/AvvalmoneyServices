using AS.BL.Services;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
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
    [RoutePrefix("api/DealRequest")]
    public class DealRequestController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IDealRequestService _dealRequestService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly IMenuNotificationService _menuNotificationService;
        private readonly ISMSSenderService _smsSenderService;

        public DealRequestController(ILogger logger,
            IDealRequestService dealRequestService,
            IUserService userService,
            ITransactionService transactionService,
            IMenuNotificationService menuNotificationService,
            ISMSSenderService smsSenderService)
        {
            _logger = logger;
            _dealRequestService = dealRequestService;
            _userService = userService;
            _transactionService = transactionService;
            _menuNotificationService = menuNotificationService;
            _smsSenderService = smsSenderService;
        }

        [HttpGet]
        [Route("DepositDealRequest/{amount}/{walletId}/{amountDifference}")]
        public HttpResponseMessage DepositDealRequest(double amount, int walletId, double amountDifference)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dealRequestService.DepositDealRequest(amount, walletId, amountDifference));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpPost]
        [Route("UpdateGateway")]
        public async Task<HttpResponseMessage> UpdateGateway([FromBody] DealRequestGatewayModel model)
        {
            try
            {
                var dealRequest = await _dealRequestService.UpdateGateway(model);
                if (dealRequest != null)
                {
                    if (dealRequest.Drq_Status == DealRequestStatus.Done)
                    {
                        await UpdateBalance(dealRequest.Drq_TotalPrice, dealRequest.Usr_Id.Value);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, dealRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> Add([FromBody] RequestDealModel model)
        {
            try
            {
                var dealRequest = await _dealRequestService.Add(model);

                if (dealRequest != null)
                {
                    if (dealRequest.Drq_Status == DealRequestStatus.Done)
                    {
                        await UpdateBalance(dealRequest.Drq_TotalPrice, dealRequest.Usr_Id.Value);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.Created, dealRequest);
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

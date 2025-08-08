using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.SamanBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace AS.WithdrawApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/SamanBank")]
    public class SamanBankController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ISamanBankService _samanBankService;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IMenuNotificationService _menuNotificationService;
        private readonly ISMSSenderService _smsSenderService;
        public SamanBankController(ILogger logger,
            ISamanBankService samanBankService,
            ITransactionService transactionService,
            IUserService userService,
            IMenuNotificationService menuNotificationService,
            ISMSSenderService smsSenderService)
        {
            _logger = logger;
            _samanBankService = samanBankService;
            _transactionService = transactionService;
            _userService = userService;
            _menuNotificationService = menuNotificationService;
            _smsSenderService = smsSenderService;
        }

        [HttpPost]
        [Route("BillStatement")]
        public async Task<HttpResponseMessage> BillStatement([FromBody] List<BillStatementTransactionModel> model)
        {
            try
            {
                var result = await _samanBankService.BillStatement(model);
                foreach (var item in result)
                {
                    if (item.Result)
                    {
                        await UpdateBalance(item.Amount, item.Usr_Id);
                    }
                    else
                    {
                        //_smsSenderService.SendToSupports(item.Message);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
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
                    TransactionType = TransactionType.Deposite,
                    UserId = userId
                });

                _logger.Information("added Transaction");

                var user = await _userService.GetByIdAsync(userId);
                user.Usr_BLNC_Balance = transaction.Tns_After;
                await _userService.Update(user);
                _logger.Information("updated userBalance");

                await _menuNotificationService.PushTransaction();
                //_smsSenderService.SendToSupports($"کیف پول {user.Usr_FullName} به مبلغ {transaction.Tns_After} تومان شارژ شد");
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

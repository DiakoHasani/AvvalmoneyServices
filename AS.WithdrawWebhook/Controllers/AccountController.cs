using AS.BL.Services;
using AS.Model.General;
using AS.Model.WithdrawApi;
using AS.WithdrawWebhook.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AS.WithdrawWebhook.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;
        private readonly IRegisterJwtToken _registerJwtToken;
        public AccountController(IAccountService accountService,
            IRegisterJwtToken registerJwtToken)
        {
            _accountService = accountService;
            _registerJwtToken= registerJwtToken;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> Login([FromBody] LoginRequestModel model)
        {
            if (!ServiceKeys.WithdrawKey.Equals(model.fhlowk ?? ""))
            {
                await Task.Delay(TimeSpan.FromSeconds(20));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var result = _accountService.WidthdrawLogin(model);
            if (!result.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, result.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Created, _registerJwtToken.Register());
        }
    }
}

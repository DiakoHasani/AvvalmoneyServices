using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.WithdrawApi;
using AS.WithdrawApi.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace AS.WithdrawApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;
        private readonly IRegisterJwtToken _registerJwtToken;
        public AccountController(IAccountService accountService, ILogger logger,
            IRegisterJwtToken registerJwtToken)
        {
            _accountService = accountService;
            _logger = logger;
            _registerJwtToken = registerJwtToken;
        }

        [HttpPost(), Route("Login")]
        public async Task<HttpResponseMessage> Login([FromBody] LoginRequestModel model)
        {
            _logger.Information("Call Login");

            if (!ServiceKeys.WithdrawKey.Equals(model.fhlowk ?? ""))
            {
                await Task.Delay(TimeSpan.FromSeconds(20));
                _logger.Error("fhlowk is invalid.", new { fhlowk = model.fhlowk });
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            }

            if (!ModelState.IsValid)
            {
                _logger.Information("ModelState is Invalid", model);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var result = _accountService.WidthdrawLogin(model);
            _logger.Information("result WidthdrawLogin", result);
            if (!result.IsValid)
            {
                _logger.Error("user cant to login", result);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, result.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Created, _registerJwtToken.Register());
        }
    }
}

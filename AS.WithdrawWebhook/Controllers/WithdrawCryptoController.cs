using AS.BL.Services;
using AS.Model.General;
using AS.Model.WithdrawCryptoBot;
using AS.Model.WithdrawWebhook;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace AS.WithdrawWebhook.Controllers
{
    [RoutePrefix("api/WithdrawCrypto")]
    public class WithdrawCryptoController : ApiController
    {
        private readonly IAESServices _aesServices;
        private readonly IWithdrawCryptoApiService _withdrawCryptoApiService;
        public WithdrawCryptoController(IAESServices aesServices,
            IWithdrawCryptoApiService withdrawCryptoApiService)
        {
            _aesServices = aesServices;
            _withdrawCryptoApiService = withdrawCryptoApiService;
        }

        [Authorize]
        [HttpPost]
        [Route("Webhook")]
        public async Task<HttpResponseMessage> Webhook([FromBody] RequestWebhookWithdrawCryptoModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                if (!ServiceKeys.WithdrawKey.Equals(model.fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var WC_Id = Convert.ToInt64(_aesServices.BotDecrypt(model.WC_Id, ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv));
                var WC_Address = _aesServices.BotDecrypt(model.WC_Address, ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv);
                var WC_Amount = Convert.ToDouble(_aesServices.BotDecrypt(model.WC_Amount, ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv));
                var WC_CryptoType = Convert.ToInt32(_aesServices.BotDecrypt(model.WC_CryptoType, ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv));

                var token = await Login();
                if (token is null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var path = HostingEnvironment.MapPath("~/App_Data/TextFile.txt");
                var text = File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    text += "\n";
                }
                text += WC_Id;
                text += "\n";
                text += WC_Address;
                text += "\n";
                text += WC_Amount;
                text += "\n";
                text += WC_CryptoType;
                text += "\n";
                text += "Bearer " + token;
                text += "\n";
                text += "----------";
                File.WriteAllText(path, text);

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("SayHello")]
        public HttpResponseMessage SayHello()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello");
        }

        private async Task<string> Login()
        {
            var message = await _withdrawCryptoApiService.Login(new RequestLoginModel
            {
                Fhlowk = ServiceKeys.WithdrawKey,
                UserName = ServiceKeys.WithdrawUserName,
                Password = ServiceKeys.WithdrawPassword,
            });

            if (message.Result)
            {
                return message.Token;
            }

            return null;
        }
    }
}

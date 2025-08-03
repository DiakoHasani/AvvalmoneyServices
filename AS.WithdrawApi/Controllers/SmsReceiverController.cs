using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.SmsReceiver;
using AS.Model.WithdrawApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace AS.WithdrawApi.Controllers
{
    [RoutePrefix("api/SmsReceiver")]
    public class SmsReceiverController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IOptBotWithdrawService _optBotWithdrawService;
        public SmsReceiverController(ILogger logger,
            IOptBotWithdrawService optBotWithdrawService)
        {
            _logger = logger;
            _optBotWithdrawService = optBotWithdrawService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Receive")]
        public async Task<HttpResponseMessage> Receive([FromBody] RequestReceiveSmsModel model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Message) && model.Message.Contains("رمز"))
                {
                    await _optBotWithdrawService.Add(new PostOptRequestModel
                    {
                        Amount = 0,
                        CreateDate = DateTime.Now,
                        MaskCardNumber = "_",
                        OPT = model.Message
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseReceiveSmsModel
                {
                    ErrorCode = 0
                });
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResponseReceiveSmsModel
                {
                    ErrorCode = 1
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetLast/{fhlowk}")]
        public async Task<HttpResponseMessage> GetLast(string fhlowk)
        {
            if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
            {
                await Task.Delay(TimeSpan.FromSeconds(20));
                _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
            var opt = _optBotWithdrawService.GetLastOpt();
            if (opt is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                opt = _optBotWithdrawService.GetLastOpt();
            }

            if (opt is null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Code not received");
            }

            return Request.CreateResponse(HttpStatusCode.OK, opt.OPT);
        }
    }
}

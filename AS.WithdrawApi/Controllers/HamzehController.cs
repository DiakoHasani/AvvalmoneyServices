using AS.BL.Services;
using AS.Log;
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
    [RoutePrefix("api/Hamzeh")]
    public class HamzehController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ISMSSenderService _smsSenderService;
        public HamzehController(ILogger logger,
            ISMSSenderService smsSenderService)
        {
            _logger = logger;
            _smsSenderService = smsSenderService;
        }

        //gfdg: text
        //fhlowk:AuthenticationKey
        [HttpGet]
        public async Task<HttpResponseMessage> Batmani(string gfdg, string fhlowk)
        {
            _logger.Information("Call Batmani");
            try
            {
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                _smsSenderService.Send("09189799357", gfdg);

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

using AS.BL.Services;
using AS.Log;
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
    [RoutePrefix("api/Contradiction")]
    public class ContradictionController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IContradictionService _contradictionService;
        private readonly ISMSSenderService _smsSenderService;

        public ContradictionController(ILogger logger,
            IContradictionService contradictionService,
            ISMSSenderService smsSenderService)
        {
            _logger = logger;
            _contradictionService = contradictionService;
            _smsSenderService= smsSenderService;
        }

        [HttpGet]
        [Route("Check/{fhlowk}")]
        public async Task<HttpResponseMessage> Check(string fhlowk)
        {
            try
            {
                _logger.Information("call Check");

                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var undecideds = _contradictionService.GetUndecided();

                undecideds = await _contradictionService.Update(undecideds);

                _logger.Information("get undecideds", undecideds);

                var result = "";
                foreach (var undecided in undecideds)
                {
                    result += $"{undecided.WC_Id} , ";
                }

                if (!string.IsNullOrWhiteSpace(result))
                {
                    _smsSenderService.SendToSupports($"لیست تراکنش های که وضعیت آن ها به در حال انتظار تغییر یافته \n{result}");
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, undecideds);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

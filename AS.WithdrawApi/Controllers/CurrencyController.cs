using AS.BL.Services;
using AS.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AS.WithdrawApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Currency")]
    public class CurrencyController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ILogger logger,
            ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        [Route("GetCur_IdByISOCode/{isoCode}")]
        public HttpResponseMessage GetCur_IdByISOCode(string isoCode)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,_currencyService.GetCur_IdByISOCode(isoCode));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

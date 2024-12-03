using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.CurrencyPriceHistory;
using AS.Model.General;
using AutoMapper;
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
    [RoutePrefix("api/CurrencyPriceHistory")]
    public class CurrencyPriceHistoryController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private readonly ILifeLogBotWithdrawService _lifeLogBotWithdrawService;

        public CurrencyPriceHistoryController(ILogger logger,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            ILifeLogBotWithdrawService lifeLogBotWithdrawService)
        {
            _logger = logger;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _lifeLogBotWithdrawService = lifeLogBotWithdrawService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> Add(CurrencyPriceHistoryModel model)
        {
            try
            {
                await _lifeLogBotWithdrawService.Add(ServiceKeys.UpdatePriceBotKey);
                _lifeLogBotWithdrawService.CheckLifeAllBots();

                await _currencyPriceHistoryService.Add(model);
                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [Route("GetByCur_Id/{cur_id}")]
        public HttpResponseMessage GetByCur_Id(int cur_id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _currencyPriceHistoryService.GetByCur_Id(cur_id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

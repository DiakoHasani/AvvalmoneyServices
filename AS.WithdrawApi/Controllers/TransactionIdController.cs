using AS.BL.Services;
using AS.Log;
using AS.Model.TransactionId;
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
    [RoutePrefix("api/TransactionId")]
    public class TransactionIdController : BaseController
    {
        private readonly ILogger _logger;
        private readonly ITransactionIdService _transactionIdService;

        public TransactionIdController(ILogger logger,
            ITransactionIdService transactionIdService)
        {
            _logger = logger;
            _transactionIdService = transactionIdService;
        }

        [HttpGet]
        [Route("CheckExistTransactionIdCode/{transactionIdCode}")]
        public HttpResponseMessage CheckExistTransactionIdCode(string transactionIdCode)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _transactionIdService.CheckExistTransactionIdCode(transactionIdCode));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<HttpResponseMessage> Add([FromBody] TransactionIdModel model)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.Created, await _transactionIdService.Add(model));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

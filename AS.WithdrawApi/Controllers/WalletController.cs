using AS.BL.Services;
using AS.Log;
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
    [RoutePrefix("api/Wallet")]
    public class WalletController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IWalletService _walletService;

        public WalletController(ILogger logger,
            IWalletService walletService)
        {
            _logger = logger;
            _walletService = walletService;
        }

        [Route("GetWalletById/{Wal_Id}")]
        public async Task<HttpResponseMessage> GetWalletById(int Wal_Id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await _walletService.GetWalletById(Wal_Id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpGet]
        [Route("UpdateLastTransaction/{Wal_Id}")]
        public async Task<HttpResponseMessage> UpdateLastTransaction(long Wal_Id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await _walletService.UpdateLastTransaction(Wal_Id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

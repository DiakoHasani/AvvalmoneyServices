using AS.BL.Services;
using AS.DAL;
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
        private readonly ISMSSenderService _smsSenderService;

        public WalletController(ILogger logger,
            IWalletService walletService,
            ISMSSenderService smsSenderService)
        {
            _logger = logger;
            _walletService = walletService;
            _smsSenderService = smsSenderService;
        }

        [Route("GetWalletById/{Wal_Id}")]
        public async Task<HttpResponseMessage> GetWalletById(int Wal_Id)
        {
            try
            {
                if (! await _walletService.CheckWalletKey(Wal_Id))
                {
                    _smsSenderService.SendToSupports($"شناسه ولت با آیدی {Wal_Id} با اطلاعات ولت مطابقت ندارد");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
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
        public async Task<HttpResponseMessage> UpdateLastTransaction(int Wal_Id)
        {
            try
            {
                if (!await _walletService.CheckWalletKey(Wal_Id))
                {
                    _smsSenderService.SendToSupports($"شناسه ولت با آیدی {Wal_Id} با اطلاعات ولت مطابقت ندارد");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }
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

using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
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
    [RoutePrefix("api/UserWalletReservation")]
    public class UserWalletReservationController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IUserWalletReservationService _userWalletReservationService;
        public UserWalletReservationController(ILogger logger,
            IUserWalletReservationService userWalletReservationService)
        {
            _logger = logger;
            _userWalletReservationService = userWalletReservationService;
        }

        [Route("GetUserWalletReservations/{currencyType}")]
        public HttpResponseMessage GetUserWalletReservations(CurrencyType currencyType)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _userWalletReservationService.GetUserWalletReservations(currencyType));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpGet]
        [Route("UpdateTxid/{UWR_Id}/{Txid}")]
        public async Task<HttpResponseMessage> UpdateTxid(int UWR_Id, string Txid)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await _userWalletReservationService.UpdateTxid(UWR_Id, Txid));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

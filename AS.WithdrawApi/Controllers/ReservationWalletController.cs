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
    [RoutePrefix("api/ReservationWallet")]
    public class ReservationWalletController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IReservationWalletService _reservationWalletService;
        public ReservationWalletController(ILogger logger,
            IReservationWalletService reservationWalletService)
        {
            _logger = logger;
            _reservationWalletService = reservationWalletService;
        }

        [Route("GetReservations")]
        public HttpResponseMessage GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _reservationWalletService.GetReservations(fromDate, toDate, cryptoType));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [HttpGet]
        [Route("ApproveStatus/{Rw_Id}")]
        public async Task<HttpResponseMessage> ApproveStatus(int Rw_Id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,await _reservationWalletService.ApproveStatus(Rw_Id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

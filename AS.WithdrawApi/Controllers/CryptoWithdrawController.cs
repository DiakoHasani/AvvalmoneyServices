using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.WithdrawCrypto;
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
    [RoutePrefix("api/CryptoWithdraw")]
    public class CryptoWithdrawController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IWithdrawCryptoService _withdrawCryptoService;
        private readonly IDealRequestService _dealRequestService;
        public CryptoWithdrawController(ILogger logger,
            IWithdrawCryptoService withdrawCryptoService,
            IDealRequestService dealRequestService)
        {
            _logger = logger;
            _withdrawCryptoService = withdrawCryptoService;
            _dealRequestService = dealRequestService;
        }

        //fhlowk: WithdrawKey
        [HttpGet]
        [Route("GetAvailable/{fhlowk}")]
        public async Task<HttpResponseMessage> GetAvailable(string fhlowk)
        {
            try
            {
                _logger.Information("Call GetLast");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var cryptoWithdrawModel = _withdrawCryptoService.GetPendingWithdraw(WithdrawCryptoStatus.Pending);
                if (cryptoWithdrawModel is null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                var withdrawCrypto = await _withdrawCryptoService.GetById(cryptoWithdrawModel.WC_Id);
                withdrawCrypto.WC_Status = (int)WithdrawCryptoStatus.PassToRobot;
                await _withdrawCryptoService.Update(withdrawCrypto);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        //fhlowk: WithdrawKey
        [HttpGet]
        [Route("GetLast/{fhlowk}")]
        public async Task<HttpResponseMessage> GetLast(string fhlowk)
        {
            try
            {
                _logger.Information("Call GetLast");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var cryptoWithdrawModel = _withdrawCryptoService.GetPendingWithdraw(WithdrawCryptoStatus.PassToRobot);
                if (cryptoWithdrawModel is null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                var withdrawCrypto = await _withdrawCryptoService.GetById(cryptoWithdrawModel.WC_Id);
                withdrawCrypto.WC_Status = (int)WithdrawCryptoStatus.RobotInProgress;
                await _withdrawCryptoService.Update(withdrawCrypto);

                _logger.Information("get cryptoWithdraw", cryptoWithdrawModel);
                return Request.CreateResponse(HttpStatusCode.OK, cryptoWithdrawModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        //fhlowk: WithdrawKey
        //sdgdfg: Status
        //iooitr: WithdrawCryptoId
        //qwewr: Txid
        [HttpPost]
        [Route("Pay")]
        public async Task<HttpResponseMessage> Pay(RequestPayWithdrawCryptoModel model)
        {
            try
            {
                _logger.Information("call Pay");
                if (!ServiceKeys.WithdrawKey.Equals(model.fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = model.fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var cryptoWithdraw = await _withdrawCryptoService.GetById(model.iooitr);
                if (cryptoWithdraw is null)
                {
                    _logger.Error("cryptoWithdraw is null");
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "notfound cryptoWithdraw");
                }

                _logger.Information("get cryptoWithdraw", new { WC_Id = model.iooitr });

                if (!(model.sdgdfg == WithdrawCryptoStatus.Success || model.sdgdfg == WithdrawCryptoStatus.Fail))
                {
                    _logger.Error("invalid Status because you should pass success or fail");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid your request");
                }

                cryptoWithdraw.WC_Status = (int)model.sdgdfg;

                await _withdrawCryptoService.Update(cryptoWithdraw);

                var dealRequest = _dealRequestService.GetById(cryptoWithdraw.Drq_Id);
                if (dealRequest is null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "notfound dealRequest");
                }
                dealRequest.Txid = model.qwewr;
                await _dealRequestService.Update(dealRequest);

                return Request.CreateResponse(HttpStatusCode.OK, "The operation was done");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

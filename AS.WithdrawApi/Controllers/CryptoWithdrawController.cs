using AS.BL.Catches;
using AS.BL.Services;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.WithdrawCrypto;
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
    [RoutePrefix("api/CryptoWithdraw")]
    public class CryptoWithdrawController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IWithdrawCryptoService _withdrawCryptoService;
        private readonly IDealRequestService _dealRequestService;
        private readonly IMapper _mapper;
        private readonly IAESServices _aesServices;
        private readonly ISMSSenderService _smsSenderService;
        private readonly ILifeLogBotWithdrawService _lifeLogBotWithdrawService;
        private readonly ITronScanService _tronScanService;
        private readonly ICryptoWithdrawCatch _cryptoWithdrawCatch;

        public CryptoWithdrawController(ILogger logger,
            IWithdrawCryptoService withdrawCryptoService,
            IDealRequestService dealRequestService,
            IMapper mapper,
            IAESServices aesServices,
            ISMSSenderService smsSenderService,
            ILifeLogBotWithdrawService lifeLogBotWithdrawService,
            ITronScanService tronScanService,
            ICryptoWithdrawCatch cryptoWithdrawCatch)
        {
            _logger = logger;
            _withdrawCryptoService = withdrawCryptoService;
            _dealRequestService = dealRequestService;
            _mapper = mapper;
            _aesServices = aesServices;
            _smsSenderService = smsSenderService;
            _lifeLogBotWithdrawService = lifeLogBotWithdrawService;
            _tronScanService = tronScanService;
            _cryptoWithdrawCatch= cryptoWithdrawCatch;
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

                await _lifeLogBotWithdrawService.Add(ServiceKeys.WithdrawCryptoBotKey);
                await _lifeLogBotWithdrawService.CheckLifeAllBots();

                var cryptoWithdrawModel = _withdrawCryptoService.GetPendingWithdraw(WithdrawCryptoStatus.Pending);
                if (cryptoWithdrawModel is null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                var withdrawCrypto = await _withdrawCryptoService.GetById(cryptoWithdrawModel.WC_Id);

                if ((CryptoType)withdrawCrypto.WC_CryptoType == CryptoType.Tron)
                {
                    var tronTransaction = await _tronScanService.GetTransfered(ServiceKeys.TronWallet, 5);
                    if (await _withdrawCryptoService.CheckRepeated(tronTransaction.Data, withdrawCrypto.WC_Id))
                    {
                        if (_cryptoWithdrawCatch.AccessToSend())
                        {
                            _smsSenderService.SendToSupports("تراکنشی با مبلغ تکراری برای انتقال وجود دارد لطفا چک کنید");
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

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

                withdrawCrypto.WC_Amount = Math.Round(withdrawCrypto.WC_Amount, 2);
                _logger.Information("get cryptoWithdraw", cryptoWithdrawModel);

                var createDate = withdrawCrypto.WC_CreateDate;

                return Request.CreateResponse(HttpStatusCode.OK, new ResponseWithdrawCryptoEncryptedModel
                {
                    WC_Id = _aesServices.BotEncrypt(withdrawCrypto.WC_Id.ToString(), ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv),
                    WC_Address = _aesServices.BotEncrypt(withdrawCrypto.WC_Address, ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv),
                    WC_Amount = _aesServices.BotEncrypt(withdrawCrypto.WC_Amount.ToString(), ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv),
                    WC_CryptoType = _aesServices.BotEncrypt(withdrawCrypto.WC_CryptoType.ToString(), ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv),
                    WC_CreateDate = _aesServices.BotEncrypt($"{createDate.Year}-{createDate.Month}-{createDate.Day} {createDate.Hour}:{createDate.Minute}:{createDate.Second}", ServiceKeys.BotEncriptionKey, ServiceKeys.BotEncriptionIv),
                    WC_Sign = withdrawCrypto.WC_Sign
                });
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

                await _dealRequestService.UpdateTxid(cryptoWithdraw.Drq_Id, model.qwewr);

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

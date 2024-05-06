using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.WithdrawApi;
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
    [RoutePrefix("api/Withraw")]
    public class WithdrawController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IUserWithdrawService _userWithdrawService;
        private readonly IUserBankCardService _userBankCardService;
        private readonly IUserService _userService;
        private readonly IBotInfoWithdrawService _botInfoWithdrawService;
        private readonly IBotWithdrawTypeService _botWithdrawTypeService;
        private readonly IBotCardsWithdrawService _botCardsWithdrawService;
        private readonly ILifeLogBotWithdrawService _lifeLogBotWithdrawService;
        private readonly IOptBotWithdrawService _optBotWithdrawService;
        private readonly ISMSSenderService _smsSenderService;
        public WithdrawController(IUserWithdrawService userWithdrawService,
            IUserBankCardService userBankCardService,
            IUserService userService,
            IBotInfoWithdrawService botInfoWithdrawService,
            IBotWithdrawTypeService botWithdrawTypeService,
            ILogger logger,
            IBotCardsWithdrawService botCardsWithdrawService,
            ILifeLogBotWithdrawService lifeLogBotWithdrawService,
            IOptBotWithdrawService optBotWithdrawService,
            ISMSSenderService smsSenderService)
        {
            _userWithdrawService = userWithdrawService;
            _userBankCardService = userBankCardService;
            _userService = userService;
            _botInfoWithdrawService = botInfoWithdrawService;
            _botWithdrawTypeService = botWithdrawTypeService;
            _logger = logger;
            _botCardsWithdrawService = botCardsWithdrawService;
            _lifeLogBotWithdrawService = lifeLogBotWithdrawService;
            _optBotWithdrawService = optBotWithdrawService;
            _smsSenderService = smsSenderService;
        }

        //fhlowk:AuthenticationKey
        [Route("GetBotAvailable/{fhlowk}")]
        public async Task<HttpResponseMessage> GetBotAvailable(string fhlowk)
        {
            try
            {
                _logger.Information("Call GetBotAvailable");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                double amount = 0;
                var withdraw = _userWithdrawService.GetLastWaitingWithdraw();
                if (withdraw is null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                #region در اینجا وضعیت تسویه حساب بررسی می شود ، وضعیت تعدادهای ترای و مبلغ بالای ده میلیون است یا خیر
                var checkWithdrawPayment = await _userWithdrawService.CheckWithdrawPayment(withdraw);
                _logger.Information("call CheckWithdrawPayment", checkWithdrawPayment);

                var userBankCard = await _userBankCardService.GetbyIdAsync(withdraw.UBC_Id);
                var user = await _userService.GetByIdAsync(withdraw.Usr_Id);

                if (checkWithdrawPayment == CheckWithdrawPaymentStatus.ErrorWithdrawTry)
                {
                    _smsSenderService.SendToSupports($"شماره کارت: {userBankCard.UBC_CardNumber} \n مبلغ :{withdraw.Wit_Amount.ToString("N0")}  " +
                        $"\n  نام کاربر: {user.Usr_FullName}  موبایل: {user.Usr_Phone ?? ""} \n" +
                        $"   تاریخ :{withdraw.Wit_DateCreate} \n خطای بات تسویه حساب");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else if (checkWithdrawPayment == CheckWithdrawPaymentStatus.WithdrawAmountIsMoreMaximumCard)
                {
                    _smsSenderService.SendToSupports($"شماره کارت: {userBankCard.UBC_CardNumber}  \n شماره شبا:{userBankCard.UBC_Shaba ?? ""} \n" +
                        $"  مبلغ :{withdraw.Wit_Amount.ToString("N0")} \n  نام کاربر: {user.Usr_FullName} \n  موبایل: {user.Usr_Phone}" +
                        $"  تاریخ :{withdraw.Wit_DateCreate} \n بالای ده میلیون");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    amount = withdraw.Wit_Amount;
                }
                #endregion

                var withrawTypes = _botWithdrawTypeService.GetByWithdrawId(withdraw.Wit_Id);
                if (withrawTypes.Count == ServiceKeys.ChangeNumberBot)
                {
                    //for change bot
                }
                else if (withrawTypes.Count >= ServiceKeys.RepeatNumberRequest)
                {
                    await _userWithdrawService.ErrorRepeatWithdraw(withdraw);
                    _smsSenderService.SendToSupports("خطای ریپیت در بات تسویه حساب رخ داده است");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                var resultUpdateLimites = await _botCardsWithdrawService.UpdateLimites();
                if (!string.IsNullOrWhiteSpace(resultUpdateLimites))
                {
                    _smsSenderService.SendToSupports(resultUpdateLimites);
                }

                var bots = _botInfoWithdrawService.GetAll();
                if (!bots.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                _logger.Information("get all bots", bots);

                Guid? botKey = null;

                foreach (var bot in bots)
                {
                    var cards = _botCardsWithdrawService.GetByBankKey(bot.Key);
                    _logger.Information("get cards by bank key", cards);
                    if (cards.Any())
                    {
                        var card = cards.Where(o => o.Limit >= amount).OrderBy(o => o.Limit).FirstOrDefault();
                        if (card != null)
                        {
                            await _botWithdrawTypeService.Add(bot.BotType, withdraw.Wit_Id);
                            botKey = card.BankKey;
                            _logger.Information("Choosed Available Bot", new { botKey = botKey });
                            break;
                        }
                    }
                }
                if (!botKey.HasValue)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new BotAvailableModel { BotKey = botKey.ToString() });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        //xyz:botKey
        //fhlowk:AuthenticationKey
        [Route("GetLastWithraw/{xyz}/{fhlowk}")]
        public async Task<HttpResponseMessage> GetLastWithraw(Guid xyz, string fhlowk)
        {
            try
            {
                _logger.Information("Call GetLastWithraw");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var botInfo = _botInfoWithdrawService.GetByKey(xyz);
                _logger.Information("get botInfo by key", new { botInfoID = botInfo.Biw_Id });
                if (botInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                var withdraw = _userWithdrawService.GetLastWaitingWithdraw();
                if (withdraw == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                _logger.Information("call GetLastWaitingWithdraw", new { withdrawId = withdraw.Wit_Id });

                var checkWithdrawPayment = await _userWithdrawService.CheckWithdrawPayment(withdraw);
                _logger.Information("call CheckWithdrawPayment", checkWithdrawPayment);

                var userBankCard = await _userBankCardService.GetbyIdAsync(withdraw.UBC_Id);
                var user = await _userService.GetByIdAsync(withdraw.Usr_Id);

                if (checkWithdrawPayment == CheckWithdrawPaymentStatus.ErrorWithdrawTry)
                {
                    _smsSenderService.SendToSupports($"شماره کارت: {userBankCard.UBC_CardNumber} \n مبلغ :{withdraw.Wit_Amount.ToString("N0")}  " +
                        $"\n  نام کاربر: {user.Usr_FullName}  موبایل: {user.Usr_Phone ?? ""} \n" +
                        $"   تاریخ :{withdraw.Wit_DateCreate} \n خطای بات تسویه حساب");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else if (checkWithdrawPayment == CheckWithdrawPaymentStatus.WithdrawAmountIsMoreMaximumCard)
                {
                    _smsSenderService.SendToSupports($"شماره کارت: {userBankCard.UBC_CardNumber}  \n شماره شبا:{userBankCard.UBC_Shaba ?? ""} \n" +
                        $"  مبلغ :{withdraw.Wit_Amount.ToString("N0")} \n  نام کاربر: {user.Usr_FullName} \n  موبایل: {user.Usr_Phone}" +
                        $"  تاریخ :{withdraw.Wit_DateCreate} \n بالای ده میلیون");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    await _lifeLogBotWithdrawService.Add(xyz);
                    botInfo.LastSeen = DateTime.Now;
                    await _botInfoWithdrawService.Update(botInfo);

                    withdraw.Try = withdraw.Try != null ? (withdraw.Try + 1) : 1;
                    await _userWithdrawService.Update(withdraw);
                    _logger.Information("updated withdraw.Try", new { Try = withdraw.Try });

                    return Request.CreateResponse(HttpStatusCode.OK, new LastWithrawResponseModel
                    {
                        CardNumber = userBankCard.UBC_CardNumber,
                        Id = withdraw.Wit_Id,
                        Price = (withdraw.Wit_Amount * 10).ToString(),
                        WithrawType = (int)WithdrawType.Withraw
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        [AllowAnonymous]
        [Route("PostOpt")]
        public async Task<WithdrawMessageModel> PostOpt([FromBody] PostOptRequestModel model)
        {
            try
            {
                _logger.Information("called PostOpt");
                await _optBotWithdrawService.Add(model);

                return new WithdrawMessageModel
                {
                    Code = 200,
                    MessageText = "added",
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new WithdrawMessageModel
                {
                    Code = 500,
                    MessageText = "خطای رخ داده است",
                    Result = false
                };
            }
        }

        //jkd:price
        //fhlowk:AuthenticationKey
        [Route("GetOpt/{jkd}/{fhlowk}")]
        public async Task<HttpResponseMessage> GetOpt(double jkd, string fhlowk)
        {
            try
            {
                _logger.Information("Called GetOpt");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
                var opt = _optBotWithdrawService.GetLastOptByAmount(jkd);

                if (opt == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    opt = _optBotWithdrawService.GetLastOptByAmount(jkd);
                }

                if (opt != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, opt.OPT);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        //lkq:witrawId
        //tty:withrawType
        //fsd:botKey
        //fhlowk:AuthenticationKey
        [Route("GetBankCard/{lkq}/{tty}/{fsd}/{fhlowk}")]
        public async Task<HttpResponseMessage> GetBankCard(long lkq, WithdrawType tty, Guid fsd, string fhlowk)
        {
            try
            {
                _logger.Information("Called GetBankCard");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var withdraw = await _userWithdrawService.GetByIdAsync(lkq);
                _logger.Information("called withdraw.GetByIdAsync", new { withdrawId = withdraw.Wit_Id });

                var resultUpdateLimites = await _botCardsWithdrawService.UpdateLimites();
                if (!string.IsNullOrWhiteSpace(resultUpdateLimites))
                {
                    _smsSenderService.SendToSupports(resultUpdateLimites);
                }

                var botCard = _botCardsWithdrawService.GetAvailableBotCard(withdraw.Wit_Amount, fsd);
                _logger.Information("called GetAvailableBotCard", new { botCardID = botCard.Bcw_Id });
                if (botCard == null)
                {
                    _smsSenderService.SendToSupports("سقف تمامی کارت ها در بخش بات تسویه حساب پر شده است");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new BotCardsWithdrawResponseModel
                {
                    CVV2 = botCard.CVV2,
                    Index = botCard.Index
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }

        //iom:withrawId
        //dsf:index
        //avj:status
        //tuj:WithrawType
        //idn:botKey
        //fhlowk:AuthenticationKey
        [HttpGet]
        [Route("PaymentWithraw/{iom}/{dsf}/{avj}/{tuj}/{idn}/{fhlowk}")]
        public async Task<HttpResponseMessage> PaymentWithraw(int iom, string dsf, PaymentWithdrawStatus avj, WithdrawType tuj, Guid idn, string fhlowk)
        {
            try
            {
                _logger.Information("Called PaymentWithraw");
                if (!ServiceKeys.WithdrawKey.Equals(fhlowk ?? ""))
                {
                    await Task.Delay(TimeSpan.FromSeconds(20));
                    _logger.Error("fhlowk is invalid.", new { fhlowk = fhlowk });
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                }

                var botCard = _botCardsWithdrawService.GetByIndex(dsf, idn);
                _logger.Information("called botCard.GetByIndex", new { botCardId = botCard.Bcw_Id });
                if (botCard == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                double price = 0;
                var withdraw = await _userWithdrawService.GetByIdAsync(iom);
                _logger.Information("called withdraw.GetByIdAsync", new { withdrawID = withdraw.Wit_Id });
                if (withdraw == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                price = withdraw.Wit_Amount;

                if (avj == PaymentWithdrawStatus.Accepted)
                {
                    withdraw.Bot = true;
                    //withdraw.ApprovedDate = DateTime.Now;
                    withdraw.Wit_Status = true;
                }
                else
                {
                    withdraw.Wit_Status = false;
                }

                await _userWithdrawService.Update(withdraw);
                _logger.Information("updated withdraw", new { withdrawId = withdraw.Wit_Id });

                if (avj == PaymentWithdrawStatus.Accepted)
                {
                    botCard.Limit -= price;
                    await _botCardsWithdrawService.Update(botCard);
                    _logger.Information("updated botCard", new { botCardId = botCard.Bcw_Id });
                }
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "");
            }
        }
    }
}

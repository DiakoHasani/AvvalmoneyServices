using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Utility.Helpers;
using AS.Model.Paystar;
using System.Threading.Tasks;
using AS.BL.Catches;
using System.Web.Services.Description;
using System.Reflection;
using AS.Model.Enums;

namespace Gateway.Controllers
{
    public class PaystarController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAESServices _aesServices;
        private readonly IPaystarService _paystarService;
        private readonly IPaystarCatch _paystarCatch;

        public PaystarController(ILogger logger,
            IAESServices aesServices,
            IPaystarService paystarService,
            IPaystarCatch paystarCatch)
        {
            _logger = logger;
            _aesServices = aesServices;
            _paystarService = paystarService;
            _paystarCatch = paystarCatch;
        }

        public async Task<ActionResult> Payment(string input)
        {
            try
            {
                //var encript = _aesServices.Encrypt($"{20000}#{Guid.NewGuid()}", ServiceKeys.GatewayEncriptionKey);

                var parameters = _aesServices.Decrypt(input, ServiceKeys.GatewayEncriptionKey).Split('#');
                var amount = parameters[0].ToInt64();
                var orderId = parameters[1];
                var gatewayTransactionType = (GatewayTransactionType)Convert.ToInt32(parameters[2]);
                var callback = $"{HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/Paystar/Verify";

                var createResponse = await _paystarService.Create(new CreateRequestPaystarModel
                {
                    Amount = amount,
                    Callback = callback,
                    OrderId = orderId,
                    Sign = _paystarService.GenerateCreateSign(amount, orderId, callback)
                });

                if (createResponse is null)
                {
                    ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                    return View();
                }

                if (createResponse.Status == 1)
                {
                    var catchResult = _paystarCatch.Add(new PaystarCatchModel
                    {
                        OrderId = orderId,
                        RefNum = createResponse.Data.RefNum,
                        Amount = amount,
                        GatewayTransactionType= gatewayTransactionType
                    });

                    if (catchResult is null)
                    {
                        ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                        return View();
                    }

                    return Redirect("https://core.paystar.ir/api/pardakht/payment?token=" + createResponse.Data.Token);
                }

                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }
        }

        public async Task<ActionResult> Verify(int status, string order_id, string ref_num, string card_number, string tracking_code)
        {
            try
            {
                var result = false;
                var message = "";
                var response = "";
                var returnUrl = $"{ServiceKeys.PanelAvvalMoneyUrl}Geteway/";

                var catchResult = _paystarCatch.Get(order_id);
                if (catchResult is null)
                {
                    message = "خطای رخ داده است به پشتیبانی اطلاع دهید ";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(card_number) && string.IsNullOrWhiteSpace(tracking_code))
                    {
                        message = "پرداخت انجام نشد";
                    }
                    else
                    {
                        response = await _paystarService.Verify(new VerifyRequestPaystarModel
                        {
                            Amount = catchResult.Amount,
                            RefNum = catchResult.RefNum,
                            Sign = _paystarService.GenerateVerifySign(catchResult.Amount, catchResult.RefNum, card_number, tracking_code)
                        });

                        result = true;
                    }
                }

                if (catchResult.GatewayTransactionType == GatewayTransactionType.Buy)
                {
                    returnUrl += $"PaystarVerify?input={_aesServices.Encrypt(_paystarService.GenerateVerifyResult(result, message, response, order_id), ServiceKeys.GatewayEncriptionKey)}";
                }
                else
                {
                    returnUrl += $"PaystarDepositVerify?input={_aesServices.Encrypt(_paystarService.GenerateVerifyResult(result, message, response, order_id), ServiceKeys.GatewayEncriptionKey)}";
                }

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید ";
                return View();
            }
        }
    }
}
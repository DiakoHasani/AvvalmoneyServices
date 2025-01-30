using AS.BL.Catches;
using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.ZarinPal;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gateway.Controllers
{
    public class ZarinpalController : Controller
    {
        private readonly IZarinpalService _zarinpalService;
        private readonly ILogger _logger;
        private readonly IAESServices _aesServices;
        private readonly IZarinPalCatch _zarinPalCatch;

        public ZarinpalController(IZarinpalService zarinpalService,
            ILogger logger,
            IAESServices aesServices,
            IZarinPalCatch zarinPalCatch)
        {
            _zarinpalService = zarinpalService;
            _logger = logger;
            _aesServices = aesServices;
            _zarinPalCatch = zarinPalCatch;
        }
        public async Task<ActionResult> Create(string input)
        {
            var parameters = _aesServices.Decrypt(input, ServiceKeys.GatewayEncriptionKey).Split('#');
            var amount = parameters[0].ToInt64();
            var orderId = parameters[1];
            var gatewayTransactionType = (GatewayTransactionType)Convert.ToInt32(parameters[2]);
            var callback = $"{HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/Zarinpal/Verify";

            var response = await _zarinpalService.Payment(new ZarinPalPaymentRequestModel
            {
                Amount = amount,
                OrderId = orderId,
                CallbackUrl = callback,
                Description = "پرداخت سفارش کاربر",
                MerchantId = ServiceKeys.ZarinpalMerchantId
            });

            if (response is null)
            {
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }

            if (response.Data.Code != 100)
            {
                ViewBag.Message = GenerateErrorText(response.Errors);
                return View();
            }

            var catchResult = _zarinPalCatch.Add(new ZarinPalCatchModel
            {
                Amount = amount,
                Authority = response.Data.Authority,
                GatewayTransactionType = gatewayTransactionType,
                OrderId = orderId
            });

            if (catchResult is null)
            {
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }

            return Redirect($"https://payment.zarinpal.com/pg/StartPay/{response.Data.Authority}");
        }

        public async Task<ActionResult> Verify(string Authority,string Status)
        {
            var result = false;
            var message = "";
            var response = "";
            var returnUrl = $"{ServiceKeys.PanelAvvalMoneyUrl}Geteway/";

            var catchResult = _zarinPalCatch.Get(Authority);
            if (catchResult is null)
            {
                message = "خطای رخ داده است به پشتیبانی اطلاع دهید ";
            }
            else
            {
                if (Status != "OK")
                {
                    message = "پرداخت انجام نشد";
                }
                else
                {
                    response = await _zarinpalService.Verify(new ZarinPalVerifyRequestModel
                    {
                        Amount=catchResult.Amount,
                        Authority = catchResult.Authority,
                        MerchantId= ServiceKeys.ZarinpalMerchantId
                    });
                    result = true;
                }
            }

            if (catchResult.GatewayTransactionType == GatewayTransactionType.Buy)
            {
                returnUrl += $"ZarinPalVerify?input={_aesServices.Encrypt(_zarinpalService.GenerateVerifyResult(result, message, response, catchResult.OrderId), ServiceKeys.GatewayEncriptionKey)}";
            }
            else
            {
                returnUrl += $"ZarinPalDepositVerify?input={_aesServices.Encrypt(_zarinpalService.GenerateVerifyResult(result, message, response, catchResult.OrderId), ServiceKeys.GatewayEncriptionKey)}";
            }

            return Redirect(returnUrl);
        }

        private string GenerateErrorText(List<string> errors)
        {
            var result = "";
            foreach (var error in errors)
            {
                result += error;
                result += "\n";
            }
            return result;
        }
    }
}
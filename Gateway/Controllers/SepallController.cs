using AS.BL.Catches;
using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Sepal;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gateway.Controllers
{
    public class SepallController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISepalService _sepalService;
        private readonly IAESServices _aesServices;
        private readonly ISepalCatch _sepalCatch;

        public SepallController(ISepalService sepalService,
            ILogger logger,
            IAESServices aesServices,
            ISepalCatch sepalCatch)
        {
            _sepalService = sepalService;
            _logger = logger;
            _aesServices = aesServices;
            _sepalCatch = sepalCatch;
        }

        public async Task<ActionResult> Create(string input)
        {
            var parameters = _aesServices.Decrypt(input, ServiceKeys.GatewayEncriptionKey).Split('#');
            var amount = parameters[0].ToInt64();
            var orderId = parameters[1];
            var gatewayTransactionType = (GatewayTransactionType)Convert.ToInt32(parameters[2]);
            var callback = $"{HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/Sepall/Verify";

            var response = await _sepalService.Request(new SepalRequestModel
            {
                Amount = amount,
                ApiKey = ServiceKeys.SepalKey,
                CallbackUrl = callback,
                InvoiceNumber = orderId
            });

            if (response is null)
            {
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }

            if (!response.Status)
            {
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }

            var catchResult = _sepalCatch.Add(new SepalCatchModel
            {
                Amount = amount,
                GatewayTransactionType = gatewayTransactionType,
                OrderId = orderId,
                PaymentNumber = response.PaymentNumber
            });

            if (catchResult is null)
            {
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }

            return Redirect($"https://sepal.ir/payment/{response.PaymentNumber}");

        }

        public async Task<ActionResult> Verify(string status, string paymentNumber, string invoiceNumber)
        {
            var result = false;
            var message = "";
            var response = "";
            var returnUrl = $"{ServiceKeys.PanelAvvalMoneyUrl}Geteway/";

            var catchResult = _sepalCatch.Get(invoiceNumber);
            if (catchResult is null)
            {
                message = "خطای رخ داده است به پشتیبانی اطلاع دهید ";
            }
            else
            {
                if (status != "1")
                {
                    message = "پرداخت انجام نشد";
                }
                else
                {
                    response = await _sepalService.Verify(new SepalVerifyModel
                    {
                        ApiKey = ServiceKeys.SepalKey,
                        InvoiceNumber = invoiceNumber,
                        PaymentNumber = paymentNumber
                    });
                    result = true;
                }
            }

            if (catchResult.GatewayTransactionType == GatewayTransactionType.Buy)
            {
                returnUrl += $"SepalVerify?input={_aesServices.Encrypt(_sepalService.GenerateVerifyResult(result, message, response, invoiceNumber,paymentNumber), ServiceKeys.GatewayEncriptionKey)}";
            }
            else
            {
                returnUrl += $"SepalDepositVerify?input={_aesServices.Encrypt(_sepalService.GenerateVerifyResult(result, message, response, invoiceNumber, paymentNumber), ServiceKeys.GatewayEncriptionKey)}";
            }

            return Redirect(returnUrl);
        }
    }
}
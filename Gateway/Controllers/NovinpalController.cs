using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AS.Utility.Helpers;
using AS.BL.Catches;

namespace Gateway.Controllers
{
    public class NovinpalController : Controller
    {
        private readonly ILogger _logger;
        private readonly INovinpalService _novinpalService;
        private readonly IAESServices _aesServices;
        private readonly INovinpalCatch _novinpalCatch;

        public NovinpalController(ILogger logger,
            INovinpalService novinpalService,
            IAESServices aesServices,
            INovinpalCatch novinpalCatch)
        {
            _logger = logger;
            _novinpalService = novinpalService;
            _aesServices = aesServices;
            _novinpalCatch = novinpalCatch;
        }

        public async Task<ActionResult> Payment(string input)
        {
            var parameters = _aesServices.Decrypt(input, ServiceKeys.GatewayEncriptionKey).Split('#');
            var amount = parameters[0].ToInt64();
            var orderId = parameters[1];
            var gatewayTransactionType = (GatewayTransactionType)Convert.ToInt32(parameters[2]);
            var callback = $"{HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/Novinpal/Verify";

            var createResponse = await _novinpalService.Payment(new AS.Model.Novinpal.NovinpalRequestModel
            {
                Amount = amount,
                OrderId = orderId,
                ReturnUrl = callback,
                ApiKey = ServiceKeys.NovinpalKey
            });

            if (createResponse is null)
            {
                ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                return View();
            }

            if (createResponse.Status == 1)
            {
                var catchResult = _novinpalCatch.Add(new AS.Model.Novinpal.NovinpalCatchModel
                {
                    Amount = amount,
                    GatewayTransactionType = gatewayTransactionType,
                    OrderId = orderId,
                    RefNum = createResponse.RefId
                });

                if (catchResult is null)
                {
                    ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
                    return View();
                }

                return Redirect($"https://gw.novinpal.ir/invoice/start/{createResponse.RefId}");
            }

            ViewBag.Message = "خطای رخ داده است به پشتیبانی اطلاع دهید";
            return View();
        }

        public async Task<ActionResult> Verify(string refId, int success, int code, string invoiceNumber, long amount)
        {
            try
            {
                var result = false;
                var message = "";
                var response = "";
                var returnUrl = $"{ServiceKeys.PanelAvvalMoneyUrl}Geteway/";

                var catchResult = _novinpalCatch.Get(invoiceNumber);
                if (catchResult is null)
                {
                    message = "خطای رخ داده است به پشتیبانی اطلاع دهید ";
                }
                else
                {
                    response = await _novinpalService.Verify(ServiceKeys.NovinpalKey, catchResult.RefNum);
                    result = true;
                }

                if (catchResult.GatewayTransactionType == GatewayTransactionType.Buy)
                {
                    returnUrl += $"NovinPalVerify?input={_aesServices.Encrypt(_novinpalService.GenerateVerifyResult(result, message, response, invoiceNumber), ServiceKeys.GatewayEncriptionKey)}";
                }
                else
                {
                    returnUrl += $"NovinPalWalletVerify?input={_aesServices.Encrypt(_novinpalService.GenerateVerifyResult(result, message, response, invoiceNumber), ServiceKeys.GatewayEncriptionKey)}";
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
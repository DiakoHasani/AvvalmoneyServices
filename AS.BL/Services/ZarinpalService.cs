using AS.Log;
using AS.Model.ZarinPal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class ZarinpalService : BaseApi, IZarinpalService
    {
        private readonly ILogger _logger;
        public ZarinpalService(ILogger logger)
        {
            _logger = logger;
        }

        public string GenerateVerifyResult(bool result, string message, string response, string orderId)
        {
            return $"{result}#{message ?? ""}#{response ?? ""}#{orderId}";
        }

        public async Task<ZarinPalPaymentResponseModel> Payment(ZarinPalPaymentRequestModel model)
        {
            var parameter = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var response = await Post($"{ZarinPalUrl}payment/request.json", parameter);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ZarinPalPaymentResponseModel>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> Verify(ZarinPalVerifyRequestModel model)
        {
            var parameter = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var response = await Post($"{ZarinPalUrl}payment/verify.json", parameter);
            return await response.Content.ReadAsStringAsync();
        }
    }

    public interface IZarinpalService
    {
        Task<ZarinPalPaymentResponseModel> Payment(ZarinPalPaymentRequestModel model);
        Task<string> Verify(ZarinPalVerifyRequestModel model);
        string GenerateVerifyResult(bool result, string message, string response, string orderId);
    }
}

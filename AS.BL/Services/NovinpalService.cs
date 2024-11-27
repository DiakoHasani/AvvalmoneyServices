using AS.Log;
using AS.Model.Novinpal;
using AS.Model.Paystar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class NovinpalService : BaseApi, INovinpalService
    {
        private readonly ILogger _logger;
        public NovinpalService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<NovinpalResponseModel> Payment(NovinpalRequestModel model)
        {
            try
            {
                var param = new Dictionary<string, string>
                {
                    { "api_key", model.ApiKey },
                    { "amount", model.Amount.ToString() },
                    { "return_url", model.ReturnUrl },
                    { "order_id", model.OrderId }
                };

                var response = await Post($"{NovinpalUrl}/invoice/request", new FormUrlEncodedContent(param));

                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<NovinpalResponseModel>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<string> Verify(string keyNovinPal, string refId)
        {
            var param = new Dictionary<string, string>();
            param.Add("api_key", keyNovinPal);
            param.Add("ref_id", refId);

            var response =await Post($"{NovinpalUrl}/invoice/verify", new FormUrlEncodedContent(param));
            return await response.Content.ReadAsStringAsync();
        }

        public string GenerateVerifyResult(bool result, string message, string response, string orderId)
        {
            return $"{result}#{message ?? ""}#{response ?? ""}#{orderId}";
        }
    }
    public interface INovinpalService
    {
        Task<NovinpalResponseModel> Payment(NovinpalRequestModel model);
        Task<string> Verify(string keyNovinPal, string refId);
        string GenerateVerifyResult(bool result, string message, string response, string orderId);
    }
}

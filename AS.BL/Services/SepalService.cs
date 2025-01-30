using AS.Log;
using AS.Model.Paystar;
using AS.Model.Sepal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class SepalService : BaseApi, ISepalService
    {
        private readonly ILogger _logger;
        public SepalService(ILogger logger)
        {
            _logger = logger;
        }

        public string GenerateVerifyResult(bool result, string message, string response, string orderId,string paymentNumber)
        {
            return $"{result}#{message ?? ""}#{response ?? ""}#{orderId}#{paymentNumber}";
        }

        public async Task<ResponseSepalModel> Request(SepalRequestModel model)
        {
            try
            {
                var parameter = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{SepalUrl}/api/request.json", parameter);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseSepalModel>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<string> Verify(SepalVerifyModel model)
        {
            try
            {
                var parameter = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{SepalUrl}/api/verify.json", parameter);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface ISepalService
    {
        Task<ResponseSepalModel> Request(SepalRequestModel model);
        Task<string> Verify(SepalVerifyModel model);
        string GenerateVerifyResult(bool result, string message, string response, string orderId, string paymentNumber);
    }
}

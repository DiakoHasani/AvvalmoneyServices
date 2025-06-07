using AS.Log;
using AS.Model.General;
using AS.Model.Paystar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class PaystarService : BaseApi, IPaystarService
    {
        private readonly ILogger _logger;
        public PaystarService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<CreateResponsePaystarModel> Create(CreateRequestPaystarModel model)
        {
            try
            {
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{PaystarUrl}/create", parameters, ServiceKeys.PaystarKey);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<CreateResponsePaystarModel>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public string GenerateCreateSign(long amount, string orderId, string callback)
        {
            var signData = amount + "#" + orderId + "#" + callback;
            var keyBytes = Encoding.UTF8.GetBytes(ServiceKeys.PaystarSignKey);
            var dataBytes = Encoding.UTF8.GetBytes(signData);

            return GenerateSign(keyBytes, dataBytes);
        }

        public string GenerateVerifySign(long amount, string refNum, string cardNumber, string trackingCode)
        {
            var signData = amount + "#" + refNum + "#" + cardNumber + "#" + trackingCode;

            var keyBytes = Encoding.UTF8.GetBytes(ServiceKeys.PaystarSignKey);
            var dataBytes = Encoding.UTF8.GetBytes(signData);

            return GenerateSign(keyBytes, dataBytes);
        }

        private string GenerateSign(byte[] keyBytes, byte[] dataBytes)
        {
            using (HMACSHA512 hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<string> Verify(VerifyRequestPaystarModel model)
        {
            try
            {
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{PaystarUrl}/verify", parameters, ServiceKeys.PaystarKey);

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public string GenerateVerifyResult(bool result, string message, string response, string orderId)
        {
            return $"{result}#{message ?? ""}#{response ?? ""}#{orderId}";
        }

        public async Task<ResponseRefreshApiKeyPaystarModel> RefreshApiKey(RequestRefreshApiKeyPaystarModel model)
        {
            try
            {
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{PaystarUrl2}application/refresh-api-key", parameters);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseRefreshApiKeyPaystarModel>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<ResponsePaystarSettlementModel> Settlement(RequestPaystarSettlementModel model, string apiKey)
        {
            try
            {
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{PaystarUrl2}bank-transfer/v2/settlement", parameters,apiKey);
                var text = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponsePaystarSettlementModel>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }

    public interface IPaystarService
    {
        Task<CreateResponsePaystarModel> Create(CreateRequestPaystarModel model);
        string GenerateCreateSign(long amount, string orderId, string callback);
        string GenerateVerifySign(long amount, string refNum, string cardNumber, string trackingCode);
        Task<string> Verify(VerifyRequestPaystarModel model);
        string GenerateVerifyResult(bool result, string message, string response, string orderId);
        Task<ResponseRefreshApiKeyPaystarModel> RefreshApiKey(RequestRefreshApiKeyPaystarModel model);
        Task<ResponsePaystarSettlementModel> Settlement(RequestPaystarSettlementModel model,string apiKey);
    }
}

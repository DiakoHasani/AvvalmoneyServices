using AS.Log;
using AS.Model.WithdrawCryptoBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class WithdrawCryptoApiService : BaseApi, IWithdrawCryptoApiService
    {
        private readonly ILogger _logger;
        public WithdrawCryptoApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> GetAvailable(string fhlowk, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/CryptoWithdraw/GetAvailable/{fhlowk}", token);
                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
                    {
                        return false;
                    }
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                }
                _logger.Error("error in call GetAvailable", new { message = await response.Content.ReadAsStringAsync() });
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }

        public async Task<ResponseLoginModel> Login(RequestLoginModel model)
        {
            try
            {
                var parameters = new Dictionary<string, string> {{ "UserName", model.UserName },
                    { "Password", model.Password },{ "fhlowk", model.Fhlowk }};

                var response = await Post(WithdrawApiUrl + "api/Account/Login", parameters);

                if (response.IsSuccessStatusCode)
                {
                    return new ResponseLoginModel
                    {
                        Code = 200,
                        Result = true,
                        Message = "Succefull call login api",
                        Token = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync())
                    };
                }

                _logger.Error("error in login WithdrawCryptoApi api", new { text = await response.Content.ReadAsStringAsync() });
                return new ResponseLoginModel
                {
                    Code = (int)response.StatusCode,
                    Message = $"error in login WithdrawCryptoApi api. error message:{await response.Content.ReadAsStringAsync()}"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new ResponseLoginModel
                {
                    Code = 500,
                    Message = "Error in main try catch Login. Exception Message: " + ex.Message
                };
            }
        }
    }
    public interface IWithdrawCryptoApiService
    {
        Task<ResponseLoginModel> Login(RequestLoginModel model);
        Task<bool> GetAvailable(string fhlowk, string token);
    }
}

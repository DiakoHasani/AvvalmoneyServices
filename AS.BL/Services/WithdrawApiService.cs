using AS.Log;
using AS.Model.PaymentWithdrawBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class WithdrawApiService : BaseApi, IWithdrawApiService
    {
        private readonly ILogger _logger;
        public WithdrawApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ResponseBotAvailableModel> GetBotAvailable(string fhlowk, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/Withraw/GetBotAvailable/{fhlowk}", token);

                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
                    {
                        return null;
                    }
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseBotAvailableModel>(await response.Content.ReadAsStringAsync());
                }
                _logger.Error("error in call GetBotAvailable", new { message = await response.Content.ReadAsStringAsync() });
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
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

                _logger.Error("error in login withdraw api", new { text = await response.Content.ReadAsStringAsync() });
                return new ResponseLoginModel
                {
                    Code = (int)response.StatusCode,
                    Message = $"error in login withdraw api. error message:{await response.Content.ReadAsStringAsync()}"
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
    public interface IWithdrawApiService
    {
        Task<ResponseLoginModel> Login(RequestLoginModel model);
        Task<ResponseBotAvailableModel> GetBotAvailable(string fhlowk, string token);
    }
}

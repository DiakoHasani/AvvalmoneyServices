using AS.Log;
using AS.Model.General;
using AS.Model.TronGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class WebhookApiService : BaseApi, IWebhookApiService
    {
        private readonly ILogger _logger;
        public WebhookApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<MessageModel> Tron(string fhlowk, string transactionIdCode, double value, int wal_Id, int rw_Id, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/Webhook/Tron/{fhlowk}/{transactionIdCode}/{value}/{wal_Id}/{rw_Id}",token);
                if (!response.IsSuccessStatusCode)
                {
                    var content=await response.Content.ReadAsStringAsync();
                    return new MessageModel
                    {
                        Message = "response.IsSuccessStatusCode is false"
                    };
                }

                return new MessageModel
                {
                    Message = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()),
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new MessageModel { Message = ex.Message };
            }
        }

        public async Task<MessageModel> Usdt(string fhlowk, string transactionIdCode, double value, int wal_Id, int rw_Id, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/Webhook/USDT/{fhlowk}/{transactionIdCode}/{value}/{wal_Id}/{rw_Id}",token);
                if (!response.IsSuccessStatusCode)
                {
                    return new MessageModel
                    {
                        Message = "response.IsSuccessStatusCode is false"
                    };
                }

                return new MessageModel
                {
                    Message = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()),
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new MessageModel { Message = ex.Message };
            }
        }
    }
    public interface IWebhookApiService
    {
        Task<MessageModel> Usdt(string fhlowk, string transactionIdCode, double value, int wal_Id, int rw_Id, string token);
        Task<MessageModel> Tron(string fhlowk, string transactionIdCode, double value, int wal_Id, int rw_Id, string token);
    }
}

using AS.Log;
using AS.Model.TransactionId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TransactionIdApiService : BaseApi, ITransactionIdApiService
    {
        private readonly ILogger _logger;
        public TransactionIdApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TransactionIdModel> Add(TransactionIdModel model, string token)
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                    { "TransactionIdCode", model.TransactionIdCode },
                    { "Wal_Id", model.Wal_Id.ToString() },
                };

                var response = await Post($"{WithdrawApiUrl}api/TransactionId/Add", parameters, token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TransactionIdModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<bool> CheckExistTransactionIdCode(string Txid, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/TransactionId/CheckExistTransactionIdCode/{Txid}", token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
    public interface ITransactionIdApiService
    {
        Task<bool> CheckExistTransactionIdCode(string Txid, string token);
        Task<TransactionIdModel> Add(TransactionIdModel model, string token);
    }
}

using BankCheckBot.Interfaces;
using BankCheckBot.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Services
{
    internal class SamanBankApiService(ILogger<SamanBankApiService> logger) : BaseApi, ISamanBankApiService
    {
        public async Task<List<ResponseBillStatementModel>> BillStatement(List<BillStatementTransactionModel> model, string token)
        {
            try
            {
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{WithdrawApiUrl}api/SamanBank/BillStatement", parameters, token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseBillStatementModel>>(await response.Content.ReadAsStringAsync());
                }
                logger.LogError(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}

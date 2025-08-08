using AS.Log;
using AS.Model.SamanBank;
using AS.Model.UserWalletReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class SamanBankApiService : BaseApi, ISamanBankApiService
    {
        private readonly ILogger _logger;
        public SamanBankApiService(ILogger logger)
        {
            _logger = logger;
        }

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
                _logger.Error(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface ISamanBankApiService
    {
        Task<List<ResponseBillStatementModel>> BillStatement(List<BillStatementTransactionModel> model, string token);
    }
}

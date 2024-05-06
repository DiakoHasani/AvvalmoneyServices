using AS.Log;
using AS.Model.TetherBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TetherBankService : BaseApi, ITetherBankService
    {
        private readonly ILogger _logger;
        public TetherBankService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ResponseTetherBankModel> Get()
        {
            try
            {
                var response = await Get(TetherBankUrl + "api/v2/top-currencies");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseTetherBankModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface ITetherBankService
    {
        Task<ResponseTetherBankModel> Get();
    }
}

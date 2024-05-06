using AS.Log;
using AS.Model.Ramzinex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class RamzinexService : BaseApi, IRamzinexService
    {
        private readonly ILogger _logger;
        ResponseRamzinexModel responseRamzinex;
        public RamzinexService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ResponseRamzinexModel> Get()
        {
            try
            {
                var response = await Get(RamzinexUrl + "exchange/api/v1.0/exchange/pairs");
                if (response.IsSuccessStatusCode)
                {
                    responseRamzinex = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseRamzinexModel>(await response.Content.ReadAsStringAsync());
                    responseRamzinex.Data = responseRamzinex.Data.Where(o => o.BaseCurrencySymbol.EN == "usdt" || o.BaseCurrencySymbol.EN == "trx").ToList();
                    return responseRamzinex;
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
    public interface IRamzinexService
    {
        Task<ResponseRamzinexModel> Get();
    }
}

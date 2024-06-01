using AS.Log;
using AS.Model.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class CurrencyApiService : BaseApi, ICurrencyApiService
    {
        private readonly ILogger _logger;
        public CurrencyApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<int> GetCur_IdByISOCode(string isoCode, string bearerToken = "")
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/Currency/GetCur_IdByISOCode/{isoCode}", bearerToken);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
                }
                _logger.Error(await response.Content.ReadAsStringAsync());
                return 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return 0;
            }
        }
    }
    public interface ICurrencyApiService
    {
        Task<int> GetCur_IdByISOCode(string isoCode, string bearerToken = "");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.Log;
using AS.Model.CurrencyPriceHistory;

namespace AS.BL.Services
{
    public class CurrencyPriceHistoryApiService : BaseApi, ICurrencyPriceHistoryApiService
    {
        private readonly ILogger _logger;
        public CurrencyPriceHistoryApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<CurrencyPriceHistoryModel> Add(CurrencyPriceHistoryModel model, string bearerToken = "")
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                    { "AdmUsr_Id", model.AdmUsr_Id.ToString() },
                    { "CPH_BuyPrice", model.CPH_BuyPrice.ToString() },
                    { "CPH_SellPrice", model.CPH_SellPrice.ToString() },
                    { "CPH_CreateDate", model.CPH_CreateDate.ToString() },
                    { "Cur_Id", model.Cur_Id.ToString() },
                };

                var response = await Post($"{WithdrawApiUrl}api/CurrencyPriceHistory/Add", parameters, bearerToken);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<CurrencyPriceHistoryModel>(await response.Content.ReadAsStringAsync());
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

        public async Task<CurrencyPriceHistoryModel> GetByCur_Id(long cur_id, string bearerToken = "")
        {
            try
            {
                var response =await Get($"{WithdrawApiUrl}api/CurrencyPriceHistory/GetByCur_Id/{cur_id}",bearerToken);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<CurrencyPriceHistoryModel>(await response.Content.ReadAsStringAsync());
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
    public interface ICurrencyPriceHistoryApiService
    {
        Task<CurrencyPriceHistoryModel> Add(CurrencyPriceHistoryModel model, string bearerToken = "");
        Task<CurrencyPriceHistoryModel> GetByCur_Id(long cur_id, string bearerToken);
    }
}

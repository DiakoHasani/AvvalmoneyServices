using AS.Log;
using AS.Model.Nobitex;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class NobitexService : BaseApi, INobitexService
    {
        private readonly ILogger _logger;
        private ResponseNobitexTetherModel responseTether;
        private ResponseNobitexTronModel responseTron;
        public NobitexService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<ResponseNobitexTetherModel> GetTether()
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                    { "srcCurrency", "usdt" },
                    { "dstCurrency", "rls" }
                };

                var response = await Post(NobitexUrl + "market/stats", parameters);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseNobitexTetherModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception in NobitexService.GetTether", ex);
                return null;
            }
        }

        public async Task<double> GetTetherAmount()
        {

            responseTether = await GetTether();

            if (responseTether is null)
            {
                _logger.Error("responseTether is null");
                return 0;
            }

            if (responseTether.Status != "ok")
            {
                _logger.Error($"responseTether.Status is {responseTether.Status}", responseTether);
                return 0;
            }

            if (responseTether.Stats.USDT.IsClosed)
            {
                _logger.Error("nobitex Tether is Closed", responseTether);
            }

            return responseTether.Stats.USDT.Latest.ToDouble().RialToToman();
        }

        public async Task<ResponseNobitexTronModel> GetTron()
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                    { "srcCurrency", "trx" },
                    { "dstCurrency", "rls" }
                };
                var response = await Post(NobitexUrl + "market/stats", parameters);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseNobitexTronModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception in NobitexService.GetTron", ex);
                return null;
            }
        }

        public async Task<double> GetTronAmount()
        {
            responseTron = await GetTron();

            if (responseTron is null)
            {
                _logger.Error("responseTron is null");
                return 0;
            }

            if (responseTron.Status != "ok")
            {
                _logger.Error($"responseTron.Status is {responseTron.Status}", responseTron);
                return 0;
            }

            if (responseTron.Stats.Tron.IsClosed)
            {
                _logger.Error("nobitex Tron is Closed", responseTron);
            }

            return responseTron.Stats.Tron.Latest.ToDouble().RialToToman();
        }
    }
    public interface INobitexService
    {
        Task<ResponseNobitexTetherModel> GetTether();
        Task<double> GetTetherAmount();
        Task<ResponseNobitexTronModel> GetTron();
        Task<double> GetTronAmount();
    }
}

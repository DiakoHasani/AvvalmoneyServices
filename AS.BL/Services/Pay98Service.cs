using AS.Log;
using AS.Model.Enums;
using AS.Model.Pay98;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class Pay98Service : BaseApi, IPay98Service
    {
        private readonly ILogger _logger;
        private ResponsePay98Model responseTether,responseTron;
        public Pay98Service(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ResponsePay98Model> GetBuyTether()
        {
            try
            {
                var response = await Get($"{Pay98Url}api/calculator?type=crypto&wallet=USDT&amount=1&side=buy&isBase=1");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponsePay98Model>(await response.Content.ReadAsStringAsync());
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

        public async Task<ResponsePay98Model> GetBuyTron()
        {
            try
            {
                var response = await Get($"{Pay98Url}api/calculator?type=crypto&wallet=TRX&amount=1&side=buy&isBase=1");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponsePay98Model>(await response.Content.ReadAsStringAsync());
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

        public async Task<ResponsePay98Model> GetSellTether()
        {
            try
            {
                var response = await Get($"{Pay98Url}api/calculator?type=crypto&wallet=USDT&amount=1&side=sell&isBase=1");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponsePay98Model>(await response.Content.ReadAsStringAsync());
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

        public async Task<ResponsePay98Model> GetSellTron()
        {
            try
            {
                var response = await Get($"{Pay98Url}api/calculator?type=crypto&wallet=TRX&amount=1&side=sell&isBase=1");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponsePay98Model>(await response.Content.ReadAsStringAsync());
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

        public async Task<double> GetTetherAmount(DealType dealType)
        {
            if (dealType == DealType.Buy)
            {
                responseTether = await GetBuyTether();
            }
            else
            {
                responseTether = await GetSellTether();
            }
            
            if(responseTether is null)
            {
                return 0;
            }

            return responseTether.Data.NoFee.Quote.ToDouble();
        }

        public async Task<double> GetTronAmount(DealType dealType)
        {
            if (dealType == DealType.Buy)
            {
                responseTron = await GetBuyTron();
            }
            else
            {
                responseTron = await GetSellTron();
            }

            if (responseTron is null)
            {
                return 0;
            }

            return responseTron.Data.NoFee.Quote.ToDouble();
        }
    }
    public interface IPay98Service
    {
        Task<ResponsePay98Model> GetBuyTether();
        Task<ResponsePay98Model> GetSellTether();
        Task<double> GetTetherAmount(DealType dealType);
        Task<ResponsePay98Model> GetBuyTron();
        Task<ResponsePay98Model> GetSellTron();
        Task<double> GetTronAmount(DealType dealType);
    }
}

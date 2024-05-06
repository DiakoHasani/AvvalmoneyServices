using AS.Log;
using AS.Model.Tetherland;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TetherlandService : BaseApi, ITetherlandService
    {
        private readonly ILogger _logger;
        ResponseTetherlandModel responseTetherland;

        public TetherlandService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<ResponseTetherlandModel> Get()
        {
            try
            {
                var response = await Get($"{TetherlandUrl}api/v5/currencies");
                if (response.IsSuccessStatusCode)
                {
                    responseTetherland = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseTetherlandModel>(await response.Content.ReadAsStringAsync());
                    responseTetherland.Data = responseTetherland.Data.Where(o => o.Symbol == "USDT" || o.Symbol == "TRX").ToList();
                    return responseTetherland;
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
    public interface ITetherlandService
    {
        Task<ResponseTetherlandModel> Get();
    }
}

using AS.Log;
using AS.Model.General;
using AS.Model.SmartPeck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class SmartPekService : BaseApi, ISmartPekService
    {
        private readonly ILogger _logger;
        public SmartPekService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> TurnOffChannel2()
        {
            try
            {
                var model = new RequestSmartPeckHookModel
                {
                    Token = ServiceKeys.SmartPekKey,
                    Command = "channel 2 off",
                    Origin = "vps1"
                };
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{SmartPeckUrl}hook", parameters);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> TurnOnChannel2()
        {
            try
            {
                var model = new RequestSmartPeckHookModel
                {
                    Token = ServiceKeys.SmartPekKey,
                    Command = "channel 2 on",
                    Origin = "vps1"
                };
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await Post($"{SmartPeckUrl}hook", parameters);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
    public interface ISmartPekService
    {
        Task<bool> TurnOnChannel2();
        Task<bool> TurnOffChannel2();
    }
}

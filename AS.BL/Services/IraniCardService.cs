using AS.Log;
using AS.Model.IraniCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class IraniCardService : BaseApi, IIraniCardService
    {
        private readonly ILogger _logger;
        public IraniCardService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ResponseIraniCardModel> Get()
        {
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("action", "getBit_action");
                param.Add("_wpnonce", await GetWPNonce());

                var response = await Post(IraniCardUrl + "wp-admin/admin-ajax.php", new FormUrlEncodedContent(param));
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseIraniCardModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error("response.IsSuccessStatusCode is false",await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<string> GetHomeHtml()
        {
            return await (await Get(IraniCardUrl)).Content.ReadAsStringAsync();
        }

        public async Task<string> GetWPNonce()
        {
            var html = await GetHomeHtml();
            html = html.Split(new[] { "id=\"main_cal_cryto\"" }, StringSplitOptions.None)[1];
            html = html.Split(new[] { "class=\"cal_bit__box\"" }, StringSplitOptions.None)[1];
            html = html.Split(new[] { "data-nonce=\"" }, StringSplitOptions.None)[1];
            html = html.Split(new[] { "\">" }, StringSplitOptions.None)[0];
            return html.Replace("\"", "");
        }
    }

    public interface IIraniCardService
    {
        Task<ResponseIraniCardModel> Get();
        Task<string> GetWPNonce();
        Task<string> GetHomeHtml();
    }
}

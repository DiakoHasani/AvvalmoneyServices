using AS.Log;
using AS.Model.Ex4Ir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class Ex4IrService : BaseApi, IEx4IrService
    {
        private readonly ILogger _logger;
        public Ex4IrService(ILogger logger)
        {
            _logger = logger;
        }

        private async Task<Ex4TokenModel> GetCookie(string url)
        {
            try
            {
                var cookieContainer = new CookieContainer();
                using (var httpClientHandler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer
                })
                {
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {
                        var html = await (await httpClient.GetAsync(new Uri(url))).Content.ReadAsStringAsync();
                        return new Ex4TokenModel
                        {
                            Cookies = cookieContainer.GetCookies(new Uri(url)).Cast<Cookie>(),
                            CsrfToken = GetCsrfToken(html)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public string GetCsrfToken(string html)
        {
            try
            {
                return html.Split(new[] { "name=\"csrf-token\"" }, StringSplitOptions.None)[1].Split('>')[0].Split('\"')[1];
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return "";
            }
        }

        public async Task<List<ResponseEx4IrModel>> Get()
        {
            var ex4token = await GetCookie(Ex4IrUrl);
            var message = new HttpRequestMessage(HttpMethod.Post, "/assets/get");
            message.Headers.Add("x-csrf-token", ex4token.CsrfToken);
            message.Headers.Add("Cookie", $"XSRF-TOKEN={ex4token.Cookies.Where(a => a.Name == "XSRF-TOKEN").FirstOrDefault().Value};" +
                " _ga=GA1.2.1026739839.1648366814; _gat_gtag_UA_110545111_1=1; _gid=GA1.2.1636441919.1656325824;" +
                $"sesid={ex4token.Cookies.Where(a => a.Name == "sesid").FirstOrDefault().Value}");

            var response = await Post(Ex4IrUrl, message);
            if (response.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseEx4IrModel>>(await response.Content.ReadAsStringAsync())
                    .Where(o => o.Symbol == "USDT" || o.Symbol == "TRX").ToList();
            }

            _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
            return null;
        }
    }
    public interface IEx4IrService
    {
        Task<List<ResponseEx4IrModel>> Get();
    }
}

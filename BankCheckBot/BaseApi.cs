using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot
{
    internal abstract class BaseApi
    {
        protected const string WithdrawApiUrl = "https://avvalex.panel.avvalmoney.co/";
        protected async Task<HttpResponseMessage> Post(string url, Dictionary<string, string> parameters)
        {
            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await client.PostAsync(url, data);
            }
        }
        protected async Task<HttpResponseMessage> Post(string url, Dictionary<string, string> parameters, string bearerToken)
        {
            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
                return await client.PostAsync(url, data);
            }
        }

        protected async Task<HttpResponseMessage> Post(string url, HttpRequestMessage message)
        {
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await client.SendAsync(message);
            }
        }

        protected async Task<HttpResponseMessage> Post(string url, FormUrlEncodedContent content)
        {
            using (var client = new HttpClient())
            {
                return await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, url) { Content = content });
            }
        }

        protected async Task<HttpResponseMessage> Post(string url, string parameters, string bearerToken)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(parameters, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                return await client.PostAsync(url, data);
            }
        }

        protected async Task<HttpResponseMessage> Post(string url, string parameters)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(parameters, Encoding.UTF8, "application/json");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                return await client.PostAsync(url, data);
            }
        }

        protected async Task<HttpResponseMessage> Get(string url)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await client.GetAsync(url);
            }
        }

        protected async Task<HttpResponseMessage> Get(string url, string bearerToken)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
                return await client.GetAsync(url);
            }
        }
    }
}

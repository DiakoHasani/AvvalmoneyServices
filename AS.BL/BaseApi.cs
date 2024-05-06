﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL
{
    public abstract class BaseApi
    {
        protected const string NobitexUrl = "https://api.nobitex.ir/";
        protected const string HdPayUrl = "https://hdpay.ir/";
        protected const string Ex4IrUrl = "https://ex4ir.net";
        protected const string IraniCardUrl = "https://www.iranicard.ir/";
        protected const string RamzinexUrl = "https://publicapi.ramzinex.com/";
        protected const string TetherBankUrl = "https://api.tether-bank.com/";
        protected const string Pay98Url = "https://pay98.app/";
        protected const string TetherlandUrl = "https://service.tetherland.com/";
        protected const string TronScanUrl = "https://apilist.tronscan.org/";
        protected const string TronScanUrl2 = "https://apilist.tronscanapi.com/";
        protected const string WithdrawApiUrl = "http://avvalex.ir/";

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

        protected async Task<HttpResponseMessage> Get(string url)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                return await client.GetAsync(url);
            }
        }

        protected async Task<HttpResponseMessage> Get(string url,string bearerToken)
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

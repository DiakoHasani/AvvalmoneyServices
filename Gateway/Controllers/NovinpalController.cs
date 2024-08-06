using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Gateway.Controllers
{
    public class NovinpalController : Controller
    {
        public async Task<ActionResult> Pay()
        {
            using (var client = new HttpClient())
            {
                var param = new Dictionary<string, string>();
                param.Add("api_key", "eff7c2c9-3dd4-47c0-aae4-d4f8d6df3cae");
                param.Add("amount", "10000");
                param.Add("return_url", $"{HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/Novinpal/Verify");
                param.Add("order_id", Guid.NewGuid().ToString());

                var response = await client.SendAsync(
                    new HttpRequestMessage(HttpMethod.Post, "https://gw.novinpal.ir" + "/invoice/request")
                    { Content = new FormUrlEncodedContent(param) });

                ViewBag.Message=await response.Content.ReadAsStringAsync();
                return View();
            }
        }

        public ActionResult Verify()
        {
            return View();
        }
    }
}
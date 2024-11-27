using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Novinpal
{
    public class NovinpalRequestModel
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }
    }
}

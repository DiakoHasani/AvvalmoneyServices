using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class CreateRequestPaystarModel
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("callback")]
        public string Callback { get; set; }

        [JsonProperty("sign")]
        public string Sign { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Novinpal
{
    public class NovinpalVerifyResponseModel
    {
        [JsonProperty("paidAt")]
        public string PaidAt { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("refNumber")]
        public string RefNumber { get; set; }

        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("verifiedBefore")]
        public bool VerifiedBefore { get; set; }
    }
}

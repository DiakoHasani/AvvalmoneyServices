using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Sepal
{
    public class ResponseSepalModel
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("paymentNumber")]
        public string PaymentNumber { get; set; }
        
        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }
    }
}

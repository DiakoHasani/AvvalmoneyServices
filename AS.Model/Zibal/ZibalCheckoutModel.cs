using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Zibal
{
    public class ZibalCheckoutModel
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("bankAccount")]
        public string BankAccount { get; set; }

        [JsonProperty("checkoutDelay")]
        public int CheckoutDelay { get; set; }

        [JsonProperty("result")]
        public int Result { get; set; } = 0;

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = "";
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Sepal
{
    public class SepalVerifyModel
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("paymentNumber")]
        public string PaymentNumber { get; set; }

        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber { get; set; }
    }
}

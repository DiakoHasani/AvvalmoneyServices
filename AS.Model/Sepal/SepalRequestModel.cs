using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Sepal
{
    public class SepalRequestModel
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("callbackUrl")]
        public string CallbackUrl { get; set; }

        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber { get; set; }
    }
}

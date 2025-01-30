using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.ZarinPal
{
    public class ZarinPalVerifyRequestModel
    {
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("authority")]
        public string Authority { get; set; }
    }
}

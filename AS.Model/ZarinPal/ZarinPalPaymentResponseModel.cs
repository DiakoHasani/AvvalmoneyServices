using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.ZarinPal
{
    public class ZarinPalPaymentResponseModel
    {
        [JsonProperty("data")]
        public ZarinPalPaymentDataResponseModel Data { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}

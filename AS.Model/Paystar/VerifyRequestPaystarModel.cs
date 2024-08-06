using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class VerifyRequestPaystarModel
    {
        [JsonProperty("ref_num")]
        public string RefNum { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("sign")]
        public string Sign { get; set; }
    }
}

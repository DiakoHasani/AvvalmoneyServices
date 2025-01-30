using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.ZarinPal
{
    public class ZarinPalVerifyResponseModel
    {
        [JsonProperty("data")]
        public ZarinPalVerifyDataResponseModel Data { get; set; }

        [JsonProperty("errors")]
        public string[] Errors { get; set; }
    }
}

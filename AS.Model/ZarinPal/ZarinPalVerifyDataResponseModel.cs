using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.ZarinPal
{
    public class ZarinPalVerifyDataResponseModel
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("card_pan")]
        public string CardPan { get; set; }

        [JsonProperty("ref_id")]
        public int RefId { get; set; }
    }
}

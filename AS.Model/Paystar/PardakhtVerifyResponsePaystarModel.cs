using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class PardakhtVerifyResponsePaystarModel
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("ref_num")]
        public string RefNum { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }
    }
}

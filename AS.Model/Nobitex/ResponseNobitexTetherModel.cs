using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class ResponseNobitexTetherModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("stats")]
        public ResponseNobitexStatTetherModel Stats { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class ResponseNobitexTronModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("stats")]
        public ResponseNobitexStatTronModel Stats { get; set; }
    }
}

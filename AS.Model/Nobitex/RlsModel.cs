using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class RlsModel
    {
        [JsonProperty("isClosed")]
        public bool IsClosed { get; set; }

        [JsonProperty("latest")]
        public string Latest { get; set; }
    }
}

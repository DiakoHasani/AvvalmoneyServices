using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Ramzinex
{
    public class RamzinexSymbolModel
    {
        [JsonProperty("en")]
        public string EN { get; set; }

        [JsonProperty("fa")]
        public string FA { get; set; }
    }
}

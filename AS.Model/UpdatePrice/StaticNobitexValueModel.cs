using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.UpdatePrice
{
    public class StaticNobitexValueModel
    {
        [JsonProperty("Buy")]
        public double Buy { get; set; }

        [JsonProperty("Sell")]
        public double Sell { get; set; }
    }
}

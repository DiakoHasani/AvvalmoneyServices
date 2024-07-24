using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.UpdatePrice
{
    public class StaticNobitexModel
    {
        [JsonProperty("Usdt")]
        public StaticNobitexValueModel Usdt { get; set; }

        [JsonProperty("Trx")]
        public StaticNobitexValueModel Trx { get; set; }

        [JsonProperty("Ton")]
        public StaticNobitexValueModel Ton { get; set; }

        [JsonProperty("Not")]
        public StaticNobitexValueModel Not { get; set; }

    }
}

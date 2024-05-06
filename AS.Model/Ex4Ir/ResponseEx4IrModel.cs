using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Ex4Ir
{
    public class ResponseEx4IrModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("buy_price")]
        public string BuyPrice { get; set; }

        [JsonProperty("sell_price")]
        public string SellPrice { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.IraniCard
{
    public class ResponseIraniCardSellPriceModel
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }
    }
}

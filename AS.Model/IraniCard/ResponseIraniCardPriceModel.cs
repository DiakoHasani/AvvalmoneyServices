using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.IraniCard
{
    public class ResponseIraniCardPriceModel
    {
        [JsonProperty("buy")]
        public ResponseIraniCardBuyPriceModel Buy { get; set; }

        [JsonProperty("sell")]
        public ResponseIraniCardBuyPriceModel Sell { get; set; }
    }
}

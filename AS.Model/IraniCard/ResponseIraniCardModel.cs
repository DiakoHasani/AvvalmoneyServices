using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.IraniCard
{
    public class ResponseIraniCardModel
    {
        [JsonProperty("USDT")]
        public ResponseIraniCardPriceModel USDT { get; set; }
        
        [JsonProperty("TRX")]
        public ResponseIraniCardPriceModel TRX { get; set; }
    }
}

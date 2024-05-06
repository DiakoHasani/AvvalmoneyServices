using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Ramzinex
{
    public class ResponseRamzinexDataModel
    {
        [JsonProperty("base_currency_symbol")]
        public RamzinexSymbolModel BaseCurrencySymbol { get; set; }

        [JsonProperty("buy")]
        public double Buy { get; set; }
        
        [JsonProperty("sell")]
        public double Sell { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrc20DataTokenInfoTronGridModel
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrxDataRawDataContractParameterValueTronGridModel
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("owner_address")]
        public string OwnerAddress { get; set; }

        [JsonProperty("to_address")]
        public string ToAddress { get; set; }
    }
}

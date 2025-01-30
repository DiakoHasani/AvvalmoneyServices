using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrxDataRawDataContractParameterTronGridModel
    {
        [JsonProperty("value")]
        public ResponseTrxDataRawDataContractParameterValueTronGridModel Value { get; set; }
    }
}

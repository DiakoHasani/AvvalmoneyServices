using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrxDataRawDataContractTronGridModel
    {
        [JsonProperty("parameter")]
        public ResponseTrxDataRawDataContractParameterTronGridModel Parameter { get; set; }
    }
}

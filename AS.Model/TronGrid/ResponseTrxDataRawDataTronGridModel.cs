using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrxDataRawDataTronGridModel
    {
        [JsonProperty("contract")]
        public List<ResponseTrxDataRawDataContractTronGridModel> MyProperty { get; set; }
    }
}

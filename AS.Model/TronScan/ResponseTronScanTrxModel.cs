using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronScan
{
    public class ResponseTronScanTrxModel
    {
        [JsonProperty("data")]
        public List<ResponseTronScanTrxDataModel> Data { get; set; }
    }
}

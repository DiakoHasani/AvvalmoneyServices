using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrxDataTronGridModel
    {
        [JsonProperty("txID")]
        public string TxID { get; set; }

        [JsonProperty("raw_data")]
        public ResponseTrxDataRawDataTronGridModel RawData { get; set; }
    }
}

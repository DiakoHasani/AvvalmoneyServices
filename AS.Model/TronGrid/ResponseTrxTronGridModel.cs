using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class ResponseTrxTronGridModel
    {
        [JsonProperty("data")]
        public List<ResponseTrxDataTronGridModel> Data { get; set; }
    }
}

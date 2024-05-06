using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Tetherland
{
    public class ResponseTetherlandModel
    {
        [JsonProperty("data")]
        public List<ResponseTetherlandDataModel> Data { get; set; }
    }
}

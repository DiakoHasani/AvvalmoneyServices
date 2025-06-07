using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class ResponseRefreshApiKeyPaystarModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public ResponseRefreshApiKeyDataPaystarModel Data { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class ResponseRefreshApiKeyDataPaystarModel
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("api_key_expire_date")]
        public string ApiKeyExpireDate { get; set; }
    }
}

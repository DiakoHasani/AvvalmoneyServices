using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.SmartPeck
{
    public class RequestSmartPeckHookModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }
    }
}

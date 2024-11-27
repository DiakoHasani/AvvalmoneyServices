using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Novinpal
{
    public class NovinpalResponseModel
    {
        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }
    }
}

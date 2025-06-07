using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class RequestPaystarSettlementModel
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("access_password")]
        public string AccessPassword { get; set; }

        [JsonProperty("transfers")]
        public List<RequestPaystarSettlementTransferModel> Transfers { get; set; }
    }
}

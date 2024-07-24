using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanEventActionsModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("TonTransfer")]
        public TonScanEventActionsTonTransferModel TonTransfer { get; set; }

        [JsonProperty("JettonTransfer")]
        public TonScanEventActionsJettonTransferModel JettonTransfer { get; set; }
    }
}

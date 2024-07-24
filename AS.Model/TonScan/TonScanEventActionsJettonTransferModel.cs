using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanEventActionsJettonTransferModel
    {
        [JsonProperty("sender")]
        public TonScanEventActionsTonTransferAddressModel Sender { get; set; }

        [JsonProperty("recipient")]
        public TonScanEventActionsTonTransferAddressModel Recipient { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("jetton")]
        public TonScanEventActionsJettonTransferJettonModel Jetton { get; set; }

        public string EventId { get; set; }
    }
}

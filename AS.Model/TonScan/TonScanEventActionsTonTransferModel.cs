using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanEventActionsTonTransferModel
    {
        [JsonProperty("sender")]
        public TonScanEventActionsTonTransferAddressModel Sender { get; set; }

        [JsonProperty("recipient")]
        public TonScanEventActionsTonTransferAddressModel Recipient { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        public string EventId { get; set; }
    }
}

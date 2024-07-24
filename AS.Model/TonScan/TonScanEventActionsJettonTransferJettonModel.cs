using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanEventActionsJettonTransferJettonModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

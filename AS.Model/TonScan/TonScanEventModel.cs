using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanEventModel
    {
        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("account")]
        public TonScanEventAccountModel Account { get; set; }

        [JsonProperty("actions")]
        public List<TonScanEventActionsModel> Actions { get; set; }
    }
}

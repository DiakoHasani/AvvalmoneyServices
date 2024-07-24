using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanModel
    {
        [JsonProperty("events")]
        public List<TonScanEventModel> Events { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TonScan
{
    public class TonScanEventActionsTonTransferAddressModel
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}

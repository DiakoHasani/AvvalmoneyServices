using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronScan
{
    public class RessponseTronScanTrc20Model
    {
        [JsonProperty("token_transfers")]
        public List<TronScanTrc20TokenTransferModel> TokenTransfers { get; set; }
    }
}

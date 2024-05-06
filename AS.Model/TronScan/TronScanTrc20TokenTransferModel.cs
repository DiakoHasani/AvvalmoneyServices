using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronScan
{
    public class TronScanTrc20TokenTransferModel
    {
        [JsonProperty("transaction_id")]
        public string TransactionIdCode { get; set; }

        [JsonProperty("from_address")]
        public string FromAddress { get; set; }

        [JsonProperty("to_address")]
        public string ToAddress { get; set; }

        [JsonProperty("quant")]
        public string Quant { get; set; }

        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("contractRet")]
        public string ContractRet { get; set; }

        [JsonProperty("finalResult")]
        public string FinalResult { get; set; }

        [JsonProperty("revert")]
        public bool Revert { get; set; }
    }
}

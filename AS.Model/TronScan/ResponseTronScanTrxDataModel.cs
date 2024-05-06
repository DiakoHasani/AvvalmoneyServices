using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronScan
{
    public class ResponseTronScanTrxDataModel
    {
        [JsonProperty("contractRet")]
        public string ContractRet { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("transferFromAddress")]
        public string TransferFromAddress { get; set; }

        [JsonProperty("transferToAddress")]
        public string TransferToAddress { get; set; }
    }
}

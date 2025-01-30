using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TronGrid
{
    public class SummaryResponseTrxTronGridModel
    {
        public string Txid { get; set; }
        public string OwnerAddress { get; set; }
        public string ToAddress { get; set; }
        public double Amount { get; set; }
    }
}

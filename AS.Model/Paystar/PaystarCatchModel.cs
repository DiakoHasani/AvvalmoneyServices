using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class PaystarCatchModel
    {
        public string OrderId { get; set; }
        public string RefNum { get; set; }
        public long Amount { get; set; }
        public GatewayTransactionType GatewayTransactionType { get; set; }
    }
}

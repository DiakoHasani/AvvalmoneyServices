using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.ZarinPal
{
    public class ZarinPalCatchModel
    {
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public GatewayTransactionType GatewayTransactionType { get; set; }
        public string Authority { get; set; }
    }
}

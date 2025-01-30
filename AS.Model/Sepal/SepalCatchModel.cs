using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Sepal
{
    public class SepalCatchModel
    {
        public long Amount { get; set; }
        public string OrderId { get; set; }
        public GatewayTransactionType GatewayTransactionType { get; set; }
        public string PaymentNumber { get; set; }
    }
}

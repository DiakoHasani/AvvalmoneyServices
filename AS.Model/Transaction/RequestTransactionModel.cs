using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Transaction
{
    public class RequestTransactionModel
    {
        public long UserId { get; set; }
        public double Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public long AdminUserId { get; set; }
    }
}

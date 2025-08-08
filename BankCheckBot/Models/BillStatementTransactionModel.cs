using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Models
{
    public class BillStatementTransactionModel
    {
        public int RowNumber { get; set; }
        public string DateTime { get; set; }
        public string Operation { get; set; }
        public string PayId { get; set; }
        public string StatementDescription { get; set; }
        public string DebitCredit { get; set; }
        public string Balance { get; set; }
        public string ChequeNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string AgentBranch { get; set; }
        public string DocumentNumber { get; set; }
    }
}

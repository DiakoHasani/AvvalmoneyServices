using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.SamanBank
{
    public class BillStatementTransactionModel
    {
        /// <summary>
        /// عملیات
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// شماره سند
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// مبلغ
        /// </summary>
        public string DebitCredit { get; set; }

    }
}

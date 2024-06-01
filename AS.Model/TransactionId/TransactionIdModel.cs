using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TransactionId
{
    public class TransactionIdModel
    {
        public long Tx_Id { get; set; }
        public string TransactionIdCode { get; set; }
        public int Wal_Id { get; set; }
    }
}

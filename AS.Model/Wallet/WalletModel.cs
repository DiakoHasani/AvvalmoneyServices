using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Wallet
{
    public class WalletModel
    {
        public int Wal_Id { get; set; }
        public string Address { get; set; }
        public bool Enabled { get; set; }
        public int CryptoType { get; set; }
        public int Aff_Id { get; set; }
        public DateTime LastTransaction { get; set; }
        public string MemoTag { get; set; }
        public bool Private { get; set; }
        public long? Usr_Id { get; set; }
    }
}

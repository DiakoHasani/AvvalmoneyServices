using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.ReservationWallet
{
    public class ReservationWalletModel
    {
        public int Rw_Id { get; set; }
        public int Wal_Id { get; set; }
        public DateTime RW_CreateDate { get; set; }
        public bool RW_Status { get; set; }
        public double RW_Amount { get; set; }
        public CryptoType CryptoType { get; set; }
        public int Aff_Id { get; set; }
        public string WalletAddress { get; set; }
    }
}

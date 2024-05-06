using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.UserWalletReservation
{
    public class UserWalletReservationModel
    {
        public int UWR_Id { get; set; }
        public DateTime UWR_CreateDate { get; set; }
        public int Wal_Id { get; set; }
        public CurrencyType UWR_CryptoType { get; set; }
        public string UWR_LastTxId { get; set; }
        public int UWR_TransactionCount { get; set; }
    }
}

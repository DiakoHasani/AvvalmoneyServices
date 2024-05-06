using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawCrypto
{
    public class ResponseWithdrawCryptoModel
    {
        public long WC_Id { get; set; }
        public string WC_Address { get; set; }
        public double WC_Amount { get; set; }
        public CurrencyType WC_CryptoType { get; set; }
    }
}

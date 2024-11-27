using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawCrypto
{
    public class ResponseWithdrawCryptoEncryptedModel
    {
        public string WC_Id { get; set; }
        public string WC_Address { get; set; }
        public string WC_Amount { get; set; }
        public string WC_CryptoType { get; set; }
        public string WC_CreateDate { get; set; }
        public string WC_Sign { get; set; }
    }
}

using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Contradiction
{
    public class ContradictionModel
    {
        public long WC_Id { get; set; }
        public string WC_Address { get; set; }
        public double WC_Amount { get; set; }
        public WithdrawCryptoStatus WC_Status { get; set; }
    }
}

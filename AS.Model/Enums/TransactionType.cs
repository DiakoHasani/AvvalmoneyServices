using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum TransactionType
    {
        Default = 0,
        Deposite = 1,
        Withdraw = 2,
        AdminDecrease = 3,
        AdminIncrease = 4,
        WithdrawCancelation = 5,
        Buy = 6,
        Sell = 7,
        PurchaseReturned = 8,
        RefralCommision = 9
    }
}

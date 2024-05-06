using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum UserBankCardStatus
    {
        [Description("در حال بررسی")]
        Pending = 0,

        [Description("تایید شده")]
        Accepted = 1,

        [Description("رد شده")]
        Rejected = 2
    }
}

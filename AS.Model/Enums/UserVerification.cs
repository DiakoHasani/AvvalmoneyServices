using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum UserVerification
    {
        [Description("Normal")]
        Normal = 0,
        [Description("NeedVerify")]
        NeedVerify = 1,
        [Description("PendingVerification")]
        PendingVerification = 2,
        [Description("Verified")]
        Verified = 3,
    }
}

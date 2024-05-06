using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum DealRequestVerificationStatus
    {
        [Description("Pending")]
        Pending = 0,
        [Description("Accepted")]
        Accepted = 1,
        [Description("Rejected")]
        Rejected = -1
    }
}

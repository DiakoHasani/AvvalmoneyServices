using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum UserStatus
    {
        [Description("Active")]
        Active = 1,

        [Description("Banned")]
        Banned = 0
    }
}

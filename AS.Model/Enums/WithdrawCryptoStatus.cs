using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum WithdrawCryptoStatus
    {
        Pending = 0,
        Fail = 1,
        Success = 2,
        Rejected = 3,
        PassToRobot = 4,
        RobotInProgress = 5
    }
}

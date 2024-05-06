using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum DealRequestStatus
    {
        InProgress = 0,
        Cancel = -1,
        Done = 1,
        ErrorZibal = 2,
        ErrorPaystar = 3,
        Returned = 4
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum DealRequestType
    {
        [Description("SellToAM")]
        SellToAM = 1,
        [Description("BuyFromAM")]
        BuyFromAM = 2,
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum CurrencyType
    {
        [Description("USDT_TRC20")]
        USDT_TRC20 = 40,

        [Description("Tron")]
        Tron = 50,

        [Description("Ton")]
        Ton = 130,

        [Description("NotCoin")]
        NotCoin = 140
    }
}

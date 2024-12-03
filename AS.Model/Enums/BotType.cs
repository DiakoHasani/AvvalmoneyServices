using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum BotType
    {
        [Description("تسویه حساب")]
        Withdraw = 0,

        [Description("تسویه کریپتو")]
        WithdrawCrypto = 1,

        [Description("آپدیت قیمت")]
        UpdatePrice = 2,

        [Description("درگاه ارز")]
        CryptoGateway = 3,

        [Description("درگاه ارز رزرو شده")]
        CryptoGatewayReservation = 3,

        [Description("بررسی تناقضات")]
        ContradictionBot = 4,
    }
}

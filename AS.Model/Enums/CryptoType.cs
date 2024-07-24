using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Enums
{
    public enum CryptoType
    {
        [Description("Default")]
        Default = 0,

        [Description("Tron")]
        Tron = 50,

        [Description("Ethereum")]
        Ethereum = 30,

        [Description("BinanceSmartChain")]
        BinanceSmartChain = 70,

        [Description("PerfectMoney")]
        PerfectMoney = 10,

        [Description("Bitcoin")]
        Bitcoin = 20,

        [Description("DogeCoin")]
        DogeCoin = 90,

        [Description("LiteCoin")]
        LiteCoin = 100,

        [Description("BitcoinCash")]
        BitcoinCash = 110,

        [Description("BitcoinCashSV")]
        BitcoinCashSV = 120,

        [Description("Ton")]
        Ton = 130,
    }
}

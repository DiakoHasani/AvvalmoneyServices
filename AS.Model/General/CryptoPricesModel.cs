using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.General
{
    public class CryptoPricesModel
    {
        public double BuyTether { get; set; }
        public double SellTether { get; set; }

        public double BuyTron { get; set; }
        public double SellTron { get; set; }

        public override string ToString()
        {
            return $"BuyTether:{BuyTether.ToString("N0")} __ SellTether:{SellTether.ToString("N0")} __ BuyTron:{BuyTron.ToString("N0")} __ SellTron:{SellTron.ToString("N0")}";
        }
    }
}

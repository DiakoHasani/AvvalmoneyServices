using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.CurrencyPriceHistory
{
    public class CurrencyPriceHistoryModel
    {
        public long CPH_Id { get; set; }
        public int Cur_Id { get; set; }
        public DateTime CPH_CreateDate { get; set; }
        public double CPH_SellPrice { get; set; }
        public double CPH_BuyPrice { get; set; }
        public long? AdmUsr_Id { get; set; }
    }
}

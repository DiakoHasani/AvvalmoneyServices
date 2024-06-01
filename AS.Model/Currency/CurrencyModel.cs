using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Currency
{
    public class CurrencyModel
    {
        public int Cur_Id { get; set; }
        public string Cur_ISOCode { get; set; }
        public string Cur_NameEN { get; set; }
        public string Cur_NameFA { get; set; }
        public bool Cur_IsActive { get; set; }
        public int Cur_OrderIndex { get; set; }
        public string Cur_ColorCode { get; set; }
    }
}

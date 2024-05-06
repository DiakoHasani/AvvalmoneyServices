using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawApi
{
    public class PostOptRequestModel
    {
        public string MaskCardNumber { get; set; }
        public string OPT { get; set; }
        public double Amount { get; set; }
        public string Time { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

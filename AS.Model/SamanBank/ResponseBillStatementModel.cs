using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.SamanBank
{
    public class ResponseBillStatementModel
    {
        public long Usr_Id { get; set; } = 0;
        public long Amount { get; set; } = 0;
        public string Operation { get; set; } = "";
        public bool Result { get; set; } = false;
        public string Message { get; set; } = "";
    }
}

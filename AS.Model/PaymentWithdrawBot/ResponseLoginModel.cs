using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.PaymentWithdrawBot
{
    public class ResponseLoginModel
    {
        public bool Result { get; set; } = false;
        public string Message { get; set; } = "";
        public int Code { get; set; } = 0;
        public string Token { get; set; } = "";
    }
}

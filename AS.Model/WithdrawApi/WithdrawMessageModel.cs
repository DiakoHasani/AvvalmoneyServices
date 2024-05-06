using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawApi
{
    public class WithdrawMessageModel
    {
        public int Code { get; set; }
        public string MessageText { get; set; }
        public bool Result { get; set; }
    }
}

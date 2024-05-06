using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawApi
{
    public class LastWithrawResponseModel
    {
        public long Id { get; set; }
        public string Price { get; set; }
        public string CardNumber { get; set; }
        public int WithrawType { get; set; }
    }
}

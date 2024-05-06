using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawApi
{
    public class UpdateLimitModel
    {
        public string ChargeBotCardMessage { get; set; } = "";
        public bool IsUpdate { get; set; } = false;
        public long ZibalBalance { get; set; } = 0;
    }
}

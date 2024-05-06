using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Pay98
{
    public class ResponsePay98NoFeeModel
    {
        [JsonProperty("quote")]
        public string Quote { get; set; }
    }
}

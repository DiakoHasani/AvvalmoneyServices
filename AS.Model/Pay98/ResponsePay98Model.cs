using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Pay98
{
    public class ResponsePay98Model
    {
        [JsonProperty("data")]
        public ResponsePay98DataModel Data { get; set; }
    }
}

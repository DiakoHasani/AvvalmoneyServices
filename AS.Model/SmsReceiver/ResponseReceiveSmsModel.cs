using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.SmsReceiver
{
    public class ResponseReceiveSmsModel
    {
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}

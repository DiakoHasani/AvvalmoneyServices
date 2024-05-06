using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.PaymentWithdrawBot
{
    public class ResponseBotAvailableModel
    {
        [JsonProperty("BotKey")]
        public string BotKey { get; set; }
    }
}

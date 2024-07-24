using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class ResponseNobitexStatNotCoinModel
    {
        [JsonProperty("not-rls")]
        public RlsModel Not { get; set; }
    }
}

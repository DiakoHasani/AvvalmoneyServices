using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class ResponseNobitexStatTronModel
    {
        [JsonProperty("trx-rls")]
        public RlsModel Tron { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class ResponseNobitexStatTetherModel
    {
        [JsonProperty("usdt-rls")]
        public RlsModel USDT { get; set; }
    }
}

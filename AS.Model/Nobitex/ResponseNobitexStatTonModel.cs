using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Nobitex
{
    public class ResponseNobitexStatTonModel
    {
        [JsonProperty("ton-rls")]
        public RlsModel Ton { get; set; }
    }
}

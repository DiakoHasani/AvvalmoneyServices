using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.UpdatePrice
{
    public class StaticModel
    {
        [JsonProperty("Nobitex")]
        public StaticNobitexModel Nobitex { get; set; }
    }
}

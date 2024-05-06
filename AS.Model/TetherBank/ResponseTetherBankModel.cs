using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.TetherBank
{
    public class ResponseTetherBankModel
    {
        [JsonProperty("currencies")]
        public List<ResponseTetherBankCurrencyModel> Currencies { get; set; }
    }
}

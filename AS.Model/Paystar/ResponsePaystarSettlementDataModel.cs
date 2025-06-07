using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class ResponsePaystarSettlementDataModel
    {
        [JsonProperty("hashid")]
        public string HashId { get; set; }

        [JsonProperty("track_id")]
        public string TrackId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("ref_code")]
        public string RefCode { get; set; }
    }
}

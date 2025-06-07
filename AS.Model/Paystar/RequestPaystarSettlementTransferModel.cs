using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.Paystar
{
    public class RequestPaystarSettlementTransferModel
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// شماره حساب مبدا
        /// </summary>
        [JsonProperty("deposit")]
        public string Deposit { get; set; }

        /// <summary>
        /// شماره حساب مقصد
        /// </summary>
        [JsonProperty("destination_account")]
        public string DestinationAccount { get; set; }

        /// <summary>
        /// نام صاحب حساب مقصد
        /// </summary>
        [JsonProperty("destination_firstname")]
        public string DestinationFirstname { get; set; }

        /// <summary>
        /// نام خانوادگی صاحب حساب مقصد
        /// </summary>
        [JsonProperty("destination_lastname")]
        public string DestinationLastName { get; set; }

        [JsonProperty("track_id")]
        public string TrackId { get; set; }

        /// <summary>
        /// شناسه واریز
        /// </summary>
        [JsonProperty("pay_id")]
        public string PayId { get; set; }
    }
}

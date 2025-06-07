using AS.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawWebhook
{
    public class RequestWebhookWithdrawCryptoModel
    {
        [JsonProperty("retetyyu")]
        public string WC_Id { get; set; }

        [JsonProperty("optuifg")]
        public string WC_Address { get; set; }

        [JsonProperty("xcbrrtys")]
        public string WC_Amount { get; set; }

        [JsonProperty("qwhpvb")]
        public string WC_CryptoType { get; set; }

        [Display(Name = "fhlowk")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [AuthenticationKeyValidation("{0} is invalid")]
        public string fhlowk { get; set; }
    }
}

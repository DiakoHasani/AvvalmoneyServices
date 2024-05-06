using AS.Model.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.WithdrawApi
{
    public class LoginRequestModel
    {
        [Display(Name = "UserName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        public string Password { get; set; }

        /// <summary>
        /// fhlowk:AuthenticationKey
        /// </summary>
        [Display(Name = "fhlowk")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [AuthenticationKeyValidation("{0} is invalid")]
        public string fhlowk { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.General
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AuthenticationKeyValidationAttribute: ValidationAttribute
    {
        public AuthenticationKeyValidationAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            return value.ToString().Equals(ServiceKeys.WithdrawKey);
        }
    }
}

using AS.Model.General;
using AS.Model.WithdrawApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class AccountService : IAccountService
    {
        public MessageModel WidthdrawLogin(LoginRequestModel model)
        {
            var result = new MessageModel();
            if (!(model.UserName == ServiceKeys.WithdrawUserName && model.Password == ServiceKeys.WithdrawPassword))
            {
                result.Message = "UserName and Password is Invalid";
                return result;
            }
            result.IsValid = true;
            return result;
        }
    }
    public interface IAccountService
    {
        MessageModel WidthdrawLogin(LoginRequestModel model);
    }
}

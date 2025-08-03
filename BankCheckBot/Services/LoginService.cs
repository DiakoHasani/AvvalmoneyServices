using BankCheckBot.Interfaces;
using BankCheckBot.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Services
{
    internal class LoginService(ILogger<LoginService> logger) : BaseApi, ILoginService
    {
        public async Task<string> LoginAsync()
        {
            try
            {
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(new AccountRequestModel
                {
                    UserName = ConstValues.WithdrawUserName,
                    Password = ConstValues.WithdrawPassword,
                    Fhlowk = ConstValues.WithdrawKey
                });
                var response = await Post($"{WithdrawApiUrl}api/Account/Login", parameters);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                }
                logger.LogError("error in login to WithdrawApi error message is: " + await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}

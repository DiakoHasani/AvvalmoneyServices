using BankCheckBot.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Services
{
    internal class SmsVerificationCodeService(ILogger<SmsVerificationCodeService> logger) : BaseApi, ISmsVerificationCodeService
    {
        public async Task<string> GetOptAsync(string bearerToken)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/SmsReceiver/GetLast/{ConstValues.WithdrawKey}", bearerToken);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"error in call GetOptAsync error message: {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}

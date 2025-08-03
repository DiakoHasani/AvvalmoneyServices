using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Interfaces
{
    public interface ISmsVerificationCodeService
    {
        Task<string> GetOptAsync(string bearerToken);
    }
}

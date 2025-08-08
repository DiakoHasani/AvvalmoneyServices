using BankCheckBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Interfaces
{
    public interface ISamanBankApiService
    {
        Task<List<ResponseBillStatementModel>> BillStatement(List<BillStatementTransactionModel> model, string token);
    }
}

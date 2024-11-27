using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
using AS.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> AddTransacton(RequestTransactionModel request)
        {
            if (request.Amount > 0)
            {
                if (request.TransactionType == TransactionType.Buy
                        || request.TransactionType == TransactionType.AdminDecrease
                        || request.TransactionType == TransactionType.Withdraw)
                {
                    request.Amount = request.Amount * -1;
                }
            }

            long befor = 0;
            if (_transactionRepository.GetAll().Where(p => p.Usr_Id == request.UserId).Any())
            {
                var s = _transactionRepository.GetAll().Where(p => p.Usr_Id == request.UserId).Select(p => p.Tns_Amount).ToList();
                for (int i = 0; i < s.Count; i++)
                {
                    befor += (long)s[i];
                }
            }

            if (befor < 0 && befor > 500000000)
            {
                befor = 0;
            }

            var after = befor + request.Amount;

            var transaction = new Transaction
            {
                Tns_Befor = befor,
                Tns_After = after,
                Tns_Amount = request.Amount,
                Tns_CreateDate = DateTime.Now,
                Tns_Id = Guid.NewGuid(),
                Usr_Id = request.UserId,
                Tns_Type = (int)request.TransactionType,
                Tns_Tag = "",
                AdmUsr_Id = request.AdminUserId
            };
            _transactionRepository.Add(transaction);
            await _transactionRepository.SaveChangeAsync();
            return transaction;
        }
    }
    public interface ITransactionService
    {
        Task<Transaction> AddTransacton(RequestTransactionModel request);
    }
}

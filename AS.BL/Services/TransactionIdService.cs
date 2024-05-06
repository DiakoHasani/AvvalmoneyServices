using AS.DAL;
using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TransactionIdService : ITransactionIdService
    {
        private readonly ITransactionIdRepository _transactionIdRepository;
        public TransactionIdService(ITransactionIdRepository transactionIdRepository)
        {
            _transactionIdRepository = transactionIdRepository;
        }

        public async Task<TransactionId> Add(string transactionIdCode, int walletId)
        {
            var transactionId = new TransactionId
            {
                TransactionIdCode = transactionIdCode,
                Wal_Id = walletId
            };
            _transactionIdRepository.Add(transactionId);
            await _transactionIdRepository.SaveChangeAsync();
            return transactionId;
        }

        public bool CheckExistTransactionIdCode(string transactionIdCode)
        {
            return _transactionIdRepository.GetAll(o=>o.TransactionIdCode==transactionIdCode).Any();
        }

        public string GetLastTxIdCodeByWalletId(int walletId)
        {
            return _transactionIdRepository.GetAll(o => o.Wal_Id == walletId).OrderByDescending(o => o.Tx_Id)?.FirstOrDefault().TransactionIdCode ?? null;
        }
    }
    public interface ITransactionIdService
    {
        string GetLastTxIdCodeByWalletId(int walletId);
        Task<TransactionId> Add(string transactionIdCode,int walletId);
        bool CheckExistTransactionIdCode(string transactionIdCode);
    }
}

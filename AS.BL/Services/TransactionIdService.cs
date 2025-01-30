using AS.DAL;
using AS.DAL.Services;
using AS.Model.TransactionId;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public TransactionIdService(ITransactionIdRepository transactionIdRepository,
            IMapper mapper)
        {
            _transactionIdRepository = transactionIdRepository;
            _mapper = mapper;
        }

        public async Task<TransactionIdModel> Add(TransactionIdModel model)
        {
            var transactionId = _mapper.Map<TransactionId>(model);
            _transactionIdRepository.Add(transactionId);
            await _transactionIdRepository.SaveChangeAsync();
            model.Tx_Id = transactionId.Tx_Id;
            return model;
        }

        public bool CheckExistTransactionIdCode(string transactionIdCode)
        {
            return _transactionIdRepository.GetAll(o => o.TransactionIdCode == transactionIdCode).Any();
        }

        public string GetLastTxIdCodeByWalletId(int walletId)
        {
            return _transactionIdRepository.GetAll(o => o.Wal_Id == walletId).OrderByDescending(o => o.Tx_Id)?.FirstOrDefault().TransactionIdCode ?? null;
        }

        public List<TransactionIdModel> GetTransactionIds(int Wal_Id, int limit)
        {
            return _transactionIdRepository.GetAll(o => o.Wal_Id == Wal_Id).OrderByDescending(o => o.Tx_Id).Skip(0).Take(limit).Select(o => new TransactionIdModel
            {
                TransactionIdCode = o.TransactionIdCode,
                Tx_Id = o.Tx_Id,
                Wal_Id = o.Wal_Id
            }).ToList();
        }
    }
    public interface ITransactionIdService
    {
        string GetLastTxIdCodeByWalletId(int walletId);
        Task<TransactionIdModel> Add(TransactionIdModel model);
        bool CheckExistTransactionIdCode(string transactionIdCode);
        List<TransactionIdModel> GetTransactionIds(int Wal_Id, int limit);
    }
}

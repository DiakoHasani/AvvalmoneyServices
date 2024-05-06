using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.WithdrawCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class WithdrawCryptoService : IWithdrawCryptoService
    {
        private readonly ILogger _logger;
        private readonly IWithdrawCryptoRepository _withdrawCryptoRepository;
        public WithdrawCryptoService(ILogger logger,
            IWithdrawCryptoRepository withdrawCryptoRepository)
        {
            _logger = logger;
            _withdrawCryptoRepository = withdrawCryptoRepository;
        }

        public async Task<WithdrawCrypto> GetById(long id)
        {
            return await _withdrawCryptoRepository.GetByIdAsync(id);
        }

        public ResponseWithdrawCryptoModel GetPendingWithdraw(WithdrawCryptoStatus status)
        {
            var date = DateTime.Now.AddMinutes(-20);
            return _withdrawCryptoRepository.GetAll().Where(o => o.WC_Status == (int)status &&
            o.WC_CreateDate > date).OrderBy(o => o.WC_Id).Select(o => new ResponseWithdrawCryptoModel
            {
                WC_Id = o.WC_Id,
                WC_Address = o.WC_Address,
                WC_Amount = o.WC_Amount,
                WC_CryptoType = (CurrencyType)o.WC_CryptoType
            }).FirstOrDefault();
        }

        public async Task<WithdrawCrypto> Update(WithdrawCrypto model)
        {
            _withdrawCryptoRepository.Update(model);
            await _withdrawCryptoRepository.SaveChangeAsync();
            return model;
        }
    }
    public interface IWithdrawCryptoService
    {
        ResponseWithdrawCryptoModel GetPendingWithdraw(WithdrawCryptoStatus status);
        Task<WithdrawCrypto> GetById(long id);
        Task<WithdrawCrypto> Update(WithdrawCrypto model);
    }
}

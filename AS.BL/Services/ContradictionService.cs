using AS.DAL.Services;
using AS.Log;
using AS.Model.Contradiction;
using AS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class ContradictionService : IContradictionService
    {
        private readonly ILogger _logger;
        private readonly IWithdrawCryptoRepository _withdrawCryptoRepository;

        public ContradictionService(ILogger logger,
            IWithdrawCryptoRepository withdrawCryptoRepository)
        {
            _logger = logger;
            _withdrawCryptoRepository = withdrawCryptoRepository;
        }

        public List<ContradictionModel> GetUndecided()
        {
            var fromDate = DateTime.Now.AddMinutes(-20);
            var toDate = DateTime.Now.AddMinutes(-13);

            var withdrawCryptos = _withdrawCryptoRepository.GetAll(o => (o.WC_Status == (int)WithdrawCryptoStatus.RobotInProgress || o.WC_Status == (int)WithdrawCryptoStatus.PassToRobot)
              && o.WC_CreateDate >= fromDate && o.WC_CreateDate <= toDate);

            return withdrawCryptos.Select(o => new ContradictionModel
            {
                WC_Address = o.WC_Address,
                WC_Amount = o.WC_Amount,
                WC_Id = o.WC_Id,
                WC_Status = (WithdrawCryptoStatus)o.WC_Status
            }).ToList();
        }

        public async Task<List<ContradictionModel>> Update(List<ContradictionModel> undecideds)
        {
            foreach (var item in undecideds)
            {
                var withdrawCrypto = _withdrawCryptoRepository.GetAll(o => o.WC_Id == item.WC_Id).FirstOrDefault();
                withdrawCrypto.WC_Status = (int)WithdrawCryptoStatus.Pending;
                item.WC_Status = WithdrawCryptoStatus.Pending;
                _withdrawCryptoRepository.Update(withdrawCrypto);
                await _withdrawCryptoRepository.SaveChangeAsync();
            }

            return undecideds;
        }
    }

    public interface IContradictionService
    {
        List<ContradictionModel> GetUndecided();
        Task<List<ContradictionModel>> Update(List<ContradictionModel> undecideds);
    }
}

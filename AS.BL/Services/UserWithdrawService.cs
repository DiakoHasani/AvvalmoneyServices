using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class UserWithdrawService : IUserWithdrawService
    {
        private readonly IUserWithdrawRepository _userWithdrawRepository;
        public UserWithdrawService(IUserWithdrawRepository userWithdrawRepository)
        {
            _userWithdrawRepository = userWithdrawRepository;
        }

        public UserWithdraw GetLastWaitingWithdraw()
        {
            return _userWithdrawRepository.GetAll(o => !o.Wit_Status.HasValue && o.Wit_DateCreate >= ServiceKeys.GetLastWaitingWithdrawTime &&
            !(o.SMS ?? false) && !(o.Bot ?? false)).OrderBy(o => o.Wit_Id).FirstOrDefault();
        }

        public async Task<CheckWithdrawPaymentStatus> CheckWithdrawPayment(UserWithdraw withdraw)
        {
            if (withdraw.Wit_Amount < ServiceKeys.MaximumAmountCardTransfer)
            {
                if ((withdraw.Try ?? 0) >= ServiceKeys.MaximumTryWithdraw)
                {
                    return await ConfigThenWithdrawTry(withdraw);
                }
                else
                {
                    return CheckWithdrawPaymentStatus.ReadyWithdrawToPayment;
                }
            }
            else
            {
                withdraw.SMS = true;
                _userWithdrawRepository.Update(withdraw);
                await _userWithdrawRepository.SaveChangeAsync();
                return CheckWithdrawPaymentStatus.WithdrawAmountIsMoreMaximumCard;
            }
        }

        public async Task<CheckWithdrawPaymentStatus> ConfigThenWithdrawTry(UserWithdraw withdraw)
        {
            withdraw.Try = withdraw.Try.HasValue ? withdraw.Try + 1 : 1;
            withdraw.SMS = true;
            _userWithdrawRepository.Update(withdraw);
            await _userWithdrawRepository.SaveChangeAsync();
            return CheckWithdrawPaymentStatus.ErrorWithdrawTry;
        }

        public async Task ErrorRepeatWithdraw(UserWithdraw withdraw)
        {
            withdraw.SMS = true;
            _userWithdrawRepository.Update(withdraw);
            await _userWithdrawRepository.SaveChangeAsync();
        }

        public async Task Update(UserWithdraw withdraw)
        {
            _userWithdrawRepository.Update(withdraw);
            await _userWithdrawRepository.SaveChangeAsync();
        }

        public async Task<UserWithdraw> GetByIdAsync(long id)
        {
            return await _userWithdrawRepository.GetByIdAsync(id);
        }
    }
    public interface IUserWithdrawService
    {
        UserWithdraw GetLastWaitingWithdraw();
        Task<CheckWithdrawPaymentStatus> CheckWithdrawPayment(UserWithdraw withdraw);
        Task<CheckWithdrawPaymentStatus> ConfigThenWithdrawTry(UserWithdraw withdraw);
        Task ErrorRepeatWithdraw(UserWithdraw withdraw);
        Task Update(UserWithdraw withdraw);
        Task<UserWithdraw> GetByIdAsync(long id);
    }
}

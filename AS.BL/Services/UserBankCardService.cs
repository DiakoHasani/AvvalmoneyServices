using AS.DAL;
using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class UserBankCardService : IUserBankCardService
    {
        private readonly IUserBankCardRepository _userBankCardRepository;
        public UserBankCardService(IUserBankCardRepository userBankCardRepository)
        {
            _userBankCardRepository = userBankCardRepository;
        }

        public async Task<UserBankCard> GetbyIdAsync(long id)
        {
            return await _userBankCardRepository.GetByIdAsync(id);
        }
    }
    public interface IUserBankCardService
    {
        Task<UserBankCard> GetbyIdAsync(long id);
    }
}

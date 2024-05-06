using AS.DAL;
using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> Update(User user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveChangeAsync();
            return user;
        }
    }
    public interface IUserService
    {
        Task<User> GetByIdAsync(long id);
        Task<User> Update(User user);
    }
}

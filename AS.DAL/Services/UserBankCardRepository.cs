using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class UserBankCardRepository : BaseRepository<UserBankCard>, IUserBankCardRepository
    {
        public UserBankCardRepository(DataContext context) : base(context)
        {

        }
    }
    public interface IUserBankCardRepository : IBaseRepository<UserBankCard>
    {
    }
}

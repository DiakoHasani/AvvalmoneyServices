using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class UserWithdrawRepository : BaseRepository<UserWithdraw>, IUserWithdrawRepository
    {
        public UserWithdrawRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IUserWithdrawRepository : IBaseRepository<UserWithdraw>
    {
    }
}

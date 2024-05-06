using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class UserWalletReservationRepository : BaseRepository<UserWalletReservation>, IUserWalletReservationRepository
    {
        public UserWalletReservationRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IUserWalletReservationRepository : IBaseRepository<UserWalletReservation>
    {
    }
}

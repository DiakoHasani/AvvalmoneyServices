using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class ReservationWalletRepository : BaseRepository<ReservationWallet>, IReservationWalletRepository
    {
        public ReservationWalletRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IReservationWalletRepository : IBaseRepository<ReservationWallet>
    {
    }
}

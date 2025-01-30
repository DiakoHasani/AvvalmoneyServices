using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class ReservationWalletRepository : BaseRepository<ReservationWallet>, IReservationWalletRepository
    {
        private readonly DataContext _dataContext;
        public ReservationWalletRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public List<ReservationWallet> GetReservations(DateTime fromDate, DateTime toDate, int cryptoType)
        {
            return _dataContext.ReservationWallets.Where(o => o.RW_CreateDate >= fromDate &&
            o.RW_CreateDate <= toDate &&
            o.RW_Status == false &&
            o.CryptoType == cryptoType)
                .Include(o => o.Wallet).ToList();
        }
    }
    public interface IReservationWalletRepository : IBaseRepository<ReservationWallet>
    {
        List<ReservationWallet> GetReservations(DateTime fromDate, DateTime toDate, int cryptoType);
    }
}

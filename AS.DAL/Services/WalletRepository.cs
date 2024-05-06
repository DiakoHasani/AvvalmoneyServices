using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(DataContext dataContext) : base(dataContext)
        {

        }

    }
    public interface IWalletRepository : IBaseRepository<Wallet>
    {
    }
}

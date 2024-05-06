using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class WithdrawCryptoRepository : BaseRepository<WithdrawCrypto>, IWithdrawCryptoRepository
    {
        public WithdrawCryptoRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IWithdrawCryptoRepository : IBaseRepository<WithdrawCrypto>
    {
    }
}

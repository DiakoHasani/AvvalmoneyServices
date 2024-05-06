using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class BotWithdrawTypeRepository :BaseRepository<BotWithrawType>, IBotWithdrawTypeRepository
    {
        public BotWithdrawTypeRepository(DataContext dataContext):base(dataContext)
        {

        }
    }
    public interface IBotWithdrawTypeRepository:IBaseRepository<BotWithrawType>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class BotInfoWithdrawRepository:BaseRepository<BotInfoWithraw>, IBotInfoWithdrawRepository
    {
        public BotInfoWithdrawRepository(DataContext dataContext):base(dataContext)
        {

        }
    }
    public interface IBotInfoWithdrawRepository:IBaseRepository<BotInfoWithraw>
    {
    }
}

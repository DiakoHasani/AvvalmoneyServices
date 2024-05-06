using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class BotCardsWithdrawRepository : BaseRepository<BotCardsWithraw>, IBotCardsWithdrawRepository
    {
        public BotCardsWithdrawRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IBotCardsWithdrawRepository : IBaseRepository<BotCardsWithraw>
    {
    }
}

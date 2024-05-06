using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class LifeLogBotWithdrawRepository : BaseRepository<LifeLogBotWithraw>, ILifeLogBotWithdrawRepository
    {
        public LifeLogBotWithdrawRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface ILifeLogBotWithdrawRepository : IBaseRepository<LifeLogBotWithraw>
    {
    }
}

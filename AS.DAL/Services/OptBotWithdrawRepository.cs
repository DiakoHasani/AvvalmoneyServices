using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class OptBotWithdrawRepository : BaseRepository<OptBotWithraw>, IOptBotWithdrawRepository
    {
        public OptBotWithdrawRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IOptBotWithdrawRepository : IBaseRepository<OptBotWithraw>
    {
    }
}

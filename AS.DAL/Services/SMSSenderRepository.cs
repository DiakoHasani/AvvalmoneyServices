using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class SMSSenderRepository : BaseRepository<SMSSender>, ISMSSenderRepository
    {
        private readonly DataContext _dataContext;
        public SMSSenderRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public void AddRange(List<SMSSender> model)
        {
            _dataContext.SMSSenders.AddRange(model);
        }
    }
    public interface ISMSSenderRepository : IBaseRepository<SMSSender>
    {
        void AddRange(List<SMSSender> model);
    }
}

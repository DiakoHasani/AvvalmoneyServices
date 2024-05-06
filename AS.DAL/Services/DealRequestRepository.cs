using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class DealRequestRepository : BaseRepository<DealRequest>, IDealRequestRepository
    {
        public DealRequestRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IDealRequestRepository : IBaseRepository<DealRequest>
    {
        
    }
}

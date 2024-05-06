using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class CurrencyRepository: BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface ICurrencyRepository:IBaseRepository<Currency>
    {
    }
}

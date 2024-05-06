using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class CurrencyPriceHistoryRepository:BaseRepository<CurrencyPriceHistory>, ICurrencyPriceHistoryRepository
    {
        public CurrencyPriceHistoryRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface ICurrencyPriceHistoryRepository: IBaseRepository<CurrencyPriceHistory>
    {
    }
}

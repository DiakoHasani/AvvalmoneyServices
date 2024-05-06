using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class TransactionRepository: BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DataContext dataContext):base(dataContext)
        {
        }
    }
    public interface ITransactionRepository:IBaseRepository<Transaction>
    {
    }
}

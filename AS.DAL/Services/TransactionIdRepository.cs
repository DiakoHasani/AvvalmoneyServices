using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class TransactionIdRepository : BaseRepository<TransactionId>, ITransactionIdRepository
    {
        public TransactionIdRepository(DataContext context) : base(context)
        {

        }
    }
    public interface ITransactionIdRepository : IBaseRepository<TransactionId>
    {
    }
}

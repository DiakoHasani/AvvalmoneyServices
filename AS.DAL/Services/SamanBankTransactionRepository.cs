using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class SamanBankTransactionRepository : BaseRepository<SamanBankTransaction>, ISamanBankTransactionRepository
    {
        public SamanBankTransactionRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface ISamanBankTransactionRepository : IBaseRepository<SamanBankTransaction>
    {
    }
}

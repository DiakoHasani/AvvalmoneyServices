using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger _logger;
        private readonly ICurrencyRepository _currencyRepository;
        public CurrencyService(ILogger logger,
            ICurrencyRepository currencyRepository)
        {
            _logger = logger;
            _currencyRepository = currencyRepository;
        }

        public int GetCur_IdByISOCode(string isoCode)
        {
            return _currencyRepository.GetAll(o => o.Cur_ISOCode == isoCode).Select(o => o.Cur_Id).FirstOrDefault();
        }
    }
    public interface ICurrencyService
    {
        int GetCur_IdByISOCode(string isoCode);
    }
}

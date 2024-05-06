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
    public class CurrencyPriceHistoryService : ICurrencyPriceHistoryService
    {
        private readonly ILogger _logger;
        private readonly ICurrencyPriceHistoryRepository _currencyPriceHistoryRepository;
        public CurrencyPriceHistoryService(ILogger logger,
            ICurrencyPriceHistoryRepository currencyPriceHistoryRepository)
        {
            _logger = logger;
            _currencyPriceHistoryRepository = currencyPriceHistoryRepository;
        }

        public async Task<CurrencyPriceHistory> Add(CurrencyPriceHistory model)
        {
            _currencyPriceHistoryRepository.Add(model);
            await _currencyPriceHistoryRepository.SaveChangeAsync();
            return model;
        }

        public CurrencyPriceHistory GetByCur_Id(int cur_id)
        {
            return _currencyPriceHistoryRepository.GetAll(o => o.Cur_Id == cur_id).OrderByDescending(o => o.CPH_Id).FirstOrDefault();
        }

        public async Task<CurrencyPriceHistory> Update(CurrencyPriceHistory model)
        {
            _currencyPriceHistoryRepository.Update(model);
            await _currencyPriceHistoryRepository.SaveChangeAsync();
            return model;
        }
    }
    public interface ICurrencyPriceHistoryService
    {
        Task<CurrencyPriceHistory> Update(CurrencyPriceHistory model);
        Task<CurrencyPriceHistory> Add(CurrencyPriceHistory model);
        CurrencyPriceHistory GetByCur_Id(int cur_id);
    }
}

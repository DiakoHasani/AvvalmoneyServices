using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.CurrencyPriceHistory;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public CurrencyPriceHistoryService(ILogger logger,
            ICurrencyPriceHistoryRepository currencyPriceHistoryRepository,
            IMapper mapper)
        {
            _logger = logger;
            _currencyPriceHistoryRepository = currencyPriceHistoryRepository;
            _mapper= mapper;
        }

        public async Task<CurrencyPriceHistoryModel> Add(CurrencyPriceHistoryModel model)
        {
            var currencyPriceHistory = _mapper.Map<CurrencyPriceHistory>(model);
            _currencyPriceHistoryRepository.Add(currencyPriceHistory);
            await _currencyPriceHistoryRepository.SaveChangeAsync();
            return _mapper.Map<CurrencyPriceHistoryModel>(currencyPriceHistory);
        }

        public CurrencyPriceHistoryModel GetByCur_Id(int cur_id)
        {
            return _mapper.Map<CurrencyPriceHistoryModel>(_currencyPriceHistoryRepository.GetAll(o => o.Cur_Id == cur_id).OrderByDescending(o => o.CPH_Id).First());
        }

        public async Task<CurrencyPriceHistoryModel> Update(CurrencyPriceHistoryModel model)
        {
            _currencyPriceHistoryRepository.Update(_mapper.Map<CurrencyPriceHistory>(model));
            await _currencyPriceHistoryRepository.SaveChangeAsync();
            return model;
        }
    }
    public interface ICurrencyPriceHistoryService
    {
        Task<CurrencyPriceHistoryModel> Update(CurrencyPriceHistoryModel model);
        Task<CurrencyPriceHistoryModel> Add(CurrencyPriceHistoryModel model);
        CurrencyPriceHistoryModel GetByCur_Id(int cur_id);
    }
}

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
            var currencyPriceHistory = new CurrencyPriceHistory
            {
                AdmUsr_Id=model.AdmUsr_Id,
                CPH_BuyPrice=(long)model.CPH_BuyPrice,
                CPH_CreateDate=model.CPH_CreateDate,
                CPH_SellPrice=(long)model.CPH_SellPrice,
                Cur_Id=model.Cur_Id,
            };
            _currencyPriceHistoryRepository.Add(currencyPriceHistory);
            await _currencyPriceHistoryRepository.SaveChangeAsync();
            model.CPH_Id = currencyPriceHistory.CPH_Id;
            return model;
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

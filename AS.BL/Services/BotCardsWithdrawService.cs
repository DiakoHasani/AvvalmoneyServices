using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.WithdrawApi;
using AS.Model.Zibal;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class BotCardsWithdrawService : IBotCardsWithdrawService
    {
        private readonly IBotCardsWithdrawRepository _botCardsWithdrawRepository;
        private readonly ILogger _logger;
        private readonly IZibalService _zibalService;
        private readonly IBotInfoWithdrawService _botInfoWithdrawService;
        public BotCardsWithdrawService(IBotCardsWithdrawRepository botCardsWithdrawRepository,
            ILogger logger,
            IZibalService zibalService,
            IBotInfoWithdrawService botInfoWithdrawService)
        {
            _botCardsWithdrawRepository = botCardsWithdrawRepository;
            _logger = logger;
            _zibalService = zibalService;
            _botInfoWithdrawService = botInfoWithdrawService;
        }

        public BotCardsWithraw GetAvailableBotCard(double price, Guid key)
        {
            var botCards = GetByBankKey(key);
            foreach (var item in botCards)
            {
                if (price <= item.Limit)
                {
                    return item;
                }
            }
            return null;
        }

        public List<BotCardsWithraw> GetByBankKey(Guid key)
        {
            return _botCardsWithdrawRepository.GetAll(o => o.BankKey == key && o.Enabled).OrderBy(o => o.Order).ToList();
        }

        public BotCardsWithraw GetByIndex(string index, Guid key)
        {
            return _botCardsWithdrawRepository.GetAll(o => o.Index == index && o.BankKey == key).FirstOrDefault();
        }

        public async Task Update(BotCardsWithraw model)
        {
            _botCardsWithdrawRepository.Update(model);
            await _botCardsWithdrawRepository.SaveChangeAsync();
        }

        public async Task<UpdateLimitModel> UpdateLimit(BotInfoWithraw bot, BotCardsWithraw card, long zibalBalance)
        {
            var result = new UpdateLimitModel();
            try
            {
                var dateNow = DateTime.Now;
                var date = card.DateUpdate ?? dateNow.AddDays(-1);
                var lastDate = new DateTime(date.Year, date.Month, date.Day);
                var nowDate = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);

                if (nowDate > lastDate)
                {
                    if (card.Limit.ToInt64() < ServiceKeys.MaximumLimitBankCard)
                    {
                        if (zibalBalance >= card.Limit.ToInt64())
                        {
                            var checkOutZibal = await _zibalService.CheckOut(new ZibalCheckoutModel
                            {
                                Amount = (ServiceKeys.MaximumLimitBankCard - card.Limit.ToInt64()) * 10,
                                Id = ServiceKeys.ZibalId,
                                CheckoutDelay = -1,
                                Bank = "saman",
                                BankAccount = card.Shaba
                            });

                            _logger.Information("call CheckOut zibal", new { card = card, checkOutZibal = checkOutZibal });

                            if (checkOutZibal.Result != 1)
                            {
                                if (checkOutZibal.Message.Equals("موفق"))
                                {
                                    zibalBalance -= ServiceKeys.MaximumLimitBankCard - card.Limit.ToInt64();
                                }
                            }

                            result.ChargeBotCardMessage = $"{card.CardNumber} = {checkOutZibal.Message} . \n";
                        }
                    }
                    card.Limit = ServiceKeys.MaximumLimitBankCard;
                    card.DateUpdate = DateTime.Now;
                    result.IsUpdate = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return result;
            }
        }

        public async Task<string> UpdateLimites()
        {
            _logger.Information("called UpdateLimites");


            long zibalBalance = await _zibalService.GetBalance();
            _logger.Information("get zibalBalance", new { zibalBalance = zibalBalance });

            var resultChargeBotCardMessage = "";
            var update = false;

            _botInfoWithdrawService.GetAll().ForEach(async bot =>
            {
                foreach (var item in _botCardsWithdrawRepository.GetAll(o => o.BankKey == bot.Key && o.Enabled).OrderBy(o => o.Order).ToList())
                {
                    var resultUpdateLimit = await UpdateLimit(bot, item, zibalBalance);
                    resultChargeBotCardMessage += resultUpdateLimit.ChargeBotCardMessage;

                    if (resultUpdateLimit.IsUpdate)
                    {
                        update = true;
                    }

                    zibalBalance = resultUpdateLimit.ZibalBalance;
                }
            });
            if (update)
            {
                await _botCardsWithdrawRepository.SaveChangeAsync();
            }
            return resultChargeBotCardMessage;
        }

    }
    public interface IBotCardsWithdrawService
    {
        Task<string> UpdateLimites();
        Task<UpdateLimitModel> UpdateLimit(BotInfoWithraw bot, BotCardsWithraw card, long zibalBalance);
        List<BotCardsWithraw> GetByBankKey(Guid key);
        BotCardsWithraw GetAvailableBotCard(double price, Guid key);
        BotCardsWithraw GetByIndex(string index, Guid key);
        Task Update(BotCardsWithraw model);
    }
}

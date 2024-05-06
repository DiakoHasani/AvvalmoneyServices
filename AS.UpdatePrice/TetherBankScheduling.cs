using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.TetherBank;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class TetherBankScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private readonly ITetherBankService _tetherBankService;

        private IPrint _print;
        int TetherCur_Id, TronCur_Id = 0;
        private ResponseTetherBankModel responseTetherBank;
        public TetherBankScheduling(ILogger logger,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            ITetherBankService tetherBankService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _tetherBankService = tetherBankService;

            TetherCur_Id = _currencyService.GetCur_IdByISOCode(ISOCode.Tether_TRC20.GetDescription());
            TronCur_Id = _currencyService.GetCur_IdByISOCode(ISOCode.Tron.GetDescription());

            Start(Run);
        }

        public void SetPrint(IPrint print)
        {
            _print = print;
        }

        private async Task Run()
        {
            try
            {
                Stop();
                responseTetherBank = await _tetherBankService.Get();
                if (responseTetherBank != null)
                {
                    _logger.Information("responseTetherBank value is", responseTetherBank);

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "USDT").TomanPrice.ToPrice(),
                        CPH_SellPrice = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "USDT").TomanPrice.ToPrice() - 200,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TetherCur_Id
                    });

                    _logger.Information("added Tether to Database");

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "TRX").TomanPrice.ToPrice(),
                        CPH_SellPrice = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "TRX").TomanPrice.ToPrice() - 100,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TronCur_Id
                    });

                    _logger.Information("added Tron to Database");
                }
                else
                {
                    _logger.Error("responseTetherBank is null");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                _print.Show("_______________________________________________");
                Continue();
            }
        }
    }
}

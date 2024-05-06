using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Nobitex;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AS.UpdatePrice
{
    public class NobitexScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly INobitexService _nobitexService;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private IPrint _print;


        double tetherAmount, tronAmount = 0;
        int TetherCur_Id, TronCur_Id = 0;

        public NobitexScheduling(ILogger logger,
            INobitexService nobitexService,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService)
        {
            _logger = logger;
            _nobitexService = nobitexService;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;

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
                tetherAmount = await _nobitexService.GetTetherAmount();
                _logger.Information($"tetherAmount Nobitex is {tetherAmount}");

                tronAmount = await _nobitexService.GetTronAmount();
                _logger.Information($"tronAmount Nobitex is {tronAmount}");

                if (tetherAmount > 0)
                {
                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tetherAmount + 500,
                        CPH_SellPrice = tetherAmount - 300,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TetherCur_Id
                    });
                    _logger.Information("added Tether to Database");
                }

                if (tronAmount > 0)
                {
                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tronAmount + 100,
                        CPH_SellPrice = tronAmount - 100,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TronCur_Id
                    });
                    _logger.Information("added Tron to Database");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Exception in NobitexScheduling.Run", ex);
            }
            finally
            {
                _print.Show("_______________________________________________");
                Continue();
            }
        }
    }
}

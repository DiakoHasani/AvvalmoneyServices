using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.Ex4Ir;
using AS.Model.General;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class RangeScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private readonly IRangePriceService _rangePriceService;

        private IPrint _print;

        int TetherCur_Id, TronCur_Id = 0;

        CryptoPricesModel cryptoPrices;

        public RangeScheduling(ILogger logger,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            IRangePriceService rangePriceService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _rangePriceService = rangePriceService;

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
                cryptoPrices = await _rangePriceService.Get();

                if(cryptoPrices != null)
                {
                    _logger.Information("cryptoPrices value is", cryptoPrices);

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = cryptoPrices.BuyTether,
                        CPH_SellPrice = cryptoPrices.SellTether,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TetherCur_Id
                    });

                    _logger.Information("added Tether to Database");

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = cryptoPrices.BuyTron,
                        CPH_SellPrice = cryptoPrices.SellTron,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TronCur_Id
                    });

                    _logger.Information("added Tron to Database");
                }
                else
                {
                    _logger.Error("cryptoPrices is null");
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

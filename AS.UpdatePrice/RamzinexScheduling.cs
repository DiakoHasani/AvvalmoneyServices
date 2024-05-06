using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Ramzinex;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class RamzinexScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private IPrint _print;
        private readonly IRamzinexService _ramzinexService;

        private ResponseRamzinexModel responseRamzinex;
        int TetherCur_Id, TronCur_Id = 0;

        public RamzinexScheduling(ILogger logger,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            IRamzinexService ramzinexService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _ramzinexService = ramzinexService;

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
                responseRamzinex = await _ramzinexService.Get();

                if (responseRamzinex != null)
                {
                    _logger.Information("responseRamzinex value is", responseRamzinex);

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "usdt").Buy.RialToToman(),
                        CPH_SellPrice = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "usdt").Sell.RialToToman(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TetherCur_Id
                    });

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "trx").Buy.RialToToman(),
                        CPH_SellPrice = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "trx").Sell.RialToToman(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TronCur_Id
                    });

                    _logger.Information("added Price to database");
                }
                else
                {
                    _logger.Error("responseRamzinex is null");
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

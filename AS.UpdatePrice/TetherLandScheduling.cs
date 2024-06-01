using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Tetherland;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class TetherLandScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private IPrint _print;
        private readonly ITetherlandService _tetherlandService;

        int TetherCur_Id, TronCur_Id = 0;
        ResponseTetherlandModel responseTetherland;
        public TetherLandScheduling(ILogger logger,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            ITetherlandService tetherlandService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _tetherlandService = tetherlandService;

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
                responseTetherland = await _tetherlandService.Get();

                if (responseTetherland != null)
                {
                    _logger.Information("responseTetherland value is", responseTetherland);

                    //await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    //{
                    //    AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                    //    CPH_BuyPrice = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "USDT").TomanAmount.ToDouble(),
                    //    CPH_SellPrice = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "USDT").TomanAmount.ToDouble(),
                    //    CPH_CreateDate = DateTime.Now,
                    //    Cur_Id = TetherCur_Id
                    //});

                    //await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    //{
                    //    AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                    //    CPH_BuyPrice = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "TRX").TomanAmount.ToDouble(),
                    //    CPH_SellPrice = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "TRX").TomanAmount.ToDouble(),
                    //    CPH_CreateDate = DateTime.Now,
                    //    Cur_Id = TronCur_Id
                    //});

                    _logger.Information("added Price to database");
                }
                else
                {
                    _logger.Error("responseTetherland is null");
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

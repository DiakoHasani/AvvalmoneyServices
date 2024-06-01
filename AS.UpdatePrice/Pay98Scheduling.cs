using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.CurrencyPriceHistory;
using AS.Model.Enums;
using AS.Model.General;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class Pay98Scheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly IPay98Service _pay98Service;
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;
        private IPrint _print;
        int TetherCur_Id, TronCur_Id = 0;
        double tronSellAmount, tronBuyAmount, tetherBuyAmount, tetherSellAmount = 0;

        public Pay98Scheduling(ILogger logger,
            IPay98Service pay98Service,
            ICurrencyApiService currencyApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService)
        {
            _logger = logger;
            _pay98Service = pay98Service;
            _currencyApiService = currencyApiService;
            _currencyPriceHistoryApiService = currencyPriceHistoryApiService;

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
                tronSellAmount = await _pay98Service.GetTronAmount(DealType.Sell);
                await Task.Delay(3000);
                tronBuyAmount = await _pay98Service.GetTronAmount(DealType.Buy);

                await Task.Delay(3000);
                tetherSellAmount = await _pay98Service.GetTetherAmount(DealType.Sell);
                await Task.Delay(3000);
                tetherBuyAmount = await _pay98Service.GetTetherAmount(DealType.Buy);

                if (tetherSellAmount != 0 && tetherBuyAmount != 0)
                {
                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tetherBuyAmount,
                        CPH_SellPrice = tetherSellAmount,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTetherCur_Id()
                    });
                    _logger.Information("added Tether to Database");
                }

                if (tronSellAmount != 0 && tronBuyAmount != 0)
                {
                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tronBuyAmount,
                        CPH_SellPrice = tronSellAmount,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTronCur_Id()
                    });
                    _logger.Information("added Tron to Database");
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

        private async Task<int> GetTetherCur_Id()
        {
            if (TetherCur_Id <= 0)
                TetherCur_Id = await _currencyApiService.GetCur_IdByISOCode(ISOCode.Tether_TRC20.GetDescription());
            return TetherCur_Id;
        }

        private async Task<int> GetTronCur_Id()
        {
            if (TronCur_Id <= 0)
                TronCur_Id = await _currencyApiService.GetCur_IdByISOCode(ISOCode.Tron.GetDescription());
            return TronCur_Id;
        }
    }
}

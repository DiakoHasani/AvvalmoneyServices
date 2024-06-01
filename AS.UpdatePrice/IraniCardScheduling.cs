using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.CurrencyPriceHistory;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.IraniCard;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.UpdatePrice
{
    public class IraniCardScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private IPrint _print;
        private readonly IIraniCardService _iraniCardService;
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;

        private ResponseIraniCardModel responseIraniCard;
        int TetherCur_Id, TronCur_Id = 0;
        public IraniCardScheduling(ILogger logger,
            IIraniCardService iraniCardService,
            ICurrencyApiService currencyApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService)
        {
            _logger = logger;
            _iraniCardService = iraniCardService;
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
                responseIraniCard = await _iraniCardService.Get();

                if (responseIraniCard != null)
                {
                    _logger.Information("responseIraniCard value is", responseIraniCard);

                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseIraniCard.USDT.Buy.Price.RialToToman(),
                        CPH_SellPrice = responseIraniCard.USDT.Sell.Price.RialToToman(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTetherCur_Id()
                    });

                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseIraniCard.TRX.Buy.Price.RialToToman(),
                        CPH_SellPrice=responseIraniCard.TRX.Sell.Price.RialToToman(),
                        CPH_CreateDate= DateTime.Now,
                        Cur_Id= await GetTronCur_Id()
                    });

                    _logger.Information("added Price to database");
                }
                else
                {
                    _logger.Error("responseIraniCard is null");
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

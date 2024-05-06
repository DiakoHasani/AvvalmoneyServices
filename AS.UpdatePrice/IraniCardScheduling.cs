using AS.BL.Services;
using AS.DAL;
using AS.Log;
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
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private IPrint _print;
        private readonly IIraniCardService _iraniCardService;

        private ResponseIraniCardModel responseIraniCard;
        int TetherCur_Id, TronCur_Id = 0;
        public IraniCardScheduling(ILogger logger,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            IIraniCardService iraniCardService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _iraniCardService = iraniCardService;

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
                responseIraniCard = await _iraniCardService.Get();

                if (responseIraniCard != null)
                {
                    _logger.Information("responseIraniCard value is", responseIraniCard);

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseIraniCard.USDT.Buy.Price.RialToToman(),
                        CPH_SellPrice = responseIraniCard.USDT.Sell.Price.RialToToman(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TetherCur_Id
                    });

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseIraniCard.TRX.Buy.Price.RialToToman(),
                        CPH_SellPrice=responseIraniCard.TRX.Sell.Price.RialToToman(),
                        CPH_CreateDate= DateTime.Now,
                        Cur_Id= TronCur_Id
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
    }
}

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
using System.Timers;

namespace AS.UpdatePrice
{
    public class Ex4IrScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyPriceHistoryService _currencyPriceHistoryService;
        private readonly IEx4IrService _ex4IrService;
        private IPrint _print;


        private List<ResponseEx4IrModel> responseEx4Irs;
        int TetherCur_Id, TronCur_Id = 0;

        public Ex4IrScheduling(ILogger logger,
            ICurrencyService currencyService,
            ICurrencyPriceHistoryService currencyPriceHistoryService,
            IEx4IrService ex4IrService)
        {
            _logger = logger;
            _currencyService = currencyService;
            _currencyPriceHistoryService = currencyPriceHistoryService;
            _ex4IrService = ex4IrService;

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
                responseEx4Irs = await _ex4IrService.Get();

                if (responseEx4Irs.Count != 0)
                {
                    _logger.Information("responseEx4Irs value is", responseEx4Irs);

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "USDT").BuyPrice.ToDouble(),
                        CPH_SellPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "USDT").SellPrice.ToDouble(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TetherCur_Id
                    });

                    _logger.Information("added Tether to Database");

                    await _currencyPriceHistoryService.Add(new CurrencyPriceHistory
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "TRX").BuyPrice.ToDouble(),
                        CPH_SellPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "TRX").SellPrice.ToDouble(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = TronCur_Id
                    });

                    _logger.Information("added Tron to Database");
                }
                else
                {
                    _logger.Error("responseEx4Irs is empty");
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

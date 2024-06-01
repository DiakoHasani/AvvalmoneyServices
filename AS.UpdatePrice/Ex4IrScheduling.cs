using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.CurrencyPriceHistory;
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
        private readonly IEx4IrService _ex4IrService;
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;
        private IPrint _print;


        private List<ResponseEx4IrModel> responseEx4Irs;
        int TetherCur_Id, TronCur_Id = 0;

        public Ex4IrScheduling(ILogger logger,
            IEx4IrService ex4IrService,
            ICurrencyApiService currencyApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService)
        {
            _logger = logger;
            _ex4IrService = ex4IrService;
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
                responseEx4Irs = await _ex4IrService.Get();

                if (responseEx4Irs.Count != 0)
                {
                    _logger.Information("responseEx4Irs value is", responseEx4Irs);

                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "USDT").BuyPrice.ToDouble(),
                        CPH_SellPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "USDT").SellPrice.ToDouble(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTetherCur_Id()
                    });

                    _logger.Information("added Tether to Database");

                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "TRX").BuyPrice.ToDouble(),
                        CPH_SellPrice = responseEx4Irs.FirstOrDefault(o => o.Symbol == "TRX").SellPrice.ToDouble(),
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTronCur_Id()
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

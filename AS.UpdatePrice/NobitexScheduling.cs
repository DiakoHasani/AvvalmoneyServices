using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.CurrencyPriceHistory;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Nobitex;
using AS.Model.PaymentWithdrawBot;
using AS.Model.UpdatePrice;
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
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;
        private readonly IWithdrawApiService _withdrawApiService;
        private readonly IStatics _statics;
        private IPrint _print;

        private string token = "";
        private DateTime loginDate = DateTime.Now;
        double tetherAmount, tronAmount, tonAmount, notAmount = 0;
        int TetherCur_Id, TronCur_Id, TonCur_Id, NotCur_Id = 0;
        StaticModel statics;

        public NobitexScheduling(ILogger logger,
            INobitexService nobitexService,
            ICurrencyApiService currencyApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService,
            IWithdrawApiService withdrawApiService,
            IStatics statics)
        {
            _logger = logger;
            _nobitexService = nobitexService;
            _currencyApiService = currencyApiService;
            _currencyPriceHistoryApiService = currencyPriceHistoryApiService;
            _withdrawApiService = withdrawApiService;
            _statics = statics;


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

                if (loginDate < DateTime.Now)
                {
                    do
                    {
                        var login = await Login();
                        if (!string.IsNullOrEmpty(login))
                        {
                            token = login;
                        }
                        else
                        {
                            await Task.Delay(10000);
                        }
                    } while (string.IsNullOrWhiteSpace(token));
                }

                tetherAmount = await _nobitexService.GetTetherAmount();
                _logger.Information($"tetherAmount Nobitex is {tetherAmount}");

                tronAmount = await _nobitexService.GetTronAmount();
                _logger.Information($"tronAmount Nobitex is {tronAmount}");

                tonAmount = await _nobitexService.GetTonAmount();
                _logger.Information($"tonAmount Nobitex is {tonAmount}");

                notAmount = await _nobitexService.GetNotCoinAmount();
                _logger.Information($"notAmount Nobitex is {notAmount}");

                statics = _statics.GetStatics();

                if (tetherAmount > 0)
                {
                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tetherAmount + statics.Nobitex.Usdt.Buy,
                        CPH_SellPrice = tetherAmount + statics.Nobitex.Usdt.Sell,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTetherCur_Id()
                    }, token);
                    _logger.Information("added Tether to Database");
                }

                if (tronAmount > 0)
                {
                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tronAmount + statics.Nobitex.Trx.Buy,
                        CPH_SellPrice = tronAmount + statics.Nobitex.Trx.Sell,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTronCur_Id()
                    }, token);
                    _logger.Information("added Tron to Database");
                }

                if (tonAmount > 0)
                {
                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = tonAmount + statics.Nobitex.Ton.Buy,
                        CPH_SellPrice = tonAmount + statics.Nobitex.Ton.Sell,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetTonCur_Id()
                    }, token);
                    _logger.Information("added Ton to Database");
                }

                if (notAmount > 0)
                {
                    await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                    {
                        AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                        CPH_BuyPrice = notAmount + statics.Nobitex.Not.Buy,
                        CPH_SellPrice = notAmount + statics.Nobitex.Not.Sell,
                        CPH_CreateDate = DateTime.Now,
                        Cur_Id = await GetNotCur_Id()
                    }, token);
                    _logger.Information("added NotCoin to Database");
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

        private async Task<int> GetTetherCur_Id()
        {
            if (TetherCur_Id <= 0)
                TetherCur_Id = await _currencyApiService.GetCur_IdByISOCode(ISOCode.Tether_TRC20.GetDescription(), token);
            return TetherCur_Id;
        }

        private async Task<int> GetTronCur_Id()
        {
            if (TronCur_Id <= 0)
                TronCur_Id = await _currencyApiService.GetCur_IdByISOCode(ISOCode.Tron.GetDescription(), token);
            return TronCur_Id;
        }

        private async Task<int> GetTonCur_Id()
        {
            if (TonCur_Id <= 0)
                TonCur_Id = await _currencyApiService.GetCur_IdByISOCode(ISOCode.Ton.GetDescription(), token);
            return TonCur_Id;
        }

        private async Task<int> GetNotCur_Id()
        {
            if (NotCur_Id <= 0)
                NotCur_Id = await _currencyApiService.GetCur_IdByISOCode(ISOCode.NotCoin.GetDescription(), token);
            return NotCur_Id;
        }

        private async Task<string> Login()
        {
            var message = await _withdrawApiService.Login(new RequestLoginModel
            {
                Fhlowk = ServiceKeys.WithdrawKey,
                UserName = ServiceKeys.WithdrawUserName,
                Password = ServiceKeys.WithdrawPassword,
            });

            if (message.Result)
            {
                _logger.Information(message.Message);
                loginDate = DateTime.Now.AddDays(ServiceKeys.WithdrawTimeLoginUpdatePriceNumber);
                return message.Token;
            }

            return null;
        }
    }
}

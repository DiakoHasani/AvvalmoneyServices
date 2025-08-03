using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.Ramzinex;
using AS.Model.UpdatePrice;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.Model.PaymentWithdrawBot;
using AS.Model.CurrencyPriceHistory;

namespace AS.UpdatePrice
{
    public class RamzinexScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private IPrint _print;
        private readonly IRamzinexService _ramzinexService;
        private readonly IStatics _statics;
        private readonly ICurrencyApiService _currencyApiService;
        private readonly ICurrencyPriceHistoryApiService _currencyPriceHistoryApiService;
        private readonly IWithdrawApiService _withdrawApiService;

        private string token = "";
        private DateTime loginDate = DateTime.Now;
        private ResponseRamzinexModel responseRamzinex;
        int TetherCur_Id, TronCur_Id, TonCur_Id, NotCur_Id = 0;
        double tetherBuyPrice, tetherSellPrice, tronBuyPrice, tronSellPrice, tonBuyPrice, tonSellPrice, notBuyPrice, notSellPrice = 0;
        StaticModel statics;

        public RamzinexScheduling(ILogger logger,
            IRamzinexService ramzinexService,
            IStatics statics,
            ICurrencyApiService currencyApiService,
            ICurrencyPriceHistoryApiService currencyPriceHistoryApiService,
            IWithdrawApiService withdrawApiService)
        {
            _logger = logger;
            _ramzinexService = ramzinexService;
            _statics = statics;
            _currencyApiService = currencyApiService;
            _currencyPriceHistoryApiService = currencyPriceHistoryApiService;
            _withdrawApiService = withdrawApiService;
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

                responseRamzinex = await _ramzinexService.Get();

                if (responseRamzinex != null)
                {
                    if (responseRamzinex.Data != null)
                    {
                        FillTetherPrice();
                        FillTronPrice();
                        FillNotPrice();
                        FillTonPrice();
                    }

                    statics = _statics.GetStatics();

                    if (tetherBuyPrice > 0)
                    {
                        if (tetherBuyPrice + statics.Nobitex.Usdt.Buy > 0 && tetherBuyPrice + statics.Nobitex.Usdt.Sell > 0)
                        {
                            await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                            {
                                AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                                CPH_BuyPrice =(long) (tetherBuyPrice + statics.Nobitex.Usdt.Buy),
                                CPH_SellPrice =(long) (tetherBuyPrice + statics.Nobitex.Usdt.Sell),
                                CPH_CreateDate = DateTime.Now,
                                Cur_Id = await GetTetherCur_Id()
                            }, token);
                        }
                    }

                    if (tronBuyPrice > 0)
                    {
                        if (tronBuyPrice + statics.Nobitex.Trx.Buy > 0 && tronBuyPrice + statics.Nobitex.Trx.Sell > 0)
                        {
                            await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                            {
                                AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                                CPH_BuyPrice =(long) (tronBuyPrice + statics.Nobitex.Trx.Buy),
                                CPH_SellPrice =(long) (tronBuyPrice + statics.Nobitex.Trx.Sell),
                                CPH_CreateDate = DateTime.Now,
                                Cur_Id = await GetTronCur_Id()
                            }, token);
                        }
                    }

                    if (tonBuyPrice > 0)
                    {
                        if (tonBuyPrice + statics.Nobitex.Ton.Buy > 0 && tonBuyPrice + statics.Nobitex.Ton.Sell > 0)
                        {
                            await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                            {
                                AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                                CPH_BuyPrice =(long) (tonBuyPrice + statics.Nobitex.Ton.Buy),
                                CPH_SellPrice =(long) (tonBuyPrice + statics.Nobitex.Ton.Sell),
                                CPH_CreateDate = DateTime.Now,
                                Cur_Id = await GetTonCur_Id()
                            }, token);
                        }
                    }

                    if (notBuyPrice > 0)
                    {
                        if (notBuyPrice + statics.Nobitex.Not.Buy > 0 && notBuyPrice + statics.Nobitex.Not.Sell > 0)
                        {
                            await _currencyPriceHistoryApiService.Add(new CurrencyPriceHistoryModel
                            {
                                AdmUsr_Id = ServiceKeys.AdmUsr_Id,
                                CPH_BuyPrice =(long) (notBuyPrice + statics.Nobitex.Not.Buy),
                                CPH_SellPrice =(long) (notBuyPrice + statics.Nobitex.Not.Sell),
                                CPH_CreateDate = DateTime.Now,
                                Cur_Id = await GetNotCur_Id()
                            }, token);
                        }
                    }

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

        private void FillTetherPrice()
        {
            if (responseRamzinex.Data.Any(o => o.BaseCurrencySymbol.EN == "usdt"))
            {
                tetherBuyPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "usdt").Buy) / 10;
                tetherSellPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "usdt").Sell) / 10;
                _logger.Information($"Tether Price: {tetherBuyPrice:N0}");
            }
        }

        private void FillTronPrice()
        {
            if (responseRamzinex.Data.Any(o => o.BaseCurrencySymbol.EN == "trx"))
            {
                tronBuyPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "trx").Buy) / 10;
                tronSellPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "trx").Sell) / 10;
                _logger.Information($"Tron Price: {tronBuyPrice:N0}");
            }
        }

        private void FillNotPrice()
        {
            if (responseRamzinex.Data.Any(o => o.BaseCurrencySymbol.EN == "not"))
            {
                notBuyPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "not").Buy) / 10;
                notSellPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "not").Sell) / 10;
                _logger.Information($"Not Price: {notBuyPrice:N0}");
            }
        }

        private void FillTonPrice()
        {
            if (responseRamzinex.Data.Any(o => o.BaseCurrencySymbol.EN == "toncoin"))
            {
                tonBuyPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "toncoin").Buy) / 10;
                tonSellPrice = ChangePriceType(responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "toncoin").Sell) / 10;
                _logger.Information($"TonCoin Price: {tonBuyPrice:N0}");
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

        private double ChangePriceType(string value)
        {
            double price = 0;
            double.TryParse(value, out price);
            return price;
        }
    }
}

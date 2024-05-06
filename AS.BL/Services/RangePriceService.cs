using AS.Log;
using AS.Model.Enums;
using AS.Model.Ex4Ir;
using AS.Model.General;
using AS.Model.IraniCard;
using AS.Model.Pay98;
using AS.Model.Ramzinex;
using AS.Model.TetherBank;
using AS.Model.Tetherland;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class RangePriceService : IRangePriceService
    {
        private readonly ILogger _logger;
        private readonly IEx4IrService _ex4IrService;
        private readonly IIraniCardService _iraniCardService;
        private readonly INobitexService _nobitexService;
        private readonly IPay98Service _pay98Service;
        private readonly IRamzinexService _ramzinexService;
        private readonly ITetherBankService _tetherBankService;
        private readonly ITetherlandService _tetherlandService;

        List<double> buyTetherPrices, sellTetherPrices;
        List<double> buyTronPrices, sellTronPrices;

        List<double> buyTetherPricesMid, sellTetherPricesMid;
        List<double> buyTronPricesMid, sellTronPricesMid;

        List<double> buyTetherPricesAvg, sellTetherPricesAvg;
        List<double> buyTronPricesAvg, sellTronPricesAvg;

        List<ResponseEx4IrModel> ex4IrResponse;
        CryptoPricesModel ex4IrPrice;

        ResponseIraniCardModel iraniCardResponse;
        CryptoPricesModel iraniCardPrice;

        double nobitexTetherAmount, nobitexTronAmount;
        CryptoPricesModel nobitexPrice;

        double responsePay98BuyTetherAmount, responsePay98SellTetherAmount, responsePay98BuyTronAmount, responsePay98SellTronAmount;
        CryptoPricesModel pay98Price;

        ResponseRamzinexModel responseRamzinex;
        CryptoPricesModel ramzinexPrice;

        ResponseTetherBankModel responseTetherBank;
        CryptoPricesModel tetherBankPrice;

        ResponseTetherlandModel responseTetherland;
        CryptoPricesModel tetherlandPrice;

        double buyTether, sellTether, buyTron, sellTron = 0;

        public RangePriceService(ILogger logger,
            IEx4IrService ex4IrService,
            IIraniCardService iraniCardService,
            INobitexService nobitexService,
            IPay98Service pay98Service,
            IRamzinexService ramzinexService,
            ITetherBankService tetherBankService,
            ITetherlandService tetherlandService)
        {
            _logger = logger;
            _ex4IrService = ex4IrService;
            _iraniCardService = iraniCardService;
            _nobitexService = nobitexService;
            _pay98Service = pay98Service;
            _ramzinexService = ramzinexService;
            _tetherBankService = tetherBankService;
            _tetherlandService = tetherlandService;
        }

        private void BuildPrices()
        {
            buyTetherPrices = new List<double>();
            sellTetherPrices = new List<double>();
            buyTronPrices = new List<double>();
            sellTronPrices = new List<double>();

            buyTetherPricesMid = new List<double>();
            sellTetherPricesMid = new List<double>();
            buyTronPricesMid = new List<double>();
            sellTronPricesMid = new List<double>();

            buyTetherPricesAvg = new List<double>();
            sellTetherPricesAvg = new List<double>();
            buyTronPricesAvg = new List<double>();
            sellTronPricesAvg = new List<double>();
        }

        private void PushPrices(CryptoPricesModel prices)
        {
            buyTetherPrices.Add(prices.BuyTether);
            sellTetherPrices.Add(prices.SellTether);
            buyTronPrices.Add(prices.BuyTron);
            sellTronPrices.Add(prices.SellTron);
        }

        public async Task<CryptoPricesModel> Get()
        {
            try
            {
                BuildPrices();

                _logger.Information("call Ex4Ir");
                ex4IrPrice = await GetEx4Ir();
                if (ex4IrPrice != null)
                {
                    _logger.Information($"Ex4Ir value is        {ex4IrPrice.ToString()}", ex4IrPrice);
                    PushPrices(ex4IrPrice);
                }
                else
                {
                    _logger.Error("ex4IrPrice is null");
                }

                _logger.Information("call IraniCard");
                iraniCardPrice = await GetIraniCard();
                if (iraniCardPrice != null)
                {
                    _logger.Information($"IraniCard value is    {iraniCardPrice.ToString()}", iraniCardPrice);
                    PushPrices(iraniCardPrice);
                }
                else
                {
                    _logger.Error("IraniCard is null");
                }

                _logger.Information("call Nobitex");
                nobitexPrice = await GetNobitex();
                if (nobitexPrice != null)
                {
                    _logger.Information($"Nobitex value is      {nobitexPrice.ToString()}", nobitexPrice);
                    PushPrices(nobitexPrice);
                }
                else
                {
                    _logger.Error("nobitexPrice is null");
                }

                _logger.Information("call Pay98");
                pay98Price = await GetPay98();
                if (pay98Price != null)
                {
                    _logger.Information($"Pay98 value is        {pay98Price.ToString()}", pay98Price);
                    PushPrices(pay98Price);
                }
                else
                {
                    _logger.Error("pay98Price is null");
                }

                _logger.Information("call TetherBank");
                tetherBankPrice = await GetTetherBank();
                if (tetherBankPrice != null)
                {
                    _logger.Information($"TetherBank value is   {tetherBankPrice.ToString()}", tetherBankPrice);
                    PushPrices(tetherBankPrice);
                }
                else
                {
                    _logger.Error("tetherBankPrice is null");
                }

                _logger.Information("call Tetherland");
                tetherlandPrice = await GetTetherland();
                if (tetherlandPrice != null)
                {
                    _logger.Information($"Tetherland value is   {tetherlandPrice.ToString()}", tetherlandPrice);
                    PushPrices(tetherlandPrice);
                }
                else
                {
                    _logger.Error("tetherlandPrice is null");
                }

                _logger.Information("call Ramzinex");
                ramzinexPrice = await GetRamzinex();
                if (ramzinexPrice != null)
                {
                    _logger.Information($"Ramzinex value is     {ramzinexPrice.ToString()}", ramzinexPrice);
                    PushPrices(ramzinexPrice);
                }
                else
                {
                    _logger.Error("ramzinexPrice is null");
                }

                SortMidPrices();
                PushMidPrices();
                GetMidPrices();
                PushAvgPrices();
                SortAvgPrices();

                #region در این بخش به عنوان مثال قیمت خرید تتر به دست آمده آن قیمت را با قیمت های چندین صرافی مقایسه می کنیم و اگر بیشتر از 300 باشد قیمت ثبت نمی شود
                #endregion
                var centerValue = GetCenterValueList(buyTetherPricesAvg);
                if ((Math.Max(buyTether, centerValue) - Math.Min(buyTether, centerValue)) > 600)
                {
                    return null;
                }

                centerValue = GetCenterValueList(sellTetherPricesAvg);
                if ((Math.Max(sellTether, centerValue) - Math.Min(sellTether, centerValue)) > 600)
                {
                    return null;
                }

                centerValue = GetCenterValueList(buyTronPricesAvg);
                if ((Math.Max(buyTron, centerValue) - Math.Min(buyTron, centerValue)) > 300)
                {
                    return null;
                }

                centerValue = GetCenterValueList(sellTronPricesAvg);
                if ((Math.Max(sellTron, centerValue) - Math.Min(sellTron, centerValue)) > 300)
                {
                    return null;
                }

                return new CryptoPricesModel
                {
                    BuyTether = buyTether,
                    SellTether = sellTether,
                    BuyTron = buyTron,
                    SellTron = sellTron
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<CryptoPricesModel> GetEx4Ir()
        {
            ex4IrResponse = await _ex4IrService.Get();
            if (ex4IrResponse is null)
            {
                return null;
            }

            if (ex4IrResponse.Count == 0)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = ex4IrResponse.FirstOrDefault(o => o.Symbol == "USDT").BuyPrice.ToDouble().RemoveDecimalNumber(),
                SellTether = ex4IrResponse.FirstOrDefault(o => o.Symbol == "USDT").SellPrice.ToDouble().RemoveDecimalNumber(),
                BuyTron = ex4IrResponse.FirstOrDefault(o => o.Symbol == "TRX").BuyPrice.ToDouble().RemoveDecimalNumber(),
                SellTron = ex4IrResponse.FirstOrDefault(o => o.Symbol == "TRX").SellPrice.ToDouble().RemoveDecimalNumber()
            };
        }

        public async Task<CryptoPricesModel> GetIraniCard()
        {
            iraniCardResponse = await _iraniCardService.Get();
            if (iraniCardResponse is null)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = iraniCardResponse.USDT.Buy.Price.RialToToman().RemoveDecimalNumber(),
                SellTether = iraniCardResponse.USDT.Sell.Price.RialToToman().RemoveDecimalNumber(),
                BuyTron = iraniCardResponse.TRX.Buy.Price.RialToToman().RemoveDecimalNumber(),
                SellTron = iraniCardResponse.TRX.Sell.Price.RialToToman().RemoveDecimalNumber()
            };
        }

        public async Task<CryptoPricesModel> GetNobitex()
        {
            nobitexTetherAmount = await _nobitexService.GetTetherAmount();
            nobitexTronAmount = await _nobitexService.GetTronAmount();

            if (nobitexTetherAmount == 0 || nobitexTronAmount == 0)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = nobitexTetherAmount.RemoveDecimalNumber(),
                SellTether = nobitexTetherAmount.RemoveDecimalNumber(),
                BuyTron = nobitexTronAmount.RemoveDecimalNumber(),
                SellTron = nobitexTronAmount.RemoveDecimalNumber()
            };
        }

        public async Task<CryptoPricesModel> GetPay98()
        {
            responsePay98BuyTetherAmount = await _pay98Service.GetTetherAmount(DealType.Buy);
            await Task.Delay(2000);
            responsePay98SellTetherAmount = await _pay98Service.GetTetherAmount(DealType.Sell);
            await Task.Delay(2000);
            responsePay98BuyTronAmount = await _pay98Service.GetTronAmount(DealType.Buy);
            await Task.Delay(2000);
            responsePay98SellTronAmount = await _pay98Service.GetTronAmount(DealType.Sell);

            if (responsePay98BuyTetherAmount == 0 || responsePay98SellTetherAmount == 0 ||
                responsePay98BuyTronAmount == 0 || responsePay98SellTronAmount == 0)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = responsePay98BuyTetherAmount.RemoveDecimalNumber(),
                SellTether = responsePay98SellTetherAmount.RemoveDecimalNumber(),
                BuyTron = responsePay98BuyTronAmount.RemoveDecimalNumber(),
                SellTron = responsePay98SellTronAmount.RemoveDecimalNumber()
            };
        }

        public async Task<CryptoPricesModel> GetRamzinex()
        {
            responseRamzinex = await _ramzinexService.Get();
            if (responseRamzinex is null)
            {
                return null;
            }
            if (responseRamzinex.Data.Count == 0)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "usdt").Buy.RialToToman().RemoveDecimalNumber(),
                SellTether = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "usdt").Sell.RialToToman().RemoveDecimalNumber(),
                BuyTron = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "trx").Buy.RialToToman().RemoveDecimalNumber(),
                SellTron = responseRamzinex.Data.FirstOrDefault(o => o.BaseCurrencySymbol.EN == "trx").Sell.RialToToman().RemoveDecimalNumber()
            };
        }

        public async Task<CryptoPricesModel> GetTetherBank()
        {
            responseTetherBank = await _tetherBankService.Get();
            if (responseTetherBank is null)
            {
                return null;
            }

            if (responseTetherBank.Currencies.Count == 0)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "USDT").TomanPrice.ToPrice().RemoveDecimalNumber(),
                SellTether = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "USDT").TomanPrice.ToPrice().RemoveDecimalNumber(),
                BuyTron = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "TRX").TomanPrice.ToPrice().RemoveDecimalNumber(),
                SellTron = responseTetherBank.Currencies.FirstOrDefault(o => o.Symbol == "TRX").TomanPrice.ToPrice().RemoveDecimalNumber()
            };
        }

        public async Task<CryptoPricesModel> GetTetherland()
        {
            responseTetherland = await _tetherlandService.Get();
            if (responseTetherland is null)
            {
                return null;
            }

            if (responseTetherland.Data.Count == 0)
            {
                return null;
            }

            return new CryptoPricesModel
            {
                BuyTether = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "USDT").TomanAmount.ToDouble().RemoveDecimalNumber(),
                SellTether = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "USDT").TomanAmount.ToDouble().RemoveDecimalNumber(),
                BuyTron = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "TRX").TomanAmount.ToDouble().RemoveDecimalNumber(),
                SellTron = responseTetherland.Data.FirstOrDefault(o => o.Symbol == "TRX").TomanAmount.ToDouble().RemoveDecimalNumber()
            };
        }

        private void SortMidPrices()
        {
            buyTetherPrices = buyTetherPrices.OrderByDescending(o => o).ToList();
            sellTetherPrices = sellTetherPrices.OrderByDescending(o => o).ToList();
            buyTronPrices = buyTronPrices.OrderByDescending(o => o).ToList();
            sellTronPrices = sellTronPrices.OrderByDescending(o => o).ToList();
        }

        private void SortAvgPrices()
        {
            buyTetherPricesAvg = buyTetherPricesAvg.OrderByDescending(o => o).ToList();
            sellTetherPricesAvg = sellTetherPricesAvg.OrderByDescending(o => o).ToList();
            buyTronPricesAvg = buyTronPricesAvg.OrderByDescending(o => o).ToList();
            sellTronPricesAvg = sellTronPricesAvg.OrderByDescending(o => o).ToList();
        }

        private void PushMidPrices()
        {
            try
            {
                buyTetherPricesMid = GetPricesValue(buyTetherPrices);
                buyTetherPricesMid.Remove(buyTetherPricesMid[0]);
                buyTetherPricesMid.Remove(buyTetherPricesMid[buyTetherPricesMid.Count - 1]);

                sellTetherPricesMid = GetPricesValue(sellTetherPrices);
                sellTetherPricesMid.Remove(sellTetherPricesMid[0]);
                sellTetherPricesMid.Remove(sellTetherPricesMid[sellTetherPricesMid.Count - 1]);

                buyTronPricesMid = GetPricesValue(buyTronPrices);
                buyTronPricesMid.Remove(buyTronPricesMid[0]);
                buyTronPricesMid.Remove(buyTronPricesMid[buyTronPricesMid.Count - 1]);

                sellTronPricesMid = GetPricesValue(sellTronPrices);
                sellTronPricesMid.Remove(sellTronPricesMid[0]);
                sellTronPricesMid.Remove(sellTronPricesMid[sellTronPricesMid.Count - 1]);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        //تست شود
        private void GetMidPrices()
        {
            if ((buyTetherPricesMid.Average() - sellTetherPricesMid.Average()) <= 150)
            {
                buyTether = buyTetherPricesMid.Average();
                sellTether = sellTetherPricesMid.Average();
            }
            else
            {
                buyTether = buyTetherPricesMid[buyTetherPricesMid.Count - 1];
                sellTether = sellTetherPricesMid[0];
            }

            if ((buyTronPricesMid.Average() - sellTronPricesMid.Average()) <= 150)
            {
                buyTron = buyTronPricesMid.Average();
                sellTron = sellTronPricesMid.Average();
            }
            else
            {
                buyTron = buyTronPricesMid[buyTronPricesMid.Count - 1];
                sellTron = sellTronPricesMid[0];
            }
        }

        private void PushAvgPrices()
        {
            if (nobitexPrice != null && iraniCardPrice != null)
            {
                buyTetherPricesAvg.Add((nobitexPrice.BuyTether + iraniCardPrice.BuyTether) / 2);
                sellTetherPricesAvg.Add((nobitexPrice.SellTether + iraniCardPrice.SellTether) / 2);
                buyTronPricesAvg.Add((nobitexPrice.BuyTron + iraniCardPrice.BuyTron) / 2);
                sellTronPricesAvg.Add((nobitexPrice.SellTron + iraniCardPrice.SellTron) / 2);
            }

            if (ex4IrPrice != null && tetherlandPrice != null)
            {
                buyTetherPricesAvg.Add((ex4IrPrice.BuyTether + tetherlandPrice.BuyTether) / 2);
                sellTetherPricesAvg.Add((ex4IrPrice.SellTether + tetherlandPrice.SellTether) / 2);
                buyTronPricesAvg.Add((ex4IrPrice.BuyTron + tetherlandPrice.BuyTron) / 2);
                sellTronPricesAvg.Add((ex4IrPrice.SellTron + tetherlandPrice.SellTron) / 2);
            }

            if (tetherBankPrice != null && pay98Price != null)
            {
                buyTetherPricesAvg.Add((tetherBankPrice.BuyTether + pay98Price.BuyTether) / 2);
                sellTetherPricesAvg.Add((tetherBankPrice.SellTether + pay98Price.SellTether) / 2);
                buyTronPricesAvg.Add((tetherBankPrice.BuyTron + pay98Price.BuyTron) / 2);
                sellTronPricesAvg.Add((tetherBankPrice.SellTron + pay98Price.SellTron) / 2);
            }
        }

        private double GetCenterValueList(List<double> arr)
        {
            var index = (int)(arr.Count / 2);
            return arr[index];
        }

        /// <summary>
        /// در این متد یک لیست را گرفته و دیتاهای آن را در لیست دیگر قرار می دهد
        /// اگر لیست را مستقیم در یک لیست دیگر قرار دهیم مشکل دیتا رفرنس به وجود می آید و در لیست دوم هرکاری انجام دهیم در لیست اول همان تغییرات اعمال می شود
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        private List<double> GetPricesValue(List<double> prices)
        {
            var result = new List<double>();
            foreach (var price in prices)
            {
                result.Add(price);
            }
            return result;
        }
    }
    public interface IRangePriceService
    {
        Task<CryptoPricesModel> Get();
        Task<CryptoPricesModel> GetEx4Ir();
        Task<CryptoPricesModel> GetIraniCard();
        Task<CryptoPricesModel> GetNobitex();
        Task<CryptoPricesModel> GetPay98();
        Task<CryptoPricesModel> GetRamzinex();
        Task<CryptoPricesModel> GetTetherBank();
        Task<CryptoPricesModel> GetTetherland();
    }
}

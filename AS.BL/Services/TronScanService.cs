using AS.Log;
using AS.Model.TronScan;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TronScanService : BaseApi, ITronScanService
    {
        private readonly ILogger _logger;
        public TronScanService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TronScanTrc20TokenTransferModel> GetLastTRC20(string walletAddress)
        {
            var response = await GetTRC20(walletAddress);
            if (response is null)
            {
                _logger.Error("response is null");
                return null;
            }

            if (response.TokenTransfers.Count == 0)
            {
                _logger.Error("response.TokenTransfers.Count == 0");
                return null;
            }

            return response.TokenTransfers.First();
        }

        public async Task<ResponseTronScanTrxDataModel> GetLastTRX(string walletAddress)
        {
            var response = await GetTRX(walletAddress);
            if (response is null)
            {
                _logger.Error("response is null");
                return null;
            }

            if (response.Data.Count == 0)
            {
                _logger.Error("response.Data.Count == 0");
                return null;
            }

            while (response.Data.First().Amount.DivisionBy6Zero() < 0.1)
            {
                response.Data.Remove(response.Data.First());
            }

            return response.Data.First();
        }

        public async Task<RessponseTronScanTrc20Model> GetTRC20(string walletAddress)
        {
            try
            {
                var response = await Get($"{TronScanUrl}api/token_trc20/transfers?limit=20&start=0&sort=-timestamp&count=true&toAddress={walletAddress}&relatedAddress={walletAddress}");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<RessponseTronScanTrc20Model>(await response.Content.ReadAsStringAsync());
                }
                _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<ResponseTronScanTrxModel> GetTRX(string walletAddress)
        {
            try
            {
                var response = await Get($"{TronScanUrl2}api/trx/transfer?sort=-timestamp&count=true&limit=20&start=0&toAddress={walletAddress}&relatedAddress={walletAddress}");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseTronScanTrxModel>(await response.Content.ReadAsStringAsync());
                }
                _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface ITronScanService
    {
        Task<RessponseTronScanTrc20Model> GetTRC20(string walletAddress);
        Task<TronScanTrc20TokenTransferModel> GetLastTRC20(string walletAddress);
        Task<ResponseTronScanTrxModel> GetTRX(string walletAddress);
        Task<ResponseTronScanTrxDataModel> GetLastTRX(string walletAddress);
    }
}

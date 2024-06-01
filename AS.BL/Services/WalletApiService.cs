using AS.Log;
using AS.Model.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class WalletApiService : BaseApi, IWalletApiService
    {
        private readonly ILogger _logger;
        public WalletApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<WalletModel> GetWalletById(int Wal_Id, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/Wallet/GetWalletById/{Wal_Id}", token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<WalletModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<bool> UpdateLastTransaction(int Wal_Id, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/Wallet/UpdateLastTransaction/{Wal_Id}",token);

                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }

    public interface IWalletApiService
    {
        Task<WalletModel> GetWalletById(int Wal_Id, string token);
        Task<bool> UpdateLastTransaction(int Wal_Id, string token);
    }
}

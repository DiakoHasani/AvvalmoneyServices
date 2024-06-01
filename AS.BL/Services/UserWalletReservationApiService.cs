using AS.Log;
using AS.Model.Enums;
using AS.Model.UserWalletReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class UserWalletReservationApiService : BaseApi, IUserWalletReservationApiService
    {
        private readonly ILogger _logger;
        public UserWalletReservationApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<UserWalletReservationModel>> GetUserWalletReservations(CurrencyType currencyType, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/UserWalletReservation/GetUserWalletReservations/{currencyType}", token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserWalletReservationModel>>(await response.Content.ReadAsStringAsync());
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

        public async Task<bool> UpdateTxid(int UWR_Id, string Txid, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/UserWalletReservation/UpdateTxid/{UWR_Id}/{Txid}", token);
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
    public interface IUserWalletReservationApiService
    {
        Task<List<UserWalletReservationModel>> GetUserWalletReservations(CurrencyType currencyType, string token);
        Task<bool> UpdateTxid(int UWR_Id, string Txid, string token);
    }
}

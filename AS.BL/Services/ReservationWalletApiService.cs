using AS.Log;
using AS.Model.Enums;
using AS.Model.ReservationWallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class ReservationWalletApiService : BaseApi, IReservationWalletApiService
    {
        private readonly ILogger _logger;
        public ReservationWalletApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> ApproveStatus(int Rw_Id, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/ReservationWallet/ApproveStatus/{Rw_Id}", token);
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

        public async Task<List<ReservationWalletModel>> GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/ReservationWallet/GetReservations?fromDate={fromDate}&toDate={toDate}&cryptoType={cryptoType}", token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReservationWalletModel>>(await response.Content.ReadAsStringAsync());
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


    }
    public interface IReservationWalletApiService
    {
        Task<List<ReservationWalletModel>> GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType, string token);
        Task<bool> ApproveStatus(int Rw_Id, string token);
    }
}

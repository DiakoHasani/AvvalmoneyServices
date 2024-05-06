using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
using AS.Model.UserWalletReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class UserWalletReservationService : IUserWalletReservationService
    {
        private readonly IUserWalletReservationRepository _userWalletReservationRepository;
        public UserWalletReservationService(IUserWalletReservationRepository userWalletReservationRepository)
        {
            _userWalletReservationRepository = userWalletReservationRepository;
        }

        public List<UserWalletReservation> GetUserWalletReservations(CurrencyType currencyType)
        {
            return _userWalletReservationRepository.GetAll(o => o.UWR_CryptoType == (int)currencyType)
                .OrderBy(o => o.UWR_Id).ToList();
        }

        public async Task<UserWalletReservation> Update(UserWalletReservation model)
        {
            _userWalletReservationRepository.Update(model);
            await _userWalletReservationRepository.SaveChangeAsync();
            return model;
        }
    }
    public interface IUserWalletReservationService
    {
        List<UserWalletReservation> GetUserWalletReservations(CurrencyType currencyType);
        Task<UserWalletReservation> Update(UserWalletReservation model);
    }
}

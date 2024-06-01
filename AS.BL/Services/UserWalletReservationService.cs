using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.UserWalletReservation;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public UserWalletReservationService(IUserWalletReservationRepository userWalletReservationRepository,
            IMapper mapper,
            ILogger logger)
        {
            _userWalletReservationRepository = userWalletReservationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public List<UserWalletReservationModel> GetUserWalletReservations(CurrencyType currencyType)
        {
            return _mapper.Map<List<UserWalletReservationModel>>(_userWalletReservationRepository.GetAll(o => o.UWR_CryptoType == (int)currencyType)
                .OrderBy(o => o.UWR_Id).ToList());
        }

        public async Task<UserWalletReservation> Update(UserWalletReservation model)
        {
            _userWalletReservationRepository.Update(model);
            await _userWalletReservationRepository.SaveChangeAsync();
            return model;
        }

        public async Task<bool> UpdateTxid(int UWR_Id, string Txid)
        {
            try
            {
                var userWalletReservation = await _userWalletReservationRepository.GetByIdAsync(UWR_Id);
                if (userWalletReservation is null)
                {
                    return false;
                }
                userWalletReservation.UWR_LastTxId= Txid;
                userWalletReservation.UWR_TransactionCount++;
                await Update(userWalletReservation);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
    public interface IUserWalletReservationService
    {
        List<UserWalletReservationModel> GetUserWalletReservations(CurrencyType currencyType);
        Task<UserWalletReservation> Update(UserWalletReservation model);
        Task<bool> UpdateTxid(int UWR_Id, string Txid);
    }
}

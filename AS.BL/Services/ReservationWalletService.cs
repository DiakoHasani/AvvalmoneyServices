using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
using AS.Model.ReservationWallet;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class ReservationWalletService : IReservationWalletService
    {
        private readonly IReservationWalletRepository _reservationWalletRepository;
        private readonly IMapper _mapper;
        public ReservationWalletService(IReservationWalletRepository reservationWalletRepository,
            IMapper mapper)
        {
            _reservationWalletRepository = reservationWalletRepository;
            _mapper = mapper;
        }

        public async Task<bool> ApproveStatus(int Rw_Id)
        {
            var reservationWallet = await _reservationWalletRepository.GetByIdAsync(Rw_Id);
            if(reservationWallet is null)
            {
                return false;
            }

            reservationWallet.RW_Status = true;
            _reservationWalletRepository.Update(reservationWallet);
            await _reservationWalletRepository.SaveChangeAsync();
            return true;
        }

        public List<ReservationWalletModel> GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType)
        {
            return _mapper.Map<List<ReservationWalletModel>>(_reservationWalletRepository.GetAll(o => o.RW_CreateDate >= fromDate &&
            o.RW_CreateDate <= toDate &&
            o.RW_Status == false &&
            o.CryptoType == (int)cryptoType).ToList());
        }

        public async Task<ReservationWallet> Update(ReservationWallet reservationWallet)
        {
            _reservationWalletRepository.Update(reservationWallet);
            await _reservationWalletRepository.SaveChangeAsync();
            return reservationWallet;
        }
    }
    public interface IReservationWalletService
    {
        List<ReservationWalletModel> GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType);
        Task<ReservationWallet> Update(ReservationWallet reservationWallet);
        Task<bool> ApproveStatus(int Rw_Id);
    }
}

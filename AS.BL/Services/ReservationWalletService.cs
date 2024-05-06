using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
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
        public ReservationWalletService(IReservationWalletRepository reservationWalletRepository)
        {
            _reservationWalletRepository = reservationWalletRepository;
        }

        public List<ReservationWallet> GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType)
        {
            return _reservationWalletRepository.GetAll(o => o.RW_CreateDate >= fromDate &&
            o.RW_CreateDate <= toDate &&
            o.RW_Status == false &&
            o.CryptoType == (int)cryptoType).ToList();
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
        List<ReservationWallet> GetReservations(DateTime fromDate, DateTime toDate, CryptoType cryptoType);
        Task<ReservationWallet> Update(ReservationWallet reservationWallet);
    }
}

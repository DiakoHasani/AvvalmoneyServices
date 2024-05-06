using AS.DAL;
using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class WalletService: IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Wallet> GetWalletById(int id)
        {
            return await _walletRepository.GetByIdAsync(id);
        }

        public async Task<Wallet> Update(Wallet wallet)
        {
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangeAsync();
            return wallet;
        }
    }
    public interface IWalletService
    {
        Task<Wallet> GetWalletById(int id);
        Task<Wallet> Update(Wallet wallet);
    }
}

using AS.DAL;
using AS.DAL.Services;
using AS.Model.Wallet;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public WalletService(IWalletRepository walletRepository,
            IMapper mapper)
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
        }

        public async Task<WalletModel> GetWalletById(int id)
        {
            return _mapper.Map<WalletModel>(await _walletRepository.GetByIdAsync(id));
        }

        public async Task<Wallet> Update(Wallet wallet)
        {
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangeAsync();
            return wallet;
        }

        public async Task<bool> UpdateLastTransaction(long Wal_Id)
        {
            var wallet =await _walletRepository.GetByIdAsync(Wal_Id);
            if(wallet is null)
                return false;

            wallet.LastTransaction = DateTime.Now;
            await _walletRepository.SaveChangeAsync();
            return true;
        }
    }
    public interface IWalletService
    {
        Task<WalletModel> GetWalletById(int id);
        Task<Wallet> Update(Wallet wallet);
        Task<bool> UpdateLastTransaction(long Wal_Id);
    }
}

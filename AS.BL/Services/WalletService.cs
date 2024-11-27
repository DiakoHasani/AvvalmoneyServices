using AS.DAL;
using AS.DAL.Services;
using AS.Model.Wallet;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
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

        public async Task<bool> UpdateLastTransaction(int Wal_Id)
        {
            var wallet =await _walletRepository.GetByIdAsync(Wal_Id);
            if(wallet is null)
                return false;

            wallet.LastTransaction = DateTime.Now;
            await _walletRepository.SaveChangeAsync();
            return true;
        }

        public async Task<bool> CheckWalletKey(int Wal_Id)
        {
            var wallet = await _walletRepository.GetByIdAsync(Wal_Id);

            if (string.IsNullOrWhiteSpace(wallet.Wal_Key))
            {
                return false;
            }
            var hash = ComputeSha256Hash(wallet.Address, wallet.Wal_Id);
            return wallet.Wal_Key.Equals(hash);
        }

        private string ComputeSha256Hash(string address, int Wal_Id)
        {
            string rawData = $"{address}_{Wal_Id}";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async Task<bool> ComputeWalletKey(int Wal_Id)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(Wal_Id);
                wallet.Wal_Key = ComputeSha256Hash(wallet.Address, wallet.Wal_Id);
                await _walletRepository.SaveChangeAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
    public interface IWalletService
    {
        Task<WalletModel> GetWalletById(int id);
        Task<Wallet> Update(Wallet wallet);
        Task<bool> UpdateLastTransaction(int Wal_Id);
        Task<bool> CheckWalletKey(int Wal_Id);
    }
}

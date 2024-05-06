using AS.DAL;
using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class BotWithdrawTypeService : IBotWithdrawTypeService
    {
        private readonly IBotWithdrawTypeRepository _botWithdrawTypeRepository;
        public BotWithdrawTypeService(IBotWithdrawTypeRepository botWithdrawTypeRepository)
        {
            _botWithdrawTypeRepository = botWithdrawTypeRepository;
        }

        public async Task Add(int botType, long withdrawId)
        {
            _botWithdrawTypeRepository.Add(new BotWithrawType
            {
                BotType = botType,
                CreateDate = DateTime.Now,
                Repeat = false,
                Wit_Id = withdrawId
            });
            await _botWithdrawTypeRepository.SaveChangeAsync();
        }

        public List<BotWithrawType> GetByWithdrawId(long withdrawId)
        {
            return _botWithdrawTypeRepository.GetAll(o => o.Wit_Id == withdrawId && !o.Repeat).OrderByDescending(o => o.Wit_Id).ToList();
        }
    }
    public interface IBotWithdrawTypeService
    {
        List<BotWithrawType> GetByWithdrawId(long withdrawId);
        Task Add(int botType, long withdrawId);
    }
}

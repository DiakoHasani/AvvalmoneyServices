using AS.DAL;
using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class BotInfoWithdrawService : IBotInfoWithdrawService
    {
        private readonly IBotInfoWithdrawRepository _botInfoWithdrawRepository;
        public BotInfoWithdrawService(IBotInfoWithdrawRepository botInfoWithdrawRepository)
        {
            _botInfoWithdrawRepository = botInfoWithdrawRepository;
        }

        public List<BotInfoWithraw> GetAll()
        {
            return _botInfoWithdrawRepository.GetAll(o => o.Active).OrderBy(o => o.Order).ToList();
        }

        public BotInfoWithraw GetByKey(Guid key)
        {
            return _botInfoWithdrawRepository.GetAll(o => o.Key == key).FirstOrDefault();
        }

        public async Task Update(BotInfoWithraw model)
        {
            _botInfoWithdrawRepository.Update(model);
            await _botInfoWithdrawRepository.SaveChangeAsync();
        }
    }
    public interface IBotInfoWithdrawService
    {
        List<BotInfoWithraw> GetAll();
        BotInfoWithraw GetByKey(Guid key);
        Task Update(BotInfoWithraw model);
    }
}

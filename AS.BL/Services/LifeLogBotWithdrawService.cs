using AS.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class LifeLogBotWithdrawService : ILifeLogBotWithdrawService
    {
        private readonly ILifeLogBotWithdrawRepository _lifeLogBotWithdrawRepository;
        public LifeLogBotWithdrawService(ILifeLogBotWithdrawRepository lifeLogBotWithdrawRepository)
        {
            _lifeLogBotWithdrawRepository = lifeLogBotWithdrawRepository;
        }

        public async Task Add(Guid botKey)
        {
            _lifeLogBotWithdrawRepository.Add(new DAL.LifeLogBotWithraw
            {
                BotKey = botKey.ToString(),
                CreateDate = DateTime.Now,
            });
            await _lifeLogBotWithdrawRepository.SaveChangeAsync();
        }
    }
    public interface ILifeLogBotWithdrawService
    {
        Task Add(Guid botKey);
    }
}

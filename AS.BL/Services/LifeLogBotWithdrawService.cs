using AS.BL.Catches;
using AS.DAL.Services;
using AS.Model.Enums;
using AS.Model.General;
using AS.Utility.Helpers;
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
        private readonly ISMSSenderService _smsSenderService;
        private readonly ILifeLogBotWithdrawCatch _lifeLogBotWithdrawCatch;
        public LifeLogBotWithdrawService(ILifeLogBotWithdrawRepository lifeLogBotWithdrawRepository,
            ISMSSenderService smsSenderService,
            ILifeLogBotWithdrawCatch lifeLogBotWithdrawCatch)
        {
            _lifeLogBotWithdrawRepository = lifeLogBotWithdrawRepository;
            _smsSenderService = smsSenderService;
            _lifeLogBotWithdrawCatch = lifeLogBotWithdrawCatch;
        }

        public async Task Add(string botKey)
        {
            _lifeLogBotWithdrawRepository.Add(new DAL.LifeLogBotWithraw
            {
                BotKey = botKey.ToString(),
                CreateDate = DateTime.Now,
            });
            await _lifeLogBotWithdrawRepository.SaveChangeAsync();
        }

        public bool CheckLife(string botKey)
        {
            var date = DateTime.Now.AddMinutes(ServiceKeys.LifeLogBotTime);
            return _lifeLogBotWithdrawRepository.GetAll(o => o.BotKey == botKey && o.CreateDate >= date).Any();
        }

        public List<BotType> CheckLifeAllBots()
        {
            var result = new List<BotType>();
            /*if (!CheckLife(ServiceKeys.BotSamanHabibiKey))
            {
                result.Add(BotType.Withdraw);
            }*/

            if (!CheckLife(ServiceKeys.WithdrawCryptoBotKey))
            {
                result.Add(BotType.WithdrawCrypto);
            }

            if (!CheckLife(ServiceKeys.UpdatePriceBotKey))
            {
                result.Add(BotType.UpdatePrice);
            }

            if (!CheckLife(ServiceKeys.CryptoGatewayKey))
            {
                result.Add(BotType.CryptoGateway);
            }

            if (!CheckLife(ServiceKeys.CryptoGatewayReservationKey))
            {
                result.Add(BotType.CryptoGatewayReservation);
            }

            if (!CheckLife(ServiceKeys.ContradictionBotKey))
            {
                result.Add(BotType.ContradictionBot);
            }

            if (result.Count > 0)
            {
                if (_lifeLogBotWithdrawCatch.AccessToSend())
                {
                    SendSms(result);
                }
            }


            return result;
        }

        private void SendSms(List<BotType> botTypes)
        {
            var message = "بات های زیر از کار افتاده اند \n";
            foreach (var item in botTypes)
            {
                message += item.GetDescription();
                message += "\n";
            }
            _smsSenderService.SendToSupports(message);
        }
    }
    public interface ILifeLogBotWithdrawService
    {
        Task Add(string botKey);
        bool CheckLife(string botKey);
        List<BotType> CheckLifeAllBots();
    }
}

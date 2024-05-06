using AS.DAL;
using AS.DAL.Services;
using AS.Model.General;
using AS.Model.WithdrawApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class OptBotWithdrawService : IOptBotWithdrawService
    {
        private readonly IOptBotWithdrawRepository _optBotWithdrawRepository;
        public OptBotWithdrawService(IOptBotWithdrawRepository optBotWithdrawRepository)
        {
            _optBotWithdrawRepository = optBotWithdrawRepository;
        }

        public async Task Add(PostOptRequestModel model)
        {
            _optBotWithdrawRepository.Add(new DAL.OptBotWithraw
            {
                Amount = model.Amount,
                CreateDate = DateTime.Now,
                MaskCardNumber = model.MaskCardNumber,
                OPT = model.OPT,
                Time = model.Time
            });
            await _optBotWithdrawRepository.SaveChangeAsync();
        }

        public OptBotWithraw GetLastOptByAmount(double amount)
        {
            var dt = ServiceKeys.GetOptDate;
            return _optBotWithdrawRepository.GetAll(o => o.Amount == amount && o.CreateDate > dt).OrderByDescending(o => o.Obw_Id).FirstOrDefault();
        }
    }
    public interface IOptBotWithdrawService
    {
        Task Add(PostOptRequestModel model);
        OptBotWithraw GetLastOptByAmount(double amount);
    }
}

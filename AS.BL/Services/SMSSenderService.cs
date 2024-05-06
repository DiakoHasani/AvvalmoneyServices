using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using Kavenegar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class SMSSenderService : ISMSSenderService
    {
        private readonly ILogger _logger;
        private readonly ISMSSenderRepository _smsSenderRepository;
        private readonly KavenegarApi _kavenegarApi;

        public SMSSenderService(ILogger logger,
            ISMSSenderRepository smsSenderRepository)
        {
            _logger = logger;
            _smsSenderRepository = smsSenderRepository;
            _kavenegarApi = new KavenegarApi(ServiceKeys.KavenegarApiKey);
        }

        private async Task<SMSSender> Add(SMSSender model)
        {
            _smsSenderRepository.Add(model);
            await _smsSenderRepository.SaveChangeAsync();
            return model;
        }

        private async Task<List<SMSSender>> AddRange(List<SMSSender> model)
        {
            _smsSenderRepository.AddRange(model);
            await _smsSenderRepository.SaveChangeAsync();
            return model;
        }

        public List<SMSSender> GetPendings()
        {
            var date = DateTime.Now.AddMinutes(-20);
            return _smsSenderRepository.GetAll(o => o.SMS_CreateDate > date &&
            (SMSSenderStatus)o.SMS_Status == SMSSenderStatus.Pending &&
            o.SMS_Repeat <= 4).ToList();
        }

        public async Task<SMSSender> Update(SMSSender model, bool result)
        {
            model.SMS_Repeat++;
            if (result)
            {
                model.SMS_Status = (int)SMSSenderStatus.Success;
            }
            else
            {
                if (model.SMS_Repeat > 4)
                {
                    model.SMS_Status = (int)SMSSenderStatus.Error;
                }
            }
            _smsSenderRepository.Update(model);
            await _smsSenderRepository.SaveChangeAsync();
            return model;
        }

        public bool Send(string reciverNumber, string text)
        {
            try
            {
                var sendResult = _kavenegarApi.Send(ServiceKeys.KavenegarNumber, reciverNumber,text);
                _logger.Information("status KavenegarApi.Send:" + sendResult.Status.ToString());
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }

        public bool SendToSupports(string text)
        {
            try
            {
                foreach (var number in ServiceKeys.SupportNumbers)
                {
                    try
                    {
                        var sendResult = _kavenegarApi.Send(ServiceKeys.KavenegarNumber, number, text);
                        _logger.Information("status KavenegarApi.Send:" + sendResult.Status.ToString());
                    }
                    catch (Exception ex) {
                        _logger.Error(ex.Message, ex);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return false;
            }
        }
    }
    public interface ISMSSenderService
    {
        bool Send(string reciverNumber, string text);
        List<SMSSender> GetPendings();
        Task<SMSSender> Update(SMSSender model, bool result);
        bool SendToSupports(string text);
    }
}

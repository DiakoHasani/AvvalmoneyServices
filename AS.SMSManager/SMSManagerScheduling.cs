using AS.BL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using Kavenegar;
using Kavenegar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.SMSManager
{
    public class SMSManagerScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ISMSSenderService _smsSenderService;

        private IPrint _print;

        KavenegarApi kavenegarApi;
        SendResult sendResult;
        public SMSManagerScheduling(ILogger logger,
            ISMSSenderService smsSenderService)
        {
            _logger = logger;
            _smsSenderService = smsSenderService;

            kavenegarApi = new KavenegarApi(ServiceKeys.KavenegarApiKey);
            Start(Run);
        }

        public void SetPrint(IPrint print)
        {
            _print = print;
        }

        private async Task Run()
        {
            try
            {
                Stop();
                var smsSenders = _smsSenderService.GetPendings();
                if (!smsSenders.Any())
                {
                    return;
                }
                _logger.Information("get PendingSms");

                foreach (var smsSender in smsSenders)
                {
                    try
                    {
                        sendResult = kavenegarApi.Send(ServiceKeys.KavenegarNumber, smsSender.SMS_Reciver, smsSender.SMS_Text);
                        _logger.Information("status KavenegarApi.Send:" + sendResult.Status.ToString());
                        await _smsSenderService.Update(smsSender, true);
                    }
                    catch (Kavenegar.Exceptions.ApiException ex)
                    {
                        _logger.Error(ex.Message, ex);
                        await _smsSenderService.Update(smsSender, false);
                    }
                    catch (Kavenegar.Exceptions.HttpException ex)
                    {
                        _logger.Error(ex.Message, ex);
                        await _smsSenderService.Update(smsSender, false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                _print.Show("_______________________________________________");
                Continue();
            }
        }
    }
}

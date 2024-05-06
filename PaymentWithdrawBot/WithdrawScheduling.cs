using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.PaymentWithdrawBot;
using AS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentWithdrawBot
{
    public class WithdrawScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ISMSSenderService _smsSenderService;
        private readonly IWithdrawApiService _withdrawApiService;

        private IPrint _print;
        private string token = "";
        private DateTime loginDate = DateTime.Now;
        private ResponseBotAvailableModel responseBotAvailable;
        bool appiumRun = false;

        public WithdrawScheduling(ILogger logger,
            ISMSSenderService smsSenderService,
            IWithdrawApiService withdrawApiService)
        {
            _logger = logger;
            _smsSenderService = smsSenderService;
            _withdrawApiService = withdrawApiService;

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
                if (!appiumRun)
                {
                    if (loginDate < DateTime.Now)
                    {
                        do
                        {
                            var login = await Login();
                            if (!string.IsNullOrEmpty(login))
                            {
                                token = login;
                                FileUtility.WriteTextToDataFile($"Bearer {token}", GetFilePath());
                            }
                            else
                            {
                                await Task.Delay(10000);
                            }
                        } while (string.IsNullOrWhiteSpace(token));
                    }

                    _logger.Information("call GetBotAvailable");
                    responseBotAvailable = await _withdrawApiService.GetBotAvailable(ServiceKeys.WithdrawKey, token);

                    if (responseBotAvailable != null)
                    {
                        System.Diagnostics.Process.Start(@"C:\Users\payam\Downloads\test\automation\appiumrun.bat");
                        appiumRun = true;
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

        private async Task<string> Login()
        {
            var message = await _withdrawApiService.Login(new RequestLoginModel
            {
                Fhlowk = ServiceKeys.WithdrawKey,
                UserName = ServiceKeys.WithdrawUserName,
                Password = ServiceKeys.WithdrawPassword,
            });

            if (message.Result)
            {
                _logger.Information(message.Message);
                loginDate = DateTime.Now.AddMinutes(ServiceKeys.WithdrawTimeLoginNumber - 3);
                return message.Token;
            }

            return null;
        }
        private string GetFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"data.txt";
        }
    }
}

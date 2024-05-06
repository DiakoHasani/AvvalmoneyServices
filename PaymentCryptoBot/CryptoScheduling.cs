using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.WithdrawCryptoBot;
using AS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentCryptoBot
{
    public class CryptoScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly ISMSSenderService _smsSenderService;
        private readonly IWithdrawCryptoApiService _withdrawCryptoApiService;

        private IPrint _print;

        public CryptoScheduling(ILogger logger,
            ISMSSenderService smsSenderService,
            IWithdrawCryptoApiService withdrawCryptoApiService)
        {
            _logger = logger;
            _smsSenderService = smsSenderService;
            _withdrawCryptoApiService = withdrawCryptoApiService;

            Start(Run);
        }

        private string token = "";
        private DateTime loginDate = DateTime.Now;
        bool appiumRun = false;

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
                    _logger.Information("call GetAvailable");
                    var responseAvailable =await _withdrawCryptoApiService.GetAvailable(ServiceKeys.WithdrawKey, token);
                    if (responseAvailable)
                    {
                        System.Diagnostics.Process.Start(@"C:\Users\batamani\Downloads\test\automation\appiumrun.bat");
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
            var message = await _withdrawCryptoApiService.Login(new RequestLoginModel
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

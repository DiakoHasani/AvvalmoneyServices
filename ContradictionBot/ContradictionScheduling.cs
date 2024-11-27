using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AS.Model.PaymentWithdrawBot;
using System.Threading.Tasks;
using AS.Model.Contradiction;

namespace ContradictionBot
{
    internal class ContradictionScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly IWithdrawApiService _withdrawApiService;
        private readonly IContradictionApiService _contradictionApiService;

        private IPrint _print;

        private string token = "";
        private DateTime loginDate = DateTime.Now;
        private List<ContradictionModel> checkResult;

        public ContradictionScheduling(ILogger logger,
            IWithdrawApiService withdrawApiService,
            IContradictionApiService contradictionApiService)
        {
            _logger = logger;
            _withdrawApiService = withdrawApiService;
            _contradictionApiService = contradictionApiService;

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

                if (loginDate < DateTime.Now)
                {
                    do
                    {
                        var login = await Login();
                        if (!string.IsNullOrEmpty(login))
                        {
                            token = login;
                        }
                        else
                        {
                            await Task.Delay(10000);
                        }
                    } while (string.IsNullOrWhiteSpace(token));
                }

                checkResult = await _contradictionApiService.Check(ServiceKeys.WithdrawKey, token);
                _logger.Information("call Check Api");

                if (checkResult is null)
                {
                    _logger.Error("checkResult is null");
                }
                else
                {
                    if (checkResult.Any())
                    {
                        _logger.Information($"change {checkResult.Count} transaction status", checkResult);
                    }
                    else
                    {
                        _logger.Information("checkResult is empty");
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
                loginDate = DateTime.Now.AddDays(ServiceKeys.WithdrawTimeLoginCryptoGatewayNumber);
                return message.Token;
            }

            return null;
        }
    }
}

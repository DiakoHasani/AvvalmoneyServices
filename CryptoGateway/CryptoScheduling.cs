using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.PaymentWithdrawBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGateway
{
    public class CryptoScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly IUSDT_TRC20Gateway _usdtTRC20Gateway;
        private readonly ITronGateway _tronGateway;
        private readonly IWithdrawApiService _withdrawApiService;
        private readonly ITonGateway _tonGateway;
        private readonly INotCoinGateway _notCoinGateway;

        private IPrint _print;

        private string token = "";
        private DateTime loginDate = DateTime.Now;
        public CryptoScheduling(ILogger logger,
            IUSDT_TRC20Gateway usdtTRC20Gateway,
            ITronGateway tronGateway,
            IWithdrawApiService withdrawApiService,
            ITonGateway tonGateway,
            INotCoinGateway notCoinGateway)
        {
            _logger = logger;
            _usdtTRC20Gateway = usdtTRC20Gateway;
            _tronGateway = tronGateway;
            _withdrawApiService = withdrawApiService;
            _tonGateway = tonGateway;
            _notCoinGateway = notCoinGateway;
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

                await _usdtTRC20Gateway.Call(token);
                _print.Show("___________");

                await _tronGateway.Call(token);
                _print.Show("___________");

                await _tonGateway.Call(token);
                _print.Show("___________");

                await _notCoinGateway.Call(token);
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

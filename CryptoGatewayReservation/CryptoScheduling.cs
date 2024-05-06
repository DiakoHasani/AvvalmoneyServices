using AS.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGatewayReservation
{
    public class CryptoScheduling : Scheduling
    {
        private readonly ILogger _logger;
        private readonly IUSDT_TRC20Gateway _usdtTRC20Gateway;
        private readonly ITronGateway _tronGateway;

        private IPrint _print;

        public CryptoScheduling(ILogger logger,
            IUSDT_TRC20Gateway usdtTRC20Gateway,
            ITronGateway tronGateway)
        {
            _logger = logger;
            _usdtTRC20Gateway = usdtTRC20Gateway;
            _tronGateway= tronGateway;
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
                await _usdtTRC20Gateway.Call();
                await _tronGateway.Call();
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

using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CryptoGateway
{
    public abstract class Scheduling
    {
        private readonly Timer _timer;
        private Func<Task> run;

        public Scheduling()
        {
            _timer = new Timer();
            ConfigScheduling();
        }

        private void ConfigScheduling()
        {
            _timer.Elapsed += new ElapsedEventHandler(Run);
            _timer.Interval = ServiceKeys.CryptoGatewayInterval;
        }

        public void Start(Func<Task> run)
        {
            this.run = run;
            Continue();
        }

        public void Continue()
        {
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }

        private async void Run(object source, ElapsedEventArgs e)
        {
            await run();
        }
    }
}

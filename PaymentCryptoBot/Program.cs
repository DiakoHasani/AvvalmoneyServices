using AS.BL;
using AS.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PaymentCryptoBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var container = new DependencyInjection().Config();
            await FirstDelay();
            new Manager(container);
            Console.ReadKey();
        }

        private static async Task FirstDelay()
        {
            for (int i = 1; i <= ServiceKeys.DelayRunCryptoBot; i++)
            {
                Console.Clear();
                Console.WriteLine(i);
                await Task.Delay(1000);
            }
            Console.Clear();
        }
    }

    public class Manager : IPrint
    {
        public Manager(UnityContainer container)
        {
            var smsScheduling = container.Resolve<CryptoScheduling>();
            smsScheduling.SetPrint(this);
        }
        public void Show(string text)
        {
            Console.WriteLine(text);
        }
    }
}

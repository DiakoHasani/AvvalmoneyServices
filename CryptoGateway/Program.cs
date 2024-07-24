using AS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace CryptoGateway
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = new DependencyInjection().Config();
            container.RegisterType<IUSDT_TRC20Gateway, USDT_TRC20Gateway>();
            container.RegisterType<ITronGateway, TronGateway>();
            container.RegisterType<ITonGateway, TonGateway>();
            container.RegisterType<INotCoinGateway, NotCoinGateway>();

            new Gateway(container);
            Console.ReadKey();
        }
    }

    public class Gateway : IPrint
    {
        public Gateway(UnityContainer container)
        {
            var cryptoScheduling = container.Resolve<CryptoScheduling>();
            cryptoScheduling.SetPrint(this);
        }
        public void Show(string text)
        {
            Console.WriteLine(text);
        }
    }
}

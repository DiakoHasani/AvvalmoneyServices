using AS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace AS.UpdatePrice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Web API configuration and services
            var container = new DependencyInjection().Config();
            container.RegisterType<IStatics, Statics>();
            new GetPrices(container);

            Console.ReadKey();
        }

        
    }

    class GetPrices : IPrint
    {
        public GetPrices(UnityContainer container)
        {
            var ramzinexScheduling = container.Resolve<RamzinexScheduling>();
            ramzinexScheduling.SetPrint(this);
            //var nobitexScheduling = container.Resolve<NobitexScheduling>();
            //nobitexScheduling.SetPrint(this);

            //var schedule = container.Resolve<RangeScheduling>();
            //schedule.SetPrint(this);
        }
        public void Show(string text)
        {
            Console.WriteLine(text);
        }
    }
}

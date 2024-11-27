using AS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ContradictionBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = new DependencyInjection().Config();

            new Manager(container);
            Console.ReadKey();
        }
    }

    public class Manager : IPrint
    {
        public Manager(UnityContainer container)
        {
            var contradictionScheduling = container.Resolve<ContradictionScheduling>();
            contradictionScheduling.SetPrint(this);
        }
        public void Show(string text)
        {
            Console.WriteLine(text);
        }
    }
}

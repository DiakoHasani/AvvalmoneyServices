using AS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace AS.SMSManager
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
            var smsScheduling = container.Resolve<SMSManagerScheduling>();
            smsScheduling.SetPrint(this);
        }
        public void Show(string text)
        {
            Console.WriteLine(text);
        }
    }
}

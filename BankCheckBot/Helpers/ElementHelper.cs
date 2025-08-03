using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Helpers
{
    internal static class ElementHelper
    {
        public static bool FillInput(this IWebElement element, string text)
        {
            try
            {
                foreach (var c in text)
                {
                    element.SendKeys(c.ToString());
                    Thread.Sleep(150);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

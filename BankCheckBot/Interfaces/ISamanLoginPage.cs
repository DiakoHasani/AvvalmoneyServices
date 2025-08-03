using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.Interfaces
{
    public interface ISamanLoginPage
    {
        bool Start(IWebDriver driver);
    }
}

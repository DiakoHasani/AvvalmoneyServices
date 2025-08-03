using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot
{
    internal class BaseOptions
    {
        protected IWebDriver GetWebDriver(string url)
        {
            ChromeOptions options = new ChromeOptions();

            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);

            // تنظیم User-Agent مشابه مرورگر واقعی
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");

            IWebDriver driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl(url);

            ((IJavaScriptExecutor)driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");
            return driver;
        }
    }
}

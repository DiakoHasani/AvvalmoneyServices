using BankCheckBot.Interfaces;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.SamanBank
{
    internal class SamanHomePage(ILogger<SamanHomePage> logger) : ISamanHomePage
    {
        public async Task<bool> StartAsync(IWebDriver driver)
        {
            try
            {
                await Task.Delay(10000);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var dashboardButton = wait.Until(d =>
    d.FindElement(By.XPath("//div[@class='menu-item']/a[contains(text(),'داشبورد')]")));
                dashboardButton.Click();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}

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
    internal class SamanUserCustomHomePage(ILogger<SamanUserCustomHomePage> logger) : ISamanUserCustomHomePage
    {
        const string _btnBill = "dmc-chart-section-deposit-bill-statement-link";
        public async Task<bool> StartAsync(IWebDriver driver)
        {
            try
            {
                await Task.Delay(10000);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                var billButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(_btnBill)));
                billButton.Click();

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

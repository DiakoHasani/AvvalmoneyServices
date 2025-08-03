using BankCheckBot.Interfaces;
using BankCheckBot.Models;
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
    internal class UserActivitiesHistoryPage(ILogger<UserActivitiesHistoryPage> logger) : IUserActivitiesHistoryPage
    {
        public async Task<bool> StartAsync(IWebDriver driver)
        {
            try
            {
                await Task.Delay(100);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                var parentDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.receipt-list.ng-star-inserted")));

                var cards = parentDiv.FindElements(By.TagName("nw-user-activity-card"));

                foreach (var card in cards)
                {
                    try
                    {
                        var date = card.FindElement(By.CssSelector("div.date")).Text.Trim();
                        var time = card.FindElement(By.CssSelector("div.time")).Text.Trim();
                        var amount = card.FindElement(By.CssSelector("span.data-space")).Text.Trim();

                        logger.LogInformation($"date: {date}");
                        logger.LogInformation($"time: {time}");
                        logger.LogInformation($"amount: {amount}");
                        logger.LogInformation($"_____________________________________");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message, ex);
                    }
                }

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

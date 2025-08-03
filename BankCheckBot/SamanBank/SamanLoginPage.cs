using BankCheckBot.Helpers;
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
    internal class SamanLoginPage(ILogger<SamanLoginPage> logger) : ISamanLoginPage
    {
        const string _userNameBoxId = "mat-input-0";
        const string _passwordBoxId = "mat-input-1";
        const string _btnLogin = "form-btn-0";
        const string _userName = "1390115";
        const string _password = "S32_dll1@@@@";
        //9271-800-1390115-1

        public bool Start(IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                var userNameBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(_userNameBoxId)));

                var passwordBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(_passwordBoxId)));

                userNameBox.FillInput(_userName);

                passwordBox.FillInput(_password);

                var loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(_btnLogin)));
                loginButton.Click();

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

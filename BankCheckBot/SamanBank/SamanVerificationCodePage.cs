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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankCheckBot.SamanBank
{
    internal class SamanVerificationCodePage(ILogger<SamanVerificationCodePage> logger,
        ISmsVerificationCodeService smsVerificationCodeService) : ISamanVerificationCodePage
    {
        private const string _verificationCodeBoxId = "mat-input-2";
        private const string _btnLogin = "form-btn-1";
        public async Task<bool> StartAsync(IWebDriver driver, string bearerToken)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var verificationCodeBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(_verificationCodeBoxId)));
                await Task.Delay(10000);
                var opt = await smsVerificationCodeService.GetOptAsync(bearerToken);
                if (string.IsNullOrWhiteSpace(opt))
                {
                    logger.LogError("opt is null");
                    return false;
                }
                Match match = Regex.Match(opt, @"رمز (\d+)");
                if (match.Success)
                {
                    opt = match.Groups[1].Value;
                }
                else
                {
                    logger.LogError("notfound opt code");
                    return false;
                }

                verificationCodeBox.FillInput(opt);

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
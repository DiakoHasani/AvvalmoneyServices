using BankCheckBot.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCheckBot.SamanBank
{
    internal class ManagementSamanBank(ILogger<ManagementSamanBank> logger,
        ISamanLoginPage samanLoginPage, ILoginService loginService,
        ISamanVerificationCodePage samanVerificationCodePage,
        ISamanHomePage samanHomePage,
        ISamanUserCustomHomePage samanUserCustomHomePage,
        ISamanBillStatementPage samanBillStatementPage) : BaseOptions, IManagementSamanBank
    {
        private const string _url = "https://ib.sb24.ir/webbank/index";

        private bool responseLoginPage = false;
        private bool responseVerificationCodePage = false;
        private bool responseHomePage = false;
        private bool responseUserActivitiesHistoryPage = false;
        private bool responseUserCustomHomePage = false;
        private bool responseSamanBillStatementPage = false;
        public async Task StartAsync()
        {
            try
            {
                var withdrawApiToken = await loginService.LoginAsync();

                var driver = GetWebDriver(_url);
                driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

                #region در اینجا عملیات صفحه لاگین را انجام میدهد
                responseLoginPage = await samanLoginPage.StartAsync(driver);
                if (!responseLoginPage)
                {
                    logger.LogError("responseLoginPage is false");
                    return;
                }
                #endregion

                #region در اینجا عملیات صفحه کدتایید انجام می شود
                responseVerificationCodePage = await samanVerificationCodePage.StartAsync(driver, withdrawApiToken);
                if (!responseVerificationCodePage)
                {
                    logger.LogError("responseVerificationCodePage is false");
                    return;
                }
                #endregion

                #region در اینجا عملیات صفحه هوم رو انجام میده
                responseHomePage = await samanHomePage.StartAsync(driver);
                if (!responseHomePage)
                {
                    logger.LogError("responseHomePage is false");
                    return;
                }
                #endregion

                #region در اینجا عملیات UserCustomeHome انجام می شود
                responseUserCustomHomePage = await samanUserCustomHomePage.StartAsync(driver);
                if (!responseUserCustomHomePage)
                {
                    logger.LogError("responseUserCustomHomePage is false");
                    return;
                }
                #endregion

                #region در اینجا عملیات صفحه billStatements انجام می شود
                responseSamanBillStatementPage = await samanBillStatementPage.StartAsync(driver, withdrawApiToken);
                if (!responseSamanBillStatementPage)
                {
                    logger.LogError("responseSamanBillStatementPage is false");
                    return;
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }
    }
}

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
    internal class SamanBillStatementPage(ILogger<SamanBillStatementPage> logger,
        ISamanBankApiService samanBankApiService) : ISamanBillStatementPage
    {
        const string _accountNumber = "9271-800-1390115-1";
        const string _btnSubmitId = "form-btn-25";
        public async Task<bool> StartAsync(IWebDriver driver, string token)
        {
            try
            {
                await Task.Delay(10000);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                #region در اینجا روی دراپ داون سپرده کلیک می کند
                var selectAccountBox = wait.Until(driver => driver.FindElement(By.CssSelector("[tsnfilteritem='depositNumber']")));
                selectAccountBox.Click();
                await Task.Delay(3000);
                var optionAccountBox = wait.Until(driver => driver.FindElement(By.XPath($"//mat-option//span[contains(text(),'{_accountNumber}')]")));
                optionAccountBox.Click();
                #endregion

                #region در اینجا روی دراپ داون نوع تراکنش کلیک می کند
                await Task.Delay(3000);
                var selectActionBox = wait.Until(driver => driver.FindElement(By.CssSelector("[tsnfilteritem='action']")));
                selectActionBox.Click();
                await Task.Delay(3000);
                var optionActionBox = wait.Until(driver => driver.FindElement(By.XPath($"//mat-option//span[contains(text(),'واریز')]")));
                optionActionBox.Click();
                #endregion

                var couter = 0;

                while (true)
                {
                    if (couter != 0)
                    {
                        await Task.Delay(60000);
                    }
                    couter++;

                    #region در اینجا روی دکمه جستجو کلیک می کند
                    await Task.Delay(3000);
                    var submit = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(_btnSubmitId)));
                    submit.Click();
                    #endregion

                    await Task.Delay(10000);

                    if (IsSessionExpiredModalVisible(driver))
                    {
                        logger.LogInformation("Session Is Expired");
                        return true;
                    }

                    var transactions = GetTransactions(wait);

                    var responseSamanBankApi = await samanBankApiService.BillStatement(transactions, token);
                    if (responseSamanBankApi is null)
                    {
                        logger.LogError("responseSamanBankApi is null");
                        return false;
                    }

                    foreach (var item in responseSamanBankApi)
                    {
                        if (item.Result)
                        {
                            logger.LogInformation($"Succefull this Transaction. Operation:{item.Operation} _ Amount:{item.Amount} _ Usr_Id:{item.Usr_Id}");
                        }
                        else
                        {
                            logger.LogError($"error this transaction. message: {item.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return false;
            }
        }

        private List<BillStatementTransactionModel> GetTransactions(WebDriverWait wait)
        {
            var rows = wait.Until(driver => driver.FindElements(By.CssSelector("#tableId tbody tr")));
            var transactions = new List<BillStatementTransactionModel>();

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.CssSelector("td"));
                var transaction = new BillStatementTransactionModel
                {
                    RowNumber = int.TryParse(cells[0].Text.Trim(), out var rowNumber) ? rowNumber : 0,
                    DateTime = cells[1].Text.Trim(),
                    Operation = cells[2].Text.Trim(),
                    PayId = cells[3].Text.Trim(),
                    StatementDescription = cells[4].Text.Trim(),
                    DebitCredit = cells[5].Text.Trim(),
                    Balance = cells[6].Text.Trim(),
                    ChequeNumber = cells[7].Text.Trim(),
                    ReferenceNumber = cells[8].Text.Trim(),
                    AgentBranch = cells[9].Text.Trim().Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "),
                    DocumentNumber = cells[10].Text.Trim()
                };

                transactions.Add(transaction);
            }
            return transactions;
        }

        private bool IsSessionExpiredModalVisible(IWebDriver driver)
        {
            try
            {
                var modal = driver.FindElement(By.XPath("//*[contains(text(), 'زمان شما به پایان رسید!')]"));
                return modal.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

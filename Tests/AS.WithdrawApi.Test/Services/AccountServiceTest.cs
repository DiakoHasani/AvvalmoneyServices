using AS.BL.Services;
using AS.Model.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WithdrawApi.Test.Services
{
    [TestClass]
    public class AccountServiceTest
    {
        private IAccountService _accountService;

        [TestInitialize]
        public void Init()
        {
            _accountService = new AccountService();
        }

        [TestMethod]
        public void WidthdrawLogin_ShouldMessage_IsValidIsTrue()
        {
            var message = _accountService.WidthdrawLogin(new Model.WithdrawApi.LoginRequestModel
            {
                fhlowk = ServiceKeys.WithdrawKey,
                UserName = ServiceKeys.WithdrawUserName,
                Password = ServiceKeys.WithdrawPassword
            });
            Assert.IsTrue(message.IsValid);
        }
    }
}

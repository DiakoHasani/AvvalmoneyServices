using AS.BL.Services;
using AS.Log;
using AS.Model.General;
using AS.Model.WithdrawApi;
using AS.WithdrawApi.Controllers;
using AS.WithdrawApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AS.WithdrawApi.Test.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private Mock<IAccountService> _acccountServiceMock;
        private Mock<ILogger> _loggerMock;
        private Mock<IRegisterJwtToken> _registerJwtToken;
        private AccountController _accountController;

        [TestInitialize]
        public void Init()
        {
            _acccountServiceMock = new Mock<IAccountService>();
            _loggerMock = new Mock<ILogger>();
            _registerJwtToken = new Mock<IRegisterJwtToken>();
            _accountController = new AccountController(_acccountServiceMock.Object, _loggerMock.Object, _registerJwtToken.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [TestMethod]
        public async Task Login_ShouldReturnHttpStatusCode_Created()
        {
            var model = new LoginRequestModel
            {
                fhlowk = ServiceKeys.WithdrawKey,
                UserName = ServiceKeys.WithdrawUserName,
                Password = ServiceKeys.WithdrawPassword
            };
            _acccountServiceMock.Setup(o => o.WidthdrawLogin(model)).Returns(new MessageModel
            {
                IsValid = true
            });
            _registerJwtToken.Setup(o => o.Register()).Returns("dfjloslf");

            var result = await _accountController.Login(model);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.Created);
        }
    }
}

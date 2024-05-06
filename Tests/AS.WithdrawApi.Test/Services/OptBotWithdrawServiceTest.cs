using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Model.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WithdrawApi.Test.Services
{
    [TestClass]
    public class OptBotWithdrawServiceTest
    {
        private Mock<IOptBotWithdrawRepository> _optBotWithdrawRepositoryMock;
        private IOptBotWithdrawService _optBotWithdrawService;
        OptBotWithraw optBotWithraw;

        [TestInitialize]
        public void Init()
        {
            _optBotWithdrawRepositoryMock = new Mock<IOptBotWithdrawRepository>();
            _optBotWithdrawService = new OptBotWithdrawService(_optBotWithdrawRepositoryMock.Object);
            optBotWithraw = new OptBotWithraw
            {
                Amount = 100000,
                CreateDate = DateTime.Now,
                OPT = "5050"
            };
        }

        [TestMethod]
        public void GetLastOptByAmount_ShouldReturnOptBotWithraw()
        {
            var data = new List<OptBotWithraw>
            {
                optBotWithraw
            }.AsQueryable();

            double amount = 100000;
            var dt = ServiceKeys.GetOptDate;

            _optBotWithdrawRepositoryMock.Setup(m => m.GetAll(o => o.Amount == amount && o.CreateDate > dt)).Returns(data);
            var result = _optBotWithdrawService.GetLastOptByAmount(amount);
            Assert.IsNotNull(result);
        }
    }
}

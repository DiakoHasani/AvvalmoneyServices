using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
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
    public class BotInfoWithdrawServiceTest
    {
        private Mock<IBotInfoWithdrawRepository> _botInfoWithdrawRepositoryMock;
        private IBotInfoWithdrawService _botInfoWithdrawService;
        BotInfoWithraw botInfoWithraw;

        [TestInitialize]
        public void Init()
        {
            _botInfoWithdrawRepositoryMock = new Mock<IBotInfoWithdrawRepository>();
            _botInfoWithdrawService = new BotInfoWithdrawService(_botInfoWithdrawRepositoryMock.Object);
            botInfoWithraw = new BotInfoWithraw
            {
                Biw_Id = 1,
                Active = true,
                BotName = "botSamanHabibi",
                Key = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e"),
                Order = 1,
                LastSeen = DateTime.Now,
            };
        }

        [TestMethod]
        public void GetAll_ShouldTheListIsFull()
        {
            var data = new List<BotInfoWithraw> { botInfoWithraw }.AsQueryable();
            _botInfoWithdrawRepositoryMock.Setup(o => o.GetAll(x => x.Active)).Returns(data);
            var result = _botInfoWithdrawService.GetAll();
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GetByKey_ShouldReturnBotInfoWithraw()
        {
            var key = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e");
            var data = new List<BotInfoWithraw> { botInfoWithraw }.AsQueryable();
            _botInfoWithdrawRepositoryMock.Setup(o => o.GetAll(x => x.Key == key)).Returns(data);
            var result =_botInfoWithdrawService.GetByKey(key);
            Assert.IsNotNull(result);
        }
    }
}

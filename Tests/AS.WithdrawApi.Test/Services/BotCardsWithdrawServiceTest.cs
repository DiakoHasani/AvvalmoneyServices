using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Log;
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
    public class BotCardsWithdrawServiceTest
    {
        private Mock<IBotCardsWithdrawRepository> _botCardsWithdrawRepositoryMock;
        private Mock<ILogger> _loggerMock;
        private Mock<IZibalService> _zibalService;
        private Mock<IBotInfoWithdrawService> _botInfoWithdrawServiceMock;
        private IBotCardsWithdrawService _botCardsWithdrawService;
        BotCardsWithraw botCardsWithraw;

        public BotCardsWithdrawServiceTest()
        {
            _botCardsWithdrawRepositoryMock = new Mock<IBotCardsWithdrawRepository>();
            _loggerMock = new Mock<ILogger>();
            _zibalService = new Mock<IZibalService>();
            _botInfoWithdrawServiceMock = new Mock<IBotInfoWithdrawService>();
            _botCardsWithdrawService = new BotCardsWithdrawService(_botCardsWithdrawRepositoryMock.Object,
                _loggerMock.Object,
                _zibalService.Object,
                _botInfoWithdrawServiceMock.Object);

            botCardsWithraw = new BotCardsWithraw
            {
                Bcw_Id = 1,
                BankName = "botSamanHabibi1",
                CardNumber = "6219861058812289",
                CVV2 = 853,
                Limit = 10000000,
                Index = "0",
                BankKey = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e"),
                Enabled = true,
                Order = 1,
                Shaba = "123",
                DateUpdate = DateTime.Now.AddDays(-1),
            };
        }

        [TestMethod]
        public void GetAvailableBotCard_ShouldReturnBotCardsWithraw()
        {
            var bankKey = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e");
            var data = new List<BotCardsWithraw>
            {
                botCardsWithraw
            }.AsQueryable();
            _botCardsWithdrawRepositoryMock.Setup(o => o.GetAll(x => x.BankKey == bankKey && x.Enabled)).Returns(data);

            var result = _botCardsWithdrawService.GetAvailableBotCard(200000, bankKey);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByIndex_ShouldReturnBotCardsWithraw()
        {
            var data = new List<BotCardsWithraw>
            {
                botCardsWithraw
            }.AsQueryable();
            var bankKey = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e");
            var index = "0";
            _botCardsWithdrawRepositoryMock.Setup(o => o.GetAll(x => x.Index == index && x.BankKey == bankKey)).Returns(data);

            var result = _botCardsWithdrawService.GetByIndex(index, bankKey);
            Assert.IsNotNull(result);
        }

    }
}

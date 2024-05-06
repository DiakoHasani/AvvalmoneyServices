using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.WithdrawApi;
using AS.WithdrawApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AS.WithdrawApi.Test.Controllers
{
    [TestClass]
    public class WithdrawControllerTest
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IUserWithdrawService> _userWithdrawServiceMock;
        private Mock<IUserBankCardService> _userBankCardServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IBotInfoWithdrawService> _botInfoWithdrawServiceMock;
        private Mock<IBotWithdrawTypeService> _botWithdrawTypeServiceMock;
        private Mock<IBotCardsWithdrawService> _botCardsWithdrawServiceMock;
        private Mock<ILifeLogBotWithdrawService> _lifeLogBotWithdrawServiceMock;
        private Mock<IOptBotWithdrawService> _optBotWithdrawServiceMock;
        private Mock<ISMSSenderService> _smsSenderServiceMock;
        private WithdrawController _withdrawController;

        private UserWithdraw userWithdraw;
        private UserBankCard userBankCard;
        private User user;
        private BotWithrawType botWithrawType;
        private BotInfoWithraw botInfoWithraw;
        private BotCardsWithraw botCardsWithraw;

        [TestInitialize]
        public void Init()
        {
            _loggerMock = new Mock<ILogger>();
            _userWithdrawServiceMock = new Mock<IUserWithdrawService>();
            _userBankCardServiceMock = new Mock<IUserBankCardService>();
            _userServiceMock = new Mock<IUserService>();
            _botInfoWithdrawServiceMock = new Mock<IBotInfoWithdrawService>();
            _botWithdrawTypeServiceMock = new Mock<IBotWithdrawTypeService>();
            _botCardsWithdrawServiceMock = new Mock<IBotCardsWithdrawService>();
            _lifeLogBotWithdrawServiceMock = new Mock<ILifeLogBotWithdrawService>();
            _optBotWithdrawServiceMock = new Mock<IOptBotWithdrawService>();
            _smsSenderServiceMock = new Mock<ISMSSenderService>();

            _withdrawController = new WithdrawController(_userWithdrawServiceMock.Object,
                _userBankCardServiceMock.Object, _userServiceMock.Object, _botInfoWithdrawServiceMock.Object,
                _botWithdrawTypeServiceMock.Object, _loggerMock.Object, _botCardsWithdrawServiceMock.Object,
                _lifeLogBotWithdrawServiceMock.Object, _optBotWithdrawServiceMock.Object, _smsSenderServiceMock.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            userWithdraw = new UserWithdraw()
            {
                AdmUsr_Id = 1,
                Aff_Id = 1,
                UBC_Id = 10,
                Usr_Id = 1,
                Wit_Amount = 100000,
                Wit_Id = 10,
                Wit_DateCreate = DateTime.Now,
                Wit_AutomatPayment = false,
            };

            userBankCard = new UserBankCard
            {
                Aff_Id = 1,
                UBC_Bank = 13,
                UBC_Id = 10,
                UBC_CardNumber = "6219861905226782",
                UBC_Status = (int)UserBankCardStatus.Accepted,
                UBC_CreateDate = DateTime.Now,
                Usr_Id = 1
            };

            user = new User
            {
                Aff_Id = 1,
                Usr_Id = 1,
                Usr_FullName = "دیاکو حسنی",
                Usr_Email = "diakohasani03@gmail.com",
                Usr_Password = "1234",
                Usr_Phone = "09180074693",
                Usr_Status = (int)UserStatus.Active,
                Usr_Verification = (int)UserVerification.Verified,
                Usr_BLNC_Balance = 4715050,
                Usr_NationalCode = "3720817601",
                Usr_MobileConfirm = true,
                Usr_MyRefralCode = 582197
            };

            botWithrawType = new BotWithrawType
            {
                Bwt_Id = 1,
                CreateDate = DateTime.Now,
                Wit_Id = 1,
                Repeat = false
            };

            botInfoWithraw = new BotInfoWithraw
            {
                Biw_Id = 1,
                Active = true,
                BotName = "botSamanHabibi",
                Key = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e"),
                Order = 1,
                LastSeen = DateTime.Now,
            };

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
        public async Task GetBotAvailable_ShouldReturnBotkey()
        {
            _userWithdrawServiceMock.Setup(m => m.GetLastWaitingWithdraw()).Returns(userWithdraw);
            _userWithdrawServiceMock.Setup(m => m.CheckWithdrawPayment(userWithdraw)).ReturnsAsync(CheckWithdrawPaymentStatus.ReadyWithdrawToPayment);
            _userBankCardServiceMock.Setup(m => m.GetbyIdAsync(userWithdraw.UBC_Id)).ReturnsAsync(userBankCard);
            _userServiceMock.Setup(m => m.GetByIdAsync(userWithdraw.Usr_Id)).ReturnsAsync(user);

            var withrawTypes = new List<BotWithrawType>
            {
                botWithrawType
            };

            _botWithdrawTypeServiceMock.Setup(m => m.GetByWithdrawId(userWithdraw.Wit_Id)).Returns(withrawTypes);
            _botCardsWithdrawServiceMock.Setup(m => m.UpdateLimites()).ReturnsAsync("");

            var bots = new List<BotInfoWithraw>
            {
                botInfoWithraw
            };
            _botInfoWithdrawServiceMock.Setup(m => m.GetAll()).Returns(bots);

            var cards = new List<BotCardsWithraw>
            {
                botCardsWithraw
            };
            _botCardsWithdrawServiceMock.Setup(m => m.GetByBankKey(Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e"))).Returns(cards);

            var response = await _withdrawController.GetBotAvailable(ServiceKeys.WithdrawKey);
            var botKey = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(botKey);
        }

        [TestMethod]
        public async Task GetLastWithraw_ShouldReturnLastWithrawResponseModel()
        {
            var key = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e");
            _botInfoWithdrawServiceMock.Setup(m => m.GetByKey(key)).Returns(botInfoWithraw);

            _userWithdrawServiceMock.Setup(m => m.GetLastWaitingWithdraw()).Returns(userWithdraw);
            _userWithdrawServiceMock.Setup(m => m.CheckWithdrawPayment(userWithdraw)).ReturnsAsync(CheckWithdrawPaymentStatus.ReadyWithdrawToPayment);
            _userBankCardServiceMock.Setup(m => m.GetbyIdAsync(userWithdraw.UBC_Id)).ReturnsAsync(userBankCard);
            _userServiceMock.Setup(m => m.GetByIdAsync(userWithdraw.Usr_Id)).ReturnsAsync(user);

            var response = await _withdrawController.GetLastWithraw(key, ServiceKeys.WithdrawKey);

            var content = await response.Content.ReadAsStringAsync();
            var lastWithrawResponseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<LastWithrawResponseModel>(content);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(lastWithrawResponseModel);
        }

        [TestMethod]
        public async Task GetBankCard_ShouldReturnBotCardsWithdrawResponseModel()
        {
            var withdrawId = 10;
            var key = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e");

            _userWithdrawServiceMock.Setup(m => m.GetByIdAsync(withdrawId)).ReturnsAsync(userWithdraw);
            _botCardsWithdrawServiceMock.Setup(m => m.UpdateLimites()).ReturnsAsync("");
            _botCardsWithdrawServiceMock.Setup(m => m.GetAvailableBotCard(userWithdraw.Wit_Amount, key)).Returns(botCardsWithraw);

            var response = await _withdrawController.GetBankCard(withdrawId, WithdrawType.Withraw, key, ServiceKeys.WithdrawKey);
            var content = await response.Content.ReadAsStringAsync();
            var botCardsWithdrawResponseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<BotCardsWithdrawResponseModel>(content);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(botCardsWithdrawResponseModel);
        }

        [TestMethod]
        public async Task PaymentWithraw_ShouldReturnTrue()
        {
            var withdrawId = 10;
            var index = "0";
            var key = Guid.Parse("5bb85cf4-39ac-4ee0-986a-c24bcbed095e");
            _botCardsWithdrawServiceMock.Setup(m => m.GetByIndex(index, key)).Returns(botCardsWithraw);
            _userWithdrawServiceMock.Setup(m=>m.GetByIdAsync(withdrawId)).ReturnsAsync(userWithdraw);
            var response = await _withdrawController.PaymentWithraw(withdrawId,index,PaymentWithdrawStatus.Accepted,WithdrawType.Withraw,key, ServiceKeys.WithdrawKey);
            var content = await response.Content.ReadAsStringAsync();
            var result= Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(content);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(result);
        }
    }
}

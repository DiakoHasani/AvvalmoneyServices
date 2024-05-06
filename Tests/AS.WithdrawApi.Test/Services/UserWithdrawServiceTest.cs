using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
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
    public class UserWithdrawServiceTest
    {
        private Mock<IUserWithdrawRepository> _userWithdrawRepositoryMock;
        private IUserWithdrawService _userWithdrawService;

        UserWithdraw userWithdraw;

        [TestInitialize]
        public void Init()
        {
            _userWithdrawRepositoryMock = new Mock<IUserWithdrawRepository>();
            _userWithdrawService = new UserWithdrawService(_userWithdrawRepositoryMock.Object);
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
        }

        [TestMethod]
        public void GetLastWaitingWithdraw_ShouldReturnWithdraw()
        {
            var data = new List<UserWithdraw> { userWithdraw }.AsQueryable();
            _userWithdrawRepositoryMock.Setup(x => x.GetAll(o => !o.Wit_Status.HasValue && o.Wit_DateCreate >= ServiceKeys.GetLastWaitingWithdrawTime &&
            !(o.SMS ?? false) && !(o.Bot ?? false))).Returns(data);

            Assert.IsNotNull(_userWithdrawService.GetLastWaitingWithdraw());
        }

        [TestMethod]
        public async Task CheckWithdrawPayment_ShouldReturnReadyWithdrawToPayment()
        {
            var result = await _userWithdrawService.CheckWithdrawPayment(userWithdraw);
            Assert.AreEqual(result, CheckWithdrawPaymentStatus.ReadyWithdrawToPayment);
        }

        [TestMethod]
        public async Task CheckWithdrawPayment_ShouldReturnErrorWithdrawTry()
        {
            userWithdraw.Try = ServiceKeys.MaximumTryWithdraw;
            var result = await _userWithdrawService.CheckWithdrawPayment(userWithdraw);
            Assert.AreEqual(result, CheckWithdrawPaymentStatus.ErrorWithdrawTry);
        }

        [TestMethod]
        public async Task CheckWithdrawPayment_ShouldReturnWithdrawAmountIsMoreMaximumCard()
        {
            userWithdraw.Wit_Amount = ServiceKeys.MaximumAmountCardTransfer;
            var result = await _userWithdrawService.CheckWithdrawPayment(userWithdraw);
            Assert.AreEqual(result, CheckWithdrawPaymentStatus.WithdrawAmountIsMoreMaximumCard);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnUserWithdraw()
        {
            _userWithdrawRepositoryMock.Setup(o => o.GetByIdAsync(10)).ReturnsAsync(userWithdraw);
            var result = await _userWithdrawService.GetByIdAsync(10);
            Assert.IsNotNull(result);
        }
    }
}

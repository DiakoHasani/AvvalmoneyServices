using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Model.Enums;
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
    public class UserBankCardServiceTest
    {
        private Mock<IUserBankCardRepository> _userBankCardRepositoryMock;
        private IUserBankCardService _userBankCardService;
        private UserBankCard userBankCard;

        [TestInitialize]
        public void Init()
        {
            _userBankCardRepositoryMock = new Mock<IUserBankCardRepository>();
            _userBankCardService = new UserBankCardService(_userBankCardRepositoryMock.Object);
            userBankCard = new UserBankCard
            {
                Aff_Id = 1,
                UBC_Bank = 13,
                UBC_Id = 10,
                UBC_CardNumber = "6219861905226782",
                UBC_Status=(int)UserBankCardStatus.Accepted,
                UBC_CreateDate= DateTime.Now,
                Usr_Id=1
            };
        }

        [TestMethod]
        public async Task GetbyIdAsync_ShouldReturnUserBankCard()
        {
            _userBankCardRepositoryMock.Setup(o => o.GetByIdAsync(10)).ReturnsAsync(userBankCard);
            var result = await _userBankCardService.GetbyIdAsync(10);
            Assert.IsNotNull(result);
        }
    }
}

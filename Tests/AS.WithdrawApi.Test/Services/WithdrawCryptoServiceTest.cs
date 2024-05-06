using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Log;
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
    public class WithdrawCryptoServiceTest
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IWithdrawCryptoRepository> _withdrawCryptoRepositoryMock;
        private IWithdrawCryptoService _withdrawCryptoService;

        private WithdrawCrypto withdrawCrypto;

        [TestInitialize]
        public void Init()
        {
            _loggerMock = new Mock<ILogger>();
            _withdrawCryptoRepositoryMock = new Mock<IWithdrawCryptoRepository>();
            _withdrawCryptoService = new WithdrawCryptoService(_loggerMock.Object, _withdrawCryptoRepositoryMock.Object);

            withdrawCrypto = new WithdrawCrypto
            {
                Drq_Id = Guid.Parse("ba04665b-4f4b-4cbd-a928-07a694360d26"),
                WC_Address = "TXbh9rEm1fYbBQR7wRaG6H7qzyxpQezWez",
                WC_Amount = 0.5,
                WC_CreateDate = DateTime.Now,
                WC_CryptoType = (int)CurrencyType.Tron,
                WC_Id = 4,
                WC_Status = (int)WithdrawCryptoStatus.Pending
            };
        }

        [TestMethod]
        public async Task GetById_ShouldReturnWithdrawCrypto()
        {
            _withdrawCryptoRepositoryMock.Setup(m => m.GetByIdAsync(4)).ReturnsAsync(withdrawCrypto);
            var result = await _withdrawCryptoService.GetById(4);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPendingWithdraw_ShouldReturnResponseWithdrawCryptoModel()
        {
            withdrawCrypto.WC_Status=(int)WithdrawCryptoStatus.PassToRobot;
            var data = new List<WithdrawCrypto>
            {
                withdrawCrypto
            }.AsQueryable();

            var date = DateTime.Now.AddMinutes(-20);
            _withdrawCryptoRepositoryMock.Setup(m => m.GetAll()).Returns(data);

            var result = _withdrawCryptoService.GetPendingWithdraw(WithdrawCryptoStatus.PassToRobot);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Update_ShouldReturnWithdrawCrypto()
        {
            withdrawCrypto.WC_Status = (int)WithdrawCryptoStatus.Success;
            var result=await _withdrawCryptoService.Update(withdrawCrypto);
            Assert.IsNotNull(result);
        }
    }
}

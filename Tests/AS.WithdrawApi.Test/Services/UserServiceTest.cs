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
    public class UserServiceTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private IUserService _userService;
        private User user;

        [TestInitialize]
        public void Init()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
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
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            _userRepositoryMock.Setup(o => o.GetByIdAsync(1)).ReturnsAsync(user);
            var result = await _userService.GetByIdAsync(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Update_ShouldReturnUser()
        {
            user.Usr_NationalCode = "9850817601";
            var result = await _userService.Update(user);
            Assert.IsNotNull(result);
        }
    }
}

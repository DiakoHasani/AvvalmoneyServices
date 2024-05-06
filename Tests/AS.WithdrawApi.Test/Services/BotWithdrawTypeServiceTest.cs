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
    public class BotWithdrawTypeServiceTest
    {
        private Mock<IBotWithdrawTypeRepository> _botWithdrawTypeRepositoryMock;
        private IBotWithdrawTypeService _botWithdrawTypeService;
        BotWithrawType botWithrawType;
        [TestInitialize]
        public void Init()
        {
            _botWithdrawTypeRepositoryMock = new Mock<IBotWithdrawTypeRepository>();
            _botWithdrawTypeService = new BotWithdrawTypeService(_botWithdrawTypeRepositoryMock.Object);
            botWithrawType = new BotWithrawType
            {
                Bwt_Id = 1,
                CreateDate = DateTime.Now,
                Wit_Id = 1,
                Repeat = false
            };
        }

        [TestMethod]
        public void GetByWithdrawId_ShouldAnyListIsTrue()
        {
            long withdrawId = 1;
            var data =new List<BotWithrawType>
            {
                botWithrawType
            }.AsQueryable();
            _botWithdrawTypeRepositoryMock.Setup(o => o.GetAll(x => x.Wit_Id == withdrawId && !x.Repeat)).Returns(data);
            var result = _botWithdrawTypeService.GetByWithdrawId(withdrawId);
            Assert.IsTrue(result.Any());
        }
    }
}

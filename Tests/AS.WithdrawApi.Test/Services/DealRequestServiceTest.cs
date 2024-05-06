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
    public class DealRequestServiceTest
    {
        private Mock<IDealRequestRepository> _dealRequestRepositoryMock;
        private IDealRequestService _dealRequestService;
        private Mock<ILogger> _logger;

        private DealRequest dealRequest;

        [TestInitialize]
        public void Init()
        {
            _dealRequestRepositoryMock = new Mock<IDealRequestRepository>();
            _logger = new Mock<ILogger>();
            _dealRequestService = new DealRequestService(_dealRequestRepositoryMock.Object, _logger.Object);

            dealRequest = new DealRequest
            {
                Drq_Id = new Guid("ba04665b-4f4b-4cbd-a928-07a694360d26"),
                Aff_Id = 1,
                AdmUsr_Id = 1,
                CPH_Id = 86204,
                Cur_Id = 50,
                Drq_Amount = 5,
                Drq_Cur_Latest_Price = 6990,
                Drq_TotalPrice = 34950,
                Usr_Id = 1,
                Drq_Type = (int)DealRequestType.BuyFromAM,
                Drq_Status = (int)DealRequestStatus.InProgress,
                Drq_VerificationType = (int)DealRequestVerificationType.Auto,
                Drq_VerificationStatus = (int)DealRequestVerificationStatus.Accepted,
                Drq_UsrWalletAddress = "TXbh9rEm1fYbBQR7wRaG6H7qzyxpQezWez",
                Txid = "asdfghjklqweiiirooedjksd1233445"
            };
        }

        [TestMethod]
        public void GetById_ShouldReturnDealRequest()
        {
            var data = new List<DealRequest>
            {
                dealRequest
            }.AsQueryable();
            var key = new Guid("ba04665b-4f4b-4cbd-a928-07a694360d26");
            _dealRequestRepositoryMock.Setup(m => m.GetAll(o => o.Drq_Id == key)).Returns(data);
            var result = _dealRequestService.GetById(key);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Update_ShouldReturnDealRequest()
        {
            dealRequest.Txid = "sdffgdfgdfg";
            var result = await _dealRequestService.Update(dealRequest);
            Assert.IsNotNull(result);
        }
    }
}

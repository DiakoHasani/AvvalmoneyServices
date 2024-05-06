using AS.BL.Services;
using AS.DAL;
using AS.Log;
using AS.Model.Enums;
using AS.Model.General;
using AS.Model.WithdrawCrypto;
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
    public class CryptoWithdrawControllerTest
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IWithdrawCryptoService> _withdrawCryptoServiceMock;
        private Mock<IDealRequestService> _dealRequestServiceMock;
        private CryptoWithdrawController _cryptoWithdrawController;

        private WithdrawCrypto withdrawCrypto;
        private DealRequest dealRequest;

        [TestInitialize]
        public void Init()
        {
            _loggerMock = new Mock<ILogger>();
            _withdrawCryptoServiceMock = new Mock<IWithdrawCryptoService>();
            _dealRequestServiceMock = new Mock<IDealRequestService>();

            _cryptoWithdrawController = new CryptoWithdrawController(_loggerMock.Object, _withdrawCryptoServiceMock.Object, _dealRequestServiceMock.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

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
        public async Task GetLast_ShouldReturnResponseWithdrawCryptoModel()
        {
            _withdrawCryptoServiceMock.Setup(m => m.GetPendingWithdraw(WithdrawCryptoStatus.PassToRobot)).Returns(new ResponseWithdrawCryptoModel
            {
                WC_Id = withdrawCrypto.WC_Id,
                WC_Address = withdrawCrypto.WC_Address,
                WC_Amount = withdrawCrypto.WC_Amount,
                WC_CryptoType = (CurrencyType)withdrawCrypto.WC_CryptoType
            });

            _withdrawCryptoServiceMock.Setup(m => m.GetById(withdrawCrypto.WC_Id)).ReturnsAsync(withdrawCrypto);

            var response = await _cryptoWithdrawController.GetLast(ServiceKeys.WithdrawKey);
            var content = await response.Content.ReadAsStringAsync();

            var responseWithdrawCryptoModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseWithdrawCryptoModel>(content);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(responseWithdrawCryptoModel);
        }

        [TestMethod]
        public async Task Pay_ShouldReturnHttpStatusCode_OK()
        {
            _withdrawCryptoServiceMock.Setup(m => m.GetById(4)).ReturnsAsync(withdrawCrypto);
            _dealRequestServiceMock.Setup(m => m.GetById(withdrawCrypto.Drq_Id)).Returns(dealRequest);

            var response = await _cryptoWithdrawController.Pay(new RequestPayWithdrawCryptoModel
            {
                fhlowk = ServiceKeys.WithdrawKey,
                sdgdfg = WithdrawCryptoStatus.Success,
                iooitr = 4,
                qwewr = "sdfosodifosdufoisdufoi"
            });

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}

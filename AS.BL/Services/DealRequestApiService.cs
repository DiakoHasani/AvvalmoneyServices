using AS.Log;
using AS.Model.DealRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class DealRequestApiService : BaseApi, IDealRequestApiService
    {
        private readonly ILogger _logger;
        public DealRequestApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<RequestDealModel> Add(RequestDealModel model, string token)
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                    { "Aff_Id", model.Aff_Id.ToString() },
                    { "CPH_Id", model.CPH_Id.ToString() },
                    { "Cur_Id", model.Cur_Id.ToString() },
                    { "Drq_Cur_Latest_Price", model.Drq_Cur_Latest_Price.ToString() },
                    { "Drq_Amount", model.Drq_Amount.ToString() },
                    { "Drq_TotalPrice", model.Drq_TotalPrice.ToString() },
                    { "Usr_Id", model.Usr_Id.ToString() },
                    { "Wal_Id", model.Wal_Id.ToString() },
                    { "Drq_Status", model.Drq_Status.ToString() },
                    { "Drq_Type", model.Drq_Type.ToString() },
                    { "Drq_VerificationStatus", model.Drq_VerificationStatus.ToString() },
                    { "Drq_VerificationType", model.Drq_VerificationType.ToString() },
                    { "Txid", model.Txid.ToString() },
                    { "Drq_CreateDate", model.Drq_CreateDate.ToString() },
                };

                var response = await Post($"{WithdrawApiUrl}api/DealRequest/Add", parameters, token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDealModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<DealRequestModel> DepositDealRequest(double amount, int walletId, double amountDifference, string token)
        {
            try
            {
                var response = await Get($"{WithdrawApiUrl}api/DealRequest/DepositDealRequest/{amount}/{walletId}/{amountDifference}", token);
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<DealRequestModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<DealRequestGatewayModel> UpdateGateway(DealRequestGatewayModel model, string token)
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                    { "Drq_Id", model.Drq_Id.ToString() },
                    { "Drq_Status", model.Drq_Status.ToString() },
                    { "Drq_Amount", model.Drq_Amount.ToString() },
                    { "Txid", model.Txid },
                };

                var response = await Post($"{WithdrawApiUrl}api/DealRequest/UpdateGateway", parameters, token);

                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<DealRequestGatewayModel>(await response.Content.ReadAsStringAsync());
                }

                _logger.Error(await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }
    }
    public interface IDealRequestApiService
    {
        Task<DealRequestModel> DepositDealRequest(double amount, int walletId, double amountDifference, string token);
        Task<DealRequestGatewayModel> UpdateGateway(DealRequestGatewayModel model, string token);
        Task<RequestDealModel> Add(RequestDealModel model, string token);
    }
}

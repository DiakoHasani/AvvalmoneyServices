using AS.Log;
using AS.Model.TronGrid;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TronGridServices : BaseApi, ITronGridServices
    {
        private readonly ILogger _logger;
        public TronGridServices(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ResponseTrc20TronGridModel> GetTrc20(string walletAddress)
        {
            try
            {
                var response = await Get(TronGridV1Url + $"accounts/{walletAddress}/transactions/trc20");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseTrc20TronGridModel>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

        public async Task<List<SummaryResponseTrxTronGridModel>> GetTrx(string walletAddress)
        {
            try
            {
                var response =await Get(TronGridV1Url+$"accounts/{walletAddress}/transactions?only_confirmed=true&limit=20");
                if (response.IsSuccessStatusCode)
                {
                    var result= Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseTrxTronGridModel>(await response.Content.ReadAsStringAsync());
                    var summaries = new List<SummaryResponseTrxTronGridModel>();

                    foreach(var item in result.Data)
                    {
                        var summary = new SummaryResponseTrxTronGridModel();
                        summary.Txid = item.TxID;

                        foreach(var property in item.RawData.MyProperty)
                        {
                            summary.Amount = property.Parameter.Value.Amount;
                            summary.ToAddress = property.Parameter.Value.ToAddress;
                            summary.OwnerAddress = property.Parameter.Value.OwnerAddress;
                        }

                        if (summary.Amount.DivisionBy6Zero()>0.1)
                        {
                            summaries.Add(summary);
                        }
                    }
                    return summaries;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }
    }
    public interface ITronGridServices
    {
        Task<ResponseTrc20TronGridModel> GetTrc20(string walletAddress);
        Task<List<SummaryResponseTrxTronGridModel>> GetTrx(string walletAddress);
    }
}

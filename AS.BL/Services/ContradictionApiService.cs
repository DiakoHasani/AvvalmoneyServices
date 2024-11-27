using AS.Log;
using AS.Model.Contradiction;
using AS.Model.PaymentWithdrawBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class ContradictionApiService: BaseApi, IContradictionApiService
    {
        private readonly ILogger _logger;
        public ContradictionApiService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<ContradictionModel>> Check(string fhlowk, string token)
        {
            var response = await Get($"{WithdrawApiUrl}api/Contradiction/Check/{fhlowk}", token);

            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
                {
                    return null;
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContradictionModel>>(await response.Content.ReadAsStringAsync());
            }
            _logger.Error("error in call Check", new { message = await response.Content.ReadAsStringAsync() });
            return null;
        }
    }
    public interface IContradictionApiService
    {
        Task<List<ContradictionModel>> Check(string fhlowk, string token);
    }
}

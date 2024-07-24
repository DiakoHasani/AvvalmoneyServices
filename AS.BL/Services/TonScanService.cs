using AS.Log;
using AS.Model.TonScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class TonScanService : BaseApi, ITonScanService
    {
        private readonly ILogger _logger;

        private const string TonTransferName = "TonTransfer";
        private const string JettonTransferName = "JettonTransfer";
        private const string OkStatus = "ok";
        private const string NotCoinSymbol = "NOT";

        private List<TonScanEventActionsModel> actions;
        private string address;
        private List<TonScanEventActionsTonTransferModel> tonTransfers;
        private List<TonScanEventActionsJettonTransferModel> jettonTransfers;

        public TonScanService(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task<TonScanModel> GetEvents(string walletAddress)
        {
            try
            {
                var response = await Get($"{TonScanUrl}accounts/{walletAddress}/events?limit=40");
                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TonScanModel>(await response.Content.ReadAsStringAsync());
                }
                _logger.Error("response.IsSuccessStatusCode is false", await response.Content.ReadAsStringAsync());
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public async Task<TonScanEventActionsTonTransferModel> GetLastTon(string walletAddress)
        {
            var response = await GetEvents(walletAddress);
            if (response is null)
            {
                return null;
            }

            if (response.Events.Count == 0)
            {
                _logger.Error("response.Events.Count == 0");
                return null;
            }

            actions = GetReceived(response);

            tonTransfers = GetTonTransfers(actions);

            return tonTransfers.FirstOrDefault();
        }

        private List<TonScanEventActionsModel> GetReceived(TonScanModel tonScan)
        {
            try
            {
                actions = new List<TonScanEventActionsModel>();
                address = tonScan.Events.FirstOrDefault().Account.Address;
                foreach (var even in tonScan.Events)
                {
                    foreach (var action in even.Actions)
                    {
                        try
                        {
                            if (action.Type == TonTransferName)
                            {
                                if (action.TonTransfer.Recipient.Address == address)
                                {
                                    action.TonTransfer.EventId = even.EventId;
                                    actions.Add(action);
                                }
                            }
                            else if (action.Type == JettonTransferName)
                            {
                                if (action.JettonTransfer.Recipient.Address == address)
                                {
                                    action.JettonTransfer.EventId = even.EventId;
                                    actions.Add(action);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message, ex);
                        }
                    }
                }
                return actions.Where(o => o.Status == OkStatus).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new List<TonScanEventActionsModel>();
            }
        }

        private List<TonScanEventActionsTonTransferModel> GetTonTransfers(List<TonScanEventActionsModel> actions)
        {
            tonTransfers = new List<TonScanEventActionsTonTransferModel>();
            foreach (var action in actions.Where(o => o.Type == TonTransferName))
            {
                tonTransfers.Add(action.TonTransfer);
            }
            return tonTransfers;
        }

        private List<TonScanEventActionsJettonTransferModel> GetJettonTransfers(List<TonScanEventActionsModel> actions)
        {
            jettonTransfers = new List<TonScanEventActionsJettonTransferModel>();
            foreach (var action in actions.Where(o => o.Type == JettonTransferName))
            {
                if (action.JettonTransfer.Jetton.Symbol== NotCoinSymbol)
                {
                    jettonTransfers.Add(action.JettonTransfer);
                }
            }
            return jettonTransfers;
        }

        public async Task<TonScanEventActionsJettonTransferModel> GetLastNotCoin(string walletAddress)
        {
            var response = await GetEvents(walletAddress);
            if (response is null)
            {
                return null;
            }

            if (response.Events.Count == 0)
            {
                _logger.Error("response.Events.Count == 0");
                return null;
            }

            actions = GetReceived(response);

            jettonTransfers = GetJettonTransfers(actions);

            return jettonTransfers.FirstOrDefault();
        }
    }

    public interface ITonScanService
    {
        Task<TonScanModel> GetEvents(string walletAddress);
        Task<TonScanEventActionsTonTransferModel> GetLastTon(string walletAddress);
        Task<TonScanEventActionsJettonTransferModel> GetLastNotCoin(string walletAddress);
    }
}

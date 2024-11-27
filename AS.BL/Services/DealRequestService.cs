using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL.Services
{
    public class DealRequestService : IDealRequestService
    {
        private readonly IDealRequestRepository _dealRequestRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        List<DealRequest> dealRequests;
        DateTime dateTime;

        public DealRequestService(IDealRequestRepository dealRequestRepository,
            ILogger logger, IMapper mapper)
        {
            _dealRequestRepository = dealRequestRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RequestDealModel> Add(RequestDealModel model)
        {
            try
            {
                var dealRequest = _mapper.Map<DealRequest>(model);
                dealRequest.Drq_Id = Guid.NewGuid();
                _dealRequestRepository.Add(dealRequest);

                await _dealRequestRepository.SaveChangeAsync();
                model.Drq_Id = dealRequest.Drq_Id;
                return model;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public DealRequestModel DepositDealRequest(double amount, int walletId, double amountDifference)
        {
            var dealRequest = DepositDealRequestStep1(amount, walletId);
            if (dealRequest != null)
            {
                return dealRequest;
            }

            dealRequest = DepositDealRequestStep2(amount, walletId, amountDifference);
            if (dealRequest != null)
            {
                return dealRequest;
            }

            dealRequest = DepositDealRequestStep3(amount, walletId, amountDifference);
            return dealRequest;
        }

        private DealRequestModel DepositDealRequestStep1(double amount, int walletId)
        {
            dateTime = DateTime.Now.AddMinutes(ServiceKeys.DepositTransactionTime);

            dealRequests = _dealRequestRepository.GetAll(o => o.Drq_Amount == amount &&
            o.Drq_Status == (int)DealRequestStatus.InProgress &&
            o.Wal_Id == walletId &&
            o.Drq_CreateDate >= dateTime).ToList();

            if (dealRequests is null || dealRequests.Count() == 0)
            {
                return null;
            }
            return _mapper.Map<DealRequestModel>(dealRequests.First());
        }

        private DealRequestModel DepositDealRequestStep2(double amount, int walletId, double amountDifference)
        {
            dateTime = DateTime.Now.AddMinutes(ServiceKeys.DepositTransactionTime);
            dealRequests = _dealRequestRepository.GetAll(o => o.Drq_CreateDate >= dateTime &&
            o.Wal_Id == walletId && o.Drq_Status == (int)DealRequestStatus.InProgress).ToList();

            if (dealRequests is null || dealRequests.Count() == 0)
            {
                return null;
            }

            if (dealRequests.Count == 1)
            {
                var dealRequest = dealRequests.First();
                var difference = Math.Abs(dealRequest.Drq_Amount - amount);
                if (difference <= amountDifference)
                {
                    return _mapper.Map<DealRequestModel>(dealRequest);
                }
            }
            return null;
        }

        private DealRequestModel DepositDealRequestStep3(double amount, int walletId, double amountDifference)
        {
            dateTime = DateTime.Now.AddMinutes(ServiceKeys.DepositTransactionTime);

            dealRequests = _dealRequestRepository.GetAll(o => o.Drq_CreateDate >= dateTime &&
            o.Wal_Id == walletId && o.Drq_Status == (int)DealRequestStatus.InProgress).ToList();

            if (dealRequests is null || dealRequests.Count() == 0)
            {
                return null;
            }

            var data = new List<Tuple<Guid, double>>();
            dealRequests.ForEach(o =>
            {
                data.Add(new Tuple<Guid, double>(o.Drq_Id, Math.Abs(o.Drq_Amount - amount)));
            });

            data = data.OrderBy(o => o.Item2).ToList();

            var dealRequest = dealRequests.FirstOrDefault(o => o.Drq_Id == data.FirstOrDefault().Item1);
            if (data.FirstOrDefault().Item2 <= amountDifference)
            {
                return _mapper.Map<DealRequestModel>(dealRequest);
            }

            return null;
        }

        public DealRequest GetById(Guid id)
        {
            return _dealRequestRepository.GetAll(o => o.Drq_Id == id).FirstOrDefault();
        }

        public async Task<DealRequestGatewayModel> UpdateGateway(DealRequestGatewayModel model)
        {
            var dealRequest = GetById(model.Drq_Id);
            if (dealRequest is null)
            {
                return null;
            }
            _mapper.Map(model, dealRequest);
            dealRequest.Drq_TotalPrice =(long) (model.Drq_Amount * dealRequest.Drq_Cur_Latest_Price);
            _dealRequestRepository.Update(dealRequest);
            await _dealRequestRepository.SaveChangeAsync();
            return _mapper.Map<DealRequestGatewayModel>(dealRequest);
        }

        public async Task<bool> UpdateTxid(Guid Drq_Id, string Txid)
        {
            try
            {
                var dealRequest = GetById(Drq_Id);
                if (dealRequest is null)
                    return false;

                dealRequest.Txid = Txid;
                _dealRequestRepository.Update(dealRequest);
                await _dealRequestRepository.SaveChangeAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    public interface IDealRequestService
    {
        DealRequestModel DepositDealRequest(double amount, int walletId, double amountDifference);
        Task<DealRequestGatewayModel> UpdateGateway(DealRequestGatewayModel model);
        DealRequest GetById(Guid id);
        Task<RequestDealModel> Add(RequestDealModel model);
        Task<bool> UpdateTxid(Guid Drq_Id, string Txid);
    }
}

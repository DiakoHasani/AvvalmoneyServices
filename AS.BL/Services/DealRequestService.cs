using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AS.Model.DealRequest;
using AS.Model.Enums;
using AS.Model.General;
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
        List<DealRequest> dealRequests;
        DateTime dateTime;

        public DealRequestService(IDealRequestRepository dealRequestRepository,
            ILogger logger)
        {
            _dealRequestRepository = dealRequestRepository;
            _logger= logger;
        }

        public async Task<DealRequest> Add(RequestDealModel model)
        {
            try
            {
                var dealRequest = new DealRequest
                {
                    Aff_Id = model.Aff_Id,
                    CPH_Id = model.CPH_Id,
                    Cur_Id = model.Cur_Id,
                    Drq_Cur_Latest_Price = model.Drq_Cur_Latest_Price,
                    Drq_Amount = model.Drq_Amount,
                    Drq_TotalPrice = model.Drq_TotalPrice,
                    Drq_CreateDate = DateTime.Now,
                    Drq_Id = Guid.NewGuid(),
                    Drq_Status = (int)model.DealRequestStatus,
                    Drq_Type = (int)model.DealRequestType,
                    Drq_VerificationStatus = (int)model.DealRequestVerificationStatus,
                    Drq_VerificationType = (int)model.DealRequestVerificationType,
                    Usr_Id = model.Usr_Id,
                    Wal_Id = model.Wal_Id,
                    Txid = model.Txid
                };
                _dealRequestRepository.Add(dealRequest);

                await _dealRequestRepository.SaveChangeAsync();
                return dealRequest;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        public DealRequest DepositDealRequest(double amount, int walletId)
        {
            dateTime = DateTime.Now.AddMinutes(ServiceKeys.DepositTransactionTime);

            dealRequests = _dealRequestRepository.GetAll().Where(o => o.Drq_Amount == amount &&
            o.Drq_Status == (int)DealRequestStatus.InProgress &&
            o.Wal_Id == walletId &&
            o.Drq_CreateDate >= dateTime).ToList();

            if (dealRequests is null || dealRequests.Count() == 0)
            {
                return null;
            }
            return dealRequests.First();
        }

        public DealRequest GetById(Guid id)
        {
            return _dealRequestRepository.GetAll(o => o.Drq_Id == id).FirstOrDefault();
        }

        public async Task<DealRequest> Update(DealRequest dealRequest)
        {
            _dealRequestRepository.Update(dealRequest);
            await _dealRequestRepository.SaveChangeAsync();
            return dealRequest;
        }
    }
    public interface IDealRequestService
    {
        DealRequest DepositDealRequest(double amount, int walletId);
        Task<DealRequest> Update(DealRequest dealRequest);
        DealRequest GetById(Guid id);
        Task<DealRequest> Add(RequestDealModel model);
    }
}

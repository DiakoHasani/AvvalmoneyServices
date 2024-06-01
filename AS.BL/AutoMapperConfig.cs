using AS.DAL;
using AS.Model.Currency;
using AS.Model.CurrencyPriceHistory;
using AS.Model.DealRequest;
using AS.Model.ReservationWallet;
using AS.Model.TransactionId;
using AS.Model.UserWalletReservation;
using AS.Model.Wallet;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.BL
{
    public class AutoMapperConfig
    {
        public IMapper Configure()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CurrencyPriceHistoryModel, CurrencyPriceHistory>();
                cfg.CreateMap<CurrencyPriceHistory, CurrencyPriceHistoryModel>();

                cfg.CreateMap<Currency, CurrencyModel>();
                cfg.CreateMap<CurrencyModel, Currency>();

                cfg.CreateMap<ReservationWallet, ReservationWalletModel>();
                cfg.CreateMap<ReservationWalletModel, ReservationWallet>();

                cfg.CreateMap<WalletModel, Wallet>();
                cfg.CreateMap<Wallet, WalletModel>();

                cfg.CreateMap<TransactionIdModel, TransactionId>();
                cfg.CreateMap<TransactionId, TransactionIdModel>();

                cfg.CreateMap<DealRequest, DealRequestModel>();
                cfg.CreateMap<DealRequestModel, DealRequest>();

                cfg.CreateMap<DealRequestGatewayModel, DealRequest>().ForMember(o => o.Drq_TotalPrice, o => o.Ignore())
                .ForMember(o => o.Usr_Id, o => o.Ignore());
                cfg.CreateMap<DealRequest, DealRequestGatewayModel>();

                cfg.CreateMap<DealRequestModel, DealRequestGatewayModel>();
                cfg.CreateMap<DealRequestGatewayModel, DealRequestModel>();

                cfg.CreateMap<UserWalletReservationModel, UserWalletReservation>();
                cfg.CreateMap<UserWalletReservation, UserWalletReservationModel>();

                cfg.CreateMap<RequestDealModel, DealRequest>();
                cfg.CreateMap<DealRequest, RequestDealModel>();
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}

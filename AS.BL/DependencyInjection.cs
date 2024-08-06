using AS.BL.Catches;
using AS.BL.Services;
using AS.DAL;
using AS.DAL.Services;
using AS.Log;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace AS.BL
{
    public class DependencyInjection
    {
        public UnityContainer Config()
        {
            var container = new UnityContainer();
            ConfigOthers(container);
            ConfigRepositories(container);
            ConfigCatch(container);
            ConfigBusiness(container);

            return container;
        }

        private void ConfigOthers(UnityContainer container)
        {
            container.RegisterType<IClient, Client>();
            container.RegisterType<ILogger, Logger>();

            var mapper = new AutoMapperConfig().Configure();
            container.RegisterInstance(mapper);
        }

        private void ConfigRepositories(UnityContainer container)
        {
            container.RegisterType<DataContext>();

            container.RegisterType<ISMSSenderRepository, SMSSenderRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserWithdrawRepository, UserWithdrawRepository>();
            container.RegisterType<IUserBankCardRepository, UserBankCardRepository>();
            container.RegisterType<IBotInfoWithdrawRepository, BotInfoWithdrawRepository>();
            container.RegisterType<IBotWithdrawTypeRepository, BotWithdrawTypeRepository>();
            container.RegisterType<IBotCardsWithdrawRepository, BotCardsWithdrawRepository>();
            container.RegisterType<ILifeLogBotWithdrawRepository, LifeLogBotWithdrawRepository>();
            container.RegisterType<IOptBotWithdrawRepository, OptBotWithdrawRepository>();
            container.RegisterType<ICurrencyPriceHistoryRepository, CurrencyPriceHistoryRepository>();
            container.RegisterType<ICurrencyRepository, CurrencyRepository>();
            container.RegisterType<IReservationWalletRepository, ReservationWalletRepository>();
            container.RegisterType<IWalletRepository, WalletRepository>();
            container.RegisterType<ITransactionIdRepository, TransactionIdRepository>();
            container.RegisterType<IDealRequestRepository, DealRequestRepository>();
            container.RegisterType<ITransactionRepository, TransactionRepository>();
            container.RegisterType<IMenuNotificationRepository, MenuNotificationRepository>();
            container.RegisterType<IWithdrawCryptoRepository, WithdrawCryptoRepository>();
            container.RegisterType<IUserWalletReservationRepository, UserWalletReservationRepository>();
        }

        private void ConfigBusiness(UnityContainer container)
        {
            container.RegisterType<ISMSSenderService, SMSSenderService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IUserWithdrawService, UserWithdrawService>();
            container.RegisterType<IUserBankCardService, UserBankCardService>();
            container.RegisterType<IBotInfoWithdrawService, BotInfoWithdrawService>();
            container.RegisterType<IBotWithdrawTypeService, BotWithdrawTypeService>();
            container.RegisterType<IBotCardsWithdrawService, BotCardsWithdrawService>();
            container.RegisterType<ILifeLogBotWithdrawService, LifeLogBotWithdrawService>();
            container.RegisterType<IOptBotWithdrawService, OptBotWithdrawService>();
            container.RegisterType<IZibalService, ZibalService>();
            container.RegisterType<INobitexService, NobitexService>();
            container.RegisterType<ICurrencyService, CurrencyService>();
            container.RegisterType<ICurrencyPriceHistoryService, CurrencyPriceHistoryService>();
            container.RegisterType<IReservationWalletService, ReservationWalletService>();
            container.RegisterType<IWalletService, WalletService>();
            container.RegisterType<ITransactionIdService, TransactionIdService>();
            container.RegisterType<IDealRequestService, DealRequestService>();
            container.RegisterType<ITransactionService, TransactionService>();
            container.RegisterType<IMenuNotificationService, MenuNotificationService>();
            container.RegisterType<IEx4IrService, Ex4IrService>();
            container.RegisterType<IIraniCardService, IraniCardService>();
            container.RegisterType<IRamzinexService, RamzinexService>();
            container.RegisterType<ITetherBankService, TetherBankService>();
            container.RegisterType<IPay98Service, Pay98Service>();
            container.RegisterType<ITetherlandService, TetherlandService>();
            container.RegisterType<IRangePriceService, RangePriceService>();
            container.RegisterType<ITronScanService, TronScanService>();
            container.RegisterType<IWithdrawCryptoService, WithdrawCryptoService>();
            container.RegisterType<IWithdrawApiService, WithdrawApiService>();
            container.RegisterType<IWithdrawCryptoApiService, WithdrawCryptoApiService>();
            container.RegisterType<IUserWalletReservationService, UserWalletReservationService>();
            container.RegisterType<ICurrencyApiService, CurrencyApiService>();
            container.RegisterType<ICurrencyPriceHistoryApiService, CurrencyPriceHistoryApiService>();
            container.RegisterType<IReservationWalletApiService, ReservationWalletApiService>();
            container.RegisterType<IWalletApiService, WalletApiService>();
            container.RegisterType<ITransactionIdApiService, TransactionIdApiService>();
            container.RegisterType<IDealRequestApiService, DealRequestApiService>();
            container.RegisterType<IUserWalletReservationApiService, UserWalletReservationApiService>();
            container.RegisterType<ITonScanService, TonScanService>();
            container.RegisterType<IAESServices, AESServices>();
            container.RegisterType<IPaystarService, PaystarService>();
        }

        private void ConfigCatch(UnityContainer container)
        {
            container.RegisterType<IPaystarCatch, PaystarCatch>();
        }

    }
}

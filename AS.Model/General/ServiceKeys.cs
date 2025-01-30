using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Model.General
{
    public class ServiceKeys
    {
        public static string WithdrawKey { get; } = "kshgug465SDFghjSDF43fjklsdfK09zvcxvvcb234TYUguyqwgsfWRTefdcxc122IOPfdlgabn33567MNOUFGHsdfjsifsjirtdfgQ";

        public static string WithdrawUserName { get; } = "dfgrKjYUsd56%pl)@sJJ3";
        public static string WithdrawPassword { get; } = "re[IOl;55)sdf!N236.$qadF9E";
        public static string WithdrawJwtSecretKey { get; } = "j!dieAK4^598_5532a#0lISisdJuP8)foM-MM@#$12";

        public static DateTime GetLastWaitingWithdrawTime { get; } = DateTime.Now.AddDays(-2);
        public static DateTime GetOptDate { get; } = DateTime.Now.AddMinutes(-5);
        public static double MaximumAmountCardTransfer { get; } = 10000000;
        public static int MaximumTryWithdraw { get; } = 3;
        public static int ChangeNumberBot { get; } = 2;
        public static int RepeatNumberRequest { get; } = 4;
        public static int ZibalId { get; } = 1496626;
        public static long MaximumLimitBankCard { get; } = 10000000;
        public static long AdmUsr_Id { get; } = 1;
        public static double Interval { get; } = 10000;//600000;
        public static double UpdatePriceInterval { get; } = 60000;
        public static double CryptoGatewayInterval { get; } = 30000;
        public static double ContradictionBotInterval { get; } = 60000;
        public static double PaymentWithdrawInterval { get; } = 20000;
        public static double PaymentCryptoInterval { get; } = 20000;
        public static double DelayRunCryptoBot { get; } = 60;
        public static double DepositTransactionTime { get; } = -20;
        public static string KavenegarApiKey { get; } = "57566D423532347844614D47442B6D666146325A6337737331574157305A65373844367467576A723971633D";
        public static string KavenegarNumber { get; } = "10009090900999";
        public static List<string> SupportNumbers { get; } = new List<string> { "09189799357", "09378456436" };
        public static DateTime WithdrawTimeLogin { get; } = DateTime.Now.AddMonths(1);
        public static string WithdrawIssuer { get; } = "http://localhost/";
        public static string AudienceSecret { get; } = "qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo";
        public static int WithdrawTimeLoginNumber { get; } = 60;
        public static int WithdrawTimeLoginUpdatePriceNumber { get; } = 1;
        public static int WithdrawTimeLoginCryptoGatewayNumber { get; } = 1;
        public static int DelayCryptoGateway { get; } = 5000;
        public static double AmountDifferenceTether { get; } = 5;
        public static double AmountDifferenceTron { get; } = 10;
        public static double AmountDifferenceTon { get; } = 3;
        public static double AmountDifferenceNotCoin { get; } = 10;
        public static double RestartCunterPaymentCryptoBot { get; } = 18;
        public static int ExpireCatchByMinute { get; } = 10;
        public static int ExpireCatchByMinuteLifeLogBot { get; } = 60;
        public static int ExpireCatchByMinuteCryptoWithdraw { get; } = 20;
        public static string GatewayEncriptionKey { get; } = "bGCw4YmGZNYp1VKw8Z/BYh2BtvlVtSpg";
        public static string PaystarKey { get; } = "gnk388d50kw3lq";
        public static string PaystarSignKey { get; } = "8617E135530BA16994437C2D9BD93B6B9B8B2FEF2A50D0BCDFCBCF60812E8968BBD47F776F30ECBD36A4D15F72BBD5303E65AE37E8278E21B7788A202D4D3245F3C79EAAA3DB725C950F11C2FFA93735A506A5D037AC538F3059A4266C6EABCD95288EC8C1AF29E4C5161390D3CD6860C87D9C5C20F9A63DF162280EF92A670B";
        public static string PanelAvvalMoneyUrl { get; } = "https://panel.avvalmoney.co/";
        public static string NovinpalKey { get; } = "eff7c2c9-3dd4-47c0-aae4-d4f8d6df3cae";
        public static string BotEncriptionKey { get; } = "u;a*hKDsv!bk658__haCVqeFL7%$hkaW";
        public static string BotEncriptionIv { get; } = "fkO=Tus%(aQ!mnK;";
        public static string UpdatePriceBotKey { get; } = "d94c26a6-1322-40c6-9084-991a5a163dd8";
        public static string BotSamanHabibiKey { get; } = "5bb85cf4-39ac-4ee0-986a-c24bcbed095e";
        public static string WithdrawCryptoBotKey { get; } = "13d80466-515b-4d1c-a550-053dcc8dfdbe";
        public static string CryptoGatewayKey { get; } = "f5487225-9716-4b37-b1de-de8b3790e670";
        public static string CryptoGatewayReservationKey { get; } = "932d9b29-8bcd-4bd3-b17f-c8e3a181eee7";
        public static string ContradictionBotKey { get; } = "4cc6e5e4-4daf-4c98-9e7d-811bc77416f6";
        public static int LifeLogBotTime { get; } = -20;
        public static string SepalKey { get; } = "bbb03ee37a36c03f619adedc61dbd8e6";
        public static string WebhookEncriptionKey { get; } = "fIPJHgga569@$sNMx1(LKLSJDSOIJFSSOIjIA";
        public static string ZarinpalMerchantId { get; } = "0544949e-06b3-429a-8212-0f01d7ccc4b9";
        public static string TronWallet { get; } = "TWM6qS3EQSA8FHZ874FSgo2L99yhBVheag";
    }
}

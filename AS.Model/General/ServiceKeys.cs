﻿using System;
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
        public static string GatewayEncriptionKey { get; } = "bGCw4YmGZNYp1VKw8Z/BYh2BtvlVtSpg";
        public static string PaystarKey { get; } = "gnk388d50kw3lq";
        public static string PaystarSignKey { get; } = "8617E135530BA16994437C2D9BD93B6B9B8B2FEF2A50D0BCDFCBCF60812E8968BBD47F776F30ECBD36A4D15F72BBD5303E65AE37E8278E21B7788A202D4D3245F3C79EAAA3DB725C950F11C2FFA93735A506A5D037AC538F3059A4266C6EABCD95288EC8C1AF29E4C5161390D3CD6860C87D9C5C20F9A63DF162280EF92A670B";
        public static string PanelAvvalMoneyUrl { get; } = "https://panel.avvalmoney.co/";
    }
}

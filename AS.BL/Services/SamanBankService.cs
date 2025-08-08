using AS.DAL.Services;
using AS.Log;
using AS.Model.Enums;
using AS.Model.SamanBank;
using AS.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Injection;

namespace AS.BL.Services
{
    public class SamanBankService : ISamanBankService
    {
        private readonly ILogger _logger;
        private readonly ISamanBankTransactionRepository _samanBankTransactionRepository;
        private readonly IUserBankCardRepository _userBankCardRepository;
        public SamanBankService(ILogger logger,
            ISamanBankTransactionRepository samanBankTransactionRepository,
            IUserBankCardRepository userBankCardRepository)
        {
            _logger = logger;
            _samanBankTransactionRepository = samanBankTransactionRepository;
            _userBankCardRepository = userBankCardRepository;
        }

        public async Task<List<ResponseBillStatementModel>> BillStatement(List<BillStatementTransactionModel> model)
        {
            var result = new List<ResponseBillStatementModel>();
            try
            {
                var samanBankTransactions = _samanBankTransactionRepository.GetAll().OrderByDescending(o => o.Sbt_Id).Skip(0).Take(50).ToList();
                foreach (var item in model)
                {
                    try
                    {
                        if (!samanBankTransactions.Any(o => o.Sbt_DocumentNumber.Trim() == item.DocumentNumber.Trim()))
                        {
                            var operationType = GetDetailPayment(item.Operation.Trim());

                            switch (operationType.Item1)
                            {
                                case DepositBankType.None:
                                    result.Add(new ResponseBillStatementModel
                                    {
                                        Result = false,
                                        Message = $"خطای در بخش کارت به کارت به وجود آمده \n {operationType.Item2}"
                                    });
                                    break;
                                case DepositBankType.Card:
                                    operationType.Item2 = operationType.Item2.GetEnglishNumber();
                                    var userBankCard = _userBankCardRepository.GetAll(o => o.UBC_CardNumber == operationType.Item2).FirstOrDefault();

                                    _samanBankTransactionRepository.Add(new DAL.SamanBankTransaction
                                    {
                                        Sbt_Amount = item.DebitCredit.Replace(",", "").GetEnglishNumber().ToInt64(),
                                        Sbt_Detail = item.Operation.Trim(),
                                        Sbt_DocumentNumber = item.DocumentNumber.Trim()
                                    });

                                    if (userBankCard != null)
                                    {
                                        result.Add(new ResponseBillStatementModel
                                        {
                                            Amount = item.DebitCredit.Replace(",", "").GetEnglishNumber().ToInt64(),
                                            Operation = operationType.Item2,
                                            Usr_Id = userBankCard.Usr_Id,
                                            Result = true
                                        });
                                    }
                                    else
                                    {
                                        result.Add(new ResponseBillStatementModel
                                        {
                                            Result = false,
                                            Message = $"شماره کارت این تراکنش در دیتابیس یافت نشد \n {operationType.Item2}"
                                        });
                                    }
                                    await _samanBankTransactionRepository.SaveChangeAsync();
                                    break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        private (DepositBankType, string) GetDetailPayment(string input)
        {
            var match = Regex.Match(input, @"از ک \d{16}");
            if (match.Success)
            {
                var cardNumberMatch = match = Regex.Match(match.Value, @"\b\d{16}\b");
                if (cardNumberMatch.Success)
                {
                    return (DepositBankType.Card, cardNumberMatch.Value.Trim());
                }
                return (DepositBankType.None, input);
            }

            match = Regex.Match(input, @"از شبا IR\d{24}");
            if (match.Success)
            {
                var shabaMatch = Regex.Match(match.Value, @"IR\d{24}");
                if (shabaMatch.Success)
                {
                    return (DepositBankType.Shaba, shabaMatch.Value.Trim());
                }
                return (DepositBankType.None, input);
            }

            match = Regex.Match(input, @"از حسابIR\d{24}");
            if (match.Success)
            {
                var hesabMatch = Regex.Match(match.Value, @"IR\d{24}");
                if (hesabMatch.Success)
                {
                    return (DepositBankType.Hesab, hesabMatch.Value.Trim());
                }
                return (DepositBankType.None, input);
            }

            match = Regex.Match(input, @"از سپرده \d{3}\.\d{3}\.\d{7}\.\d");
            if (match.Success)
            {
                var sepordehMatch = Regex.Match(match.Value, @"\b\d{3}\.\d{3}\.\d{7}\.\d\b");
                if (sepordehMatch.Success)
                {
                    return (DepositBankType.Sepordeh, sepordehMatch.Value.Trim());
                }
                return (DepositBankType.None, input);
            }

            return (DepositBankType.None, input);
        }
    }
    public interface ISamanBankService
    {
        Task<List<ResponseBillStatementModel>> BillStatement(List<BillStatementTransactionModel> model);
    }
}

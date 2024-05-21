using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Extensions;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Wallets;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Attributes;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Services;

namespace MS.Core.MM.Service
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(ILogger logger, IZeroOneApiService zeroOneApiService, IIncomeExpenseRepo incomeExpense, IDateTimeProvider dateTimeProvider) : base(logger)
        {
            ZeroOneApiService = zeroOneApiService;
            IncomeExpense = incomeExpense;
            DateTimeProvider = dateTimeProvider;
        }

        protected IZeroOneApiService ZeroOneApiService { get; }
        private IIncomeExpenseRepo IncomeExpense { get; }
        private IDateTimeProvider DateTimeProvider { get; }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<ResWalletInfo>> WalletInfo(ReqWalletInfo req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var zoUserInfo = await ZeroOneApiService.GetUserInfo(new ZOUserInfoReq(param.UserId)).GetReturnDataAsync();

                decimal freezeIncome = await IncomeExpense.GetFreezeIncome(param.UserId);

                decimal income = await IncomeExpense.GetMonthIncome(param.UserId, DateTimeProvider.Now);

                return new BaseReturnDataModel<ResWalletInfo>(ReturnCode.Success)
                {
                    DataModel = new ResWalletInfo
                    {
                        Amount = zoUserInfo.Amount.ToString(GlobalSettings.AmountFormat),
                        Point = zoUserInfo.Point.ToString(GlobalSettings.AmountFormat),
                        FreezeIncome = Math.Floor(freezeIncome).ToString(),
                        Income = Math.Floor(income).ToString(),
                    },
                };
            }, req);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<ResIncomeInfo>>> IncomeInfo(ReqIncomeInfo req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                DateTime startTime = param.Date.Date;
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);

                PageResultModel<MMIncomeExpenseModel> incomes =
                    await IncomeExpense.GetPageTransactionByFilter(new PageIncomeExpenseFilter
                    {
                        UserId = param.UserId,
                        TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Income },
                        StartTime = startTime,
                        EndTime = endTime,
                        Categories = param.PostType?.ConvertToIncomeExpenseCategory().ToEnumerable(),
                        Pagination = new PaginationModel
                        {
                            PageNo = param.PageNo,
                        },
                        PayType = IncomeExpensePayType.Amount,
                        Status = IncomeExpenseStatusEnum.Dispatched,
                    });

                string[] expenseIds = incomes.Data.Select(e => e.TargetId).Distinct().ToArray();

                Dictionary<string, MMIncomeExpenseModel> expenses = (await IncomeExpense.GetTransactionByFilter(new IncomeExpenseFilter
                {
                    Ids = expenseIds,
                    TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Expense }
                })).ToDictionary(e => e.Id, e => e);

                ResIncomeInfo[] result = incomes.Data.Select(e =>
                {
                    MMIncomeExpenseModel? expense = expenses.GetValueOrDefault(e.TargetId);
                    return new ResIncomeInfo
                    {
                        Id = e.Id,
                        Category = e.Category,
                        Amount = Math.Floor(e.Amount * e.Rebate).ToString(),
                        UserId = expense?.UserId ?? 0,
                        Title = $"{e.Category.GetDescription()}{e.Memo}",
                        PostTitle = e.Title,
                        TransactionTime = e.CreateTime.ToString(GlobalSettings.DateTimeFormat),
                        UnlockAmount = Math.Truncate(expense?.Amount ?? e.Amount).ToString()
                    };
                }).ToArray();

                return new BaseReturnDataModel<PageResultModel<ResIncomeInfo>>(ReturnCode.Success)
                {
                    DataModel = new PageResultModel<ResIncomeInfo>
                    {
                        Data = result,
                        PageNo = incomes.PageNo,
                        PageSize = incomes.PageSize,
                        TotalPage = incomes.TotalPage,
                    }
                };
            }, req);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<ResExpenseInfo>>> ExpenseInfo(ReqExpenseInfo req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                DateTime startTime = req.Date.Date;
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);

                PageResultModel<MMIncomeExpenseModel> incomes =
                    await IncomeExpense.GetPageTransactionByFilter(new PageIncomeExpenseFilter
                    {
                        UserId = param.UserId,
                        TransactionTypes = new List<IncomeExpenseTransactionTypeEnum>
                        {
                            IncomeExpenseTransactionTypeEnum.Expense,
                            IncomeExpenseTransactionTypeEnum.Refund
                        },
                        StartTime = startTime,
                        EndTime = endTime,
                        Pagination = new PaginationModel
                        {
                            PageNo = req.PageNo,
                        },
                        PayType = IncomeExpensePayType.Point,
                    });

                ResExpenseInfo[] result = incomes.Data.Select(expense => new ResExpenseInfo
                {
                    Amount = Math.Floor(expense.Amount * expense.Rebate).ToString(),
                    Title = GenerateExpenseTitle(expense),
                    TransactionTime = expense.CreateTime.ToString(GlobalSettings.DateTimeFormat),
                    Category = expense.Category,
                    TransactionType = expense.TransactionType
                }).ToArray();

                return new BaseReturnDataModel<PageResultModel<ResExpenseInfo>>(ReturnCode.Success)
                {
                    DataModel = new PageResultModel<ResExpenseInfo>
                    {
                        Data = result,
                        PageNo = incomes.PageNo,
                        PageSize = incomes.PageSize,
                        TotalPage = incomes.TotalPage,
                    }
                };
            }, req);
        }

        private static string GenerateExpenseTitle(MMIncomeExpenseModel e)
        {
            switch (e.Category)
            {
                case IncomeExpenseCategoryEnum.Official:
                    if (e.TransactionType == IncomeExpenseTransactionTypeEnum.Expense)
                    {
                        return $"{e.Category.GetDescription<ExpenseDescriptionAttribute>()}{e.Memo}";
                    }
                    else
                    {
                        return "官方帖预约退费";
                    }
                case IncomeExpenseCategoryEnum.Square:
                    if (e.TransactionType == IncomeExpenseTransactionTypeEnum.Expense)
                    {
                        return e.Category.GetDescription<ExpenseDescriptionAttribute>();
                    }
                    else
                    {
                        return "广场帖解锁退费";
                    }
                case IncomeExpenseCategoryEnum.Agency:
                    if (e.TransactionType == IncomeExpenseTransactionTypeEnum.Expense)
                    {
                        return e.Category.GetDescription<ExpenseDescriptionAttribute>();
                    }
                    else
                    {
                        return "寻芳阁帖解锁退费";
                    }
                default:
                    return e.Category.GetDescription<ExpenseDescriptionAttribute>();
            }
        }

        public async Task<BaseReturnDataModel<decimal>> GetTodayTotalIncomeExpense(ReqTodayTotalIncomeExpense req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                DateTime startTime = req.Date.Date;
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);

                decimal amount =
                    await IncomeExpense.GetSumTransactionByFilter(new PageIncomeExpenseFilter
                    {
                        UserId = param.UserId,
                        TransactionTypes = req.TransactionTypes,
                        StartTime = startTime,
                        EndTime = endTime,
                        PayType = req.PayType
                    });

                return new BaseReturnDataModel<decimal>(ReturnCode.Success)
                {
                    DataModel = amount
                };
            }, req);
        }

        public async Task<BaseReturnDataModel<decimal>> GetSumGetIncomeByFilter(ReqTodayTotalIncomeExpense req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                DateTime startTime = req.Date.Date;
                DateTime endTime = startTime.AddDays(1).AddMilliseconds(-1);

                decimal amount =
                    await IncomeExpense.GetSumGetIncomeByFilter(new PageIncomeExpenseFilter
                    {
                        UserId = param.UserId,
                        TransactionTypes = req.TransactionTypes,
                        StartTime = startTime,
                        EndTime = endTime,
                        PayType = req.PayType
                    });

                return new BaseReturnDataModel<decimal>(ReturnCode.Success)
                {
                    DataModel = amount
                };
            }, req);
        }
    }
}
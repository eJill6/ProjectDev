using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.Extensions;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Wallets;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Wallet;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 錢包相關
    /// </summary>
    public class WalletController : ApiControllerBase
    {
        /// <summary>
        /// 錢包相關
        /// </summary>
        /// <param name="logger">log</param>
        public WalletController(ILogger<WalletController> logger, IWalletService walletService, IMapper mapper) : base(logger)
        {
            WalletService = walletService;
            Mapper = mapper;
        }

        private IWalletService WalletService { get; }
        private IMapper Mapper { get; }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<WalletInfoViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> WalletInfo()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                int userId = User.GetUserId();

                ReqWalletInfo req = new ReqWalletInfo
                {
                    UserId = userId,
                };

                var res = await WalletService.WalletInfo(req);

                if (res.IsSuccess == false)
                {
                    return new BaseReturnDataModel<WalletInfoViewModel>(res);
                }

                var result = Mapper.Map<WalletInfoViewModel>(res.DataModel);

                return new BaseReturnDataModel<WalletInfoViewModel>(ReturnCode.Success)
                {
                    DataModel = result,
                };
            }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<IncomeSummaryViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IncomeInfo(IncomeInfoData query)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                int userId = User.GetUserId();

                ReqIncomeInfo req = new ReqIncomeInfo
                {
                    UserId = userId,
                    Date = query.Date,
                    PageNo = query.PageNo ?? query.Page ?? 1,
                    PostType = (PostType?)query.PostType
                };

                var res = await WalletService.IncomeInfo(req);

                if (res.IsSuccess == false)
                {
                    return new BaseReturnDataModel<IncomeSummaryViewModel>(res);
                }

                var result = Mapper.Map<IncomeSummaryViewModel>(res.DataModel);

                ReqTodayTotalIncomeExpense totalReq = new ReqTodayTotalIncomeExpense
                {
                    UserId = userId,
                    Date = query.Date,
                    PostType = (PostType?)query.PostType,
                    TransactionType = IncomeExpenseTransactionTypeEnum.Income,
                    PayType = IncomeExpensePayType.Amount,
                };

                result.TotalAmount = await WalletService.GetTodayTotalIncomeExpense(totalReq).GetReturnDataAsync();

                return new BaseReturnDataModel<IncomeSummaryViewModel>(ReturnCode.Success)
                {
                    DataModel = result,
                };
            }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExpenseSummaryViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExpenseInfo(ExpenseInfoData query)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                int userId = User.GetUserId();

                ReqExpenseInfo req = new ReqExpenseInfo
                {
                    UserId = userId,
                    Date = query.Date,
                    PageNo = query.PageNo ?? query.Page ?? 1,
                };

                var res = await WalletService.ExpenseInfo(req);

                if (res.IsSuccess == false)
                {
                    return new BaseReturnDataModel<ExpenseSummaryViewModel>(res);
                }

                var result = Mapper.Map<ExpenseSummaryViewModel>(res.DataModel);

                ReqTodayTotalIncomeExpense totalReq = new ReqTodayTotalIncomeExpense
                {
                    UserId = userId,
                    Date = query.Date,
                    TransactionType = IncomeExpenseTransactionTypeEnum.Expense,
                    PayType = IncomeExpensePayType.Point,
                };

                result.TotalAmount = await WalletService.GetTodayTotalIncomeExpense(totalReq).GetReturnDataAsync();

                return new BaseReturnDataModel<ExpenseSummaryViewModel>(ReturnCode.Success)
                {
                    DataModel = result,
                };
            }));
        }
    }
}
using MS.Core.MM.Models.Wallets;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Services.interfaces
{
    public interface IWalletService
    {
        /// <summary>
        /// 消費明細
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<ResExpenseInfo>>> ExpenseInfo(ReqExpenseInfo req);

        Task<BaseReturnDataModel<decimal>> GetTodayTotalIncomeExpense(ReqTodayTotalIncomeExpense req);

        Task<BaseReturnDataModel<decimal>> GetSumGetIncomeByFilter(ReqTodayTotalIncomeExpense req);

        /// <summary>
        /// 收益明細
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<ResIncomeInfo>>> IncomeInfo(ReqIncomeInfo req);

        /// <summary>
        /// 覓錢包
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ResWalletInfo>> WalletInfo(ReqWalletInfo req);
    }
}
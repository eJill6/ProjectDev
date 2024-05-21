using MS.Core.MM.Models.Entities.IncomeExpense;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPostTransaction;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.My;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Linq.Expressions;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IIncomeExpenseRepo
    {
        /// <summary>
        /// 取得月收益
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="now">日期</param>
        /// <returns>該月收益</returns>
        Task<decimal> GetMonthIncome(int userId, DateTime now);

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<MMIncomeExpenseModel>> GetTransactionByFilter(IncomeExpenseFilter filter);

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<PageResultModel<MMIncomeExpenseModel>> GetPageTransactionByFilter(PageIncomeExpenseFilter filter);
        /// <summary>
        /// 查询非觅老板与超觅老板的收付记录
        /// </summary>
        /// <param name="paramter"></param>
        /// <returns></returns>
        Task<PageResultModel<AdminUserManagerOfficialIncomeExpensesList>> GetPageIncomeExpenseData(QueryIncomeExpensePageParamter paramter);
        /// <summary>
        /// 查询觅老板与超觅老板的收付记录
        /// </summary>
        /// <param name="paramter"></param>
        /// <returns></returns>
        Task<PageResultModel<AdminUserManagerOfficialIncomeExpensesList>> GetPageOfficialIncomeExpenseData(QueryIncomeExpensePageParamter paramter);

        /// <summary>
        /// 後台取得解锁单记录
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>解锁单记录</returns>
        Task<PageResultModel<MMIncomeExpenseModel>> List(AdminPostTransactionListParam param);

        /// <summary>
        /// 後台透過特定編號取得收支记录
        /// </summary>
        /// <param name="param">查詢參數</param>
        /// <returns>收支记录</returns>
        Task<PageResultModel<MMIncomeExpenseModel>> List(AdminIncomeExpenseListByTypeParam param);

        /// <summary>
        /// 暫鎖收益
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<decimal> GetFreezeIncome(int userId);

        /// <summary>
        /// 透過舉報的參數取的支出收益單
        /// </summary>
        /// <param name="param">舉報的參數</param>
        /// <returns>支出收益單</returns>
        Task<MMIncomeExpenseModel?> GetByReport(AdminIncomeExpenseByReport param);

        /// <summary>
        /// 後台更新收益支出單的狀態
        /// </summary>
        /// <param name="param"></param>
        /// <returns>更新結果</returns>
        Task<DBResult> Update(AdminIncomeExpenseUpdateStatusParam param);

        Task<decimal> GetSumTransactionByFilter(IncomeExpenseFilter filter);

        Task<decimal> GetSumGetIncomeByFilter(IncomeExpenseFilter filter);

        /// <summary>
        /// 後台取得收益單列表
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>收益單列表</returns>
        Task<PageResultModel<MMIncomeExpenseModel>> List(AdminIncomeListParam param);

        Task<DBResult> UpdateStatus(MMIncomeExpenseModel incomeExpense);

        Task<DBResult> UpdateStatus(IEnumerable<MMIncomeExpenseModel> incomeExpenses);

        Task<DBResult> UpdateZeroAmountStatus();

        Task<DBResult> Dispatched(IEnumerable<MMIncomeExpenseModel> incomeExpenses);

        Task<DBResult> Dispatched(MMIncomeExpenseModel incomeExpense);

        Task<IEnumerable<MMIncomeExpenseModel>> GetTransactionByFilterOrderByDescending(IncomeExpenseFilter filter);

        Task<MMIncomeExpenseModel?> GetBySourceId(string sourceId);

        Task<PageResultModel<AdminIncomeList>> GetIncomeExpenseList(AdminIncomeListParam param);
    }
}
using MS.Core.MM.Models;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.User;
using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    public interface IUserSummaryService
    {
        /// <summary>
        /// 取得使用者數量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<decimal>> GetUserAmount(int userId, UserSummaryTypeEnum type, UserSummaryCategoryEnum category);
        /// <summary>
        /// 取得多筆使用者的數量
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<UserSummaryModel[]>> GetUserSummaris(IEnumerable<int> userIds);
        /// <summary>
        /// 取得使用者摘要(總收益等等)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<UserSummaryModel[]>> GetUserSummaris(int userId);
        /// <summary>
        /// 重置 User UnLock 次數
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnModel> RestSetUserUnLock();
    }
}
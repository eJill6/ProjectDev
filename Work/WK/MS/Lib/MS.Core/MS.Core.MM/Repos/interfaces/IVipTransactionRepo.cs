using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IVipTransactionRepo
    {
        /// <summary>
        /// Vip 購買紀錄
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<MMVipTransaction>> GetUserVipLogs(int userId);
        /// <summary>
        /// Vip 購買次數
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUserVipLogCount(int userId, VipType vipType);

        Task<string> GetSequenceIdentity();

        Task<string> GetSequenceIdentity<T>() where T : BaseDBModel;

        /// <summary>
        /// 取得特定類型的會員卡資料
        /// </summary>
        /// <param name="vipId">會員卡類型編號</param>
        /// <returns>會員卡資料</returns>
        [Obsolete]
        Task<MMVipTransaction[]> GetUserEfficientVipsByType(int vipId);

        /// <summary>
        /// 後台取的使用者會員卡資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>使用者會員卡資料</returns>
        Task<PageResultModel<MMVipTransaction>> GetVips(AdminUserManagerUserCardsListParam param);

        /// <summary>
        /// 取得具有 Vip會員的 UserId
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetVipUserIds(IEnumerable<int> userIds);
    }
}

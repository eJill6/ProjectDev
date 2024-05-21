using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.User;
using MS.Core.MM.Models.Vip;
using MS.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace MS.Core.MM.Services.interfaces
{
    public interface IVipService
    {
        /// <summary>
        /// 取得所有會員卡
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<ResVip[]>> GetVips();

        /// <summary>
        /// 取得販售的會員卡(上架中)
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<ResVip[]>> GetListedVips();

        /// <summary>
        /// 使用者 VIP 購買紀錄
        /// </summary>
        /// <param name="vipLog"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ResUserVipTransLog[]>> GetUserVipTransLogs(ReqUserVipTransLog vipLog);

        /// <summary>
        /// 會員詳細資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserInfoData> GetUserInfoData(int userId);

        /// <summary>
        /// 會員詳細資訊
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserInfoData> GetUserInfoData([NotNull] MMUserInfo user);

        Task<UserSummaryInfoData> GetUserSummaryInfoData(int userId);
        Task<MMVipType[]> GetVipTypes();
        /// <summary>
        /// 是否Vip
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsVip(int userId);
    }
}
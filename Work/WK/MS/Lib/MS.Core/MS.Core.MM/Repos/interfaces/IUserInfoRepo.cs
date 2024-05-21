using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Auth.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Linq.Expressions;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IUserInfoRepo
    {
        /// <summary>
        /// 取得用戶
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MMUserInfo?> GetUserInfo(int userId);

        /// <summary>
        /// 取得用戶-多筆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<MMUserInfo>> GetUserInfos(IEnumerable<int> userIds);

        Task<IEnumerable<MMBoss>> GetFavoriteBoss(string[] applyIdArray);

        Task<IEnumerable<MMIdentityApply>> GetFavoriteApply(string[] applyIdArray);

        Task<IEnumerable<MMPost>> GetFavoritePost(string[] applyIdArray, int postType);

        Task<bool> DeleteFavorite(MMPostFavorite entity);

        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        Task<bool> Insert(MMUserInfo entity);

        /// <summary>
        /// 更新用戶的暱稱及頭像
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">用戶</param>
        /// <param name="avatar">用戶頭像</param>
        /// <returns></returns>
        Task<bool> UpdateNicknameAvatar(int userId, string? nickname, string? avatar);

        /// <summary>
        /// 新增修改用戶
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="visitType">訪問類型</param>
        /// <returns></returns>
        Task<bool> UserUpsert(MMUserInfo entity, VisitType visitType = VisitType.Login);

        /// <summary>
        /// 後台取得會員資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>會員資料</returns>
        Task<PageResultModel<MMUserInfo>> GetUserInfos(AdminUserManagerUsersListParam param);

        /// <summary>
        /// 申請身份
        /// </summary>
        /// <param name="entity">身份申請資料</param>
        /// <returns></returns>
        Task<bool> ApplyIdentity(MMIdentityApply entity);

        /// <summary>
        /// 申請覓老闆身份
        /// </summary>
        /// <param name="entity">身份申請資料</param>
        /// <param name="bossEntity">覓老闆填寫資料</param>
        /// <param name="mediaEntity">上傳圖片</param>
        /// <returns></returns>
        Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia mediaEntity);

        Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia[] mediaEntity);

        Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia mediaEntity, bool IsAdminApply);

        Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia[] mediaEntity, bool IsAdminApply);
        /// <summary>
        /// 修改申请记录申请身份
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="applyIdentity"></param>
        /// <returns></returns>
        Task<bool> ApplyIdentityEdit(string applyId, byte applyIdentity);
        /// <summary>
        /// 查询用户的分成比
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetBossPlatformSharing(int userId);
        /// <summary>
        /// 更新觅老板的平台拆账比
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="platformSharing"></param>
        /// <returns></returns>
        Task<bool> BossPlatformSharingUpdate(string applyId, int platformSharing);
        /// <summary>
        /// 获取到申请的ID
        /// </summary>
        /// <returns></returns>
        Task<string> GetMMIdentityApplySequenceIdentity();

        /// <summary>
        /// 是否已送出
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<bool> IsAlreadyIdentityApply(int userId);

        /// <summary>
        /// 身份认证申请
        /// </summary>
        /// <param name="param">申请信息</param>
        /// <returns></returns>
        Task<DBResult> UserIdentityApply(AdminUserManagerIdentityApplyParam param);
        Task<UserBossApplyInfoModel> QueryUserBossInfoAndIdentityApplyInfo(int userId);

        /// <summary>
        /// 取得申請資訊
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<MMIdentityApply[]> GetIdentityApplyData(int userId);

        Task<IEnumerable<BriefUserInfo>> GetBriefUserInfoByFilter(BriefUserInfoFilter filter);

        Task<IEnumerable<MMUserSummary>> GetUserSummary(UserSummaryFilter filter);

        /// <summary>
        /// 設定商場營業中(或關閉)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ShopOpened(int userId);

        /// <summary>
        /// 設定商場營業中關閉
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UpdateShopStatus(int userId, bool isOpen);

        /// <summary>
        /// 保证金调整记录
        /// </summary>
        /// <returns>保证金调整记录</returns>
        Task<IEnumerable<MMEarnestMoneyHistory>> GetEarnestMoneyData(int userId);

        /// <summary>
        /// 保证金修改申请
        /// </summary>
        /// <returns></returns>
        Task<bool> EarnestMoneyAudit(AdminUserManagerEarnestMoneyChangeParam param);

        /// <summary>
        /// 获取老板信息
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<MMBoss> GetByApplyId(string applyId);

        /// <summary>
        /// 修改店铺信息
        /// </summary>
        /// <returns></returns>
        Task<bool> StoreEdit(AdminUserBossParam param);

        Task<(IEnumerable<MMOfficialShopList>, int)> OfficialShopList(ReqOfficialShop param);

        /// <summary>
        /// 申请状态修改
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> UpdateApplyStatusByApplyId(string applyId, ReviewStatus status, string contactApp, string contactInfo);

        /// <summary>
        /// 更新店铺的觀看次數
        /// </summary>
        /// <param name="applyId">申请id</param>
        /// <returns></returns>
        Task<bool> UpdateShopViewsCount(string applyId);
    }
}
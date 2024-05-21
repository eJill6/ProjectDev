using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Auth.Enums;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Telegram.Bot.Types;

namespace MS.Core.MM.Repos
{
    public class UserInfoRepo : BaseInlodbRepository, IUserInfoRepo
    {
        private IDateTimeProvider DateTimeProvider { get; }

        /// <summary>
        /// 用戶相關
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        public UserInfoRepo(IOptionsMonitor<MsSqlConnections> setting,
            IRequestIdentifierProvider provider,
            IDateTimeProvider dateTimeProvider,
            ILogger logger) : base(setting, provider, logger)
        {
            DateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<MMUserSummary>> GetUserSummary(UserSummaryFilter filter)
        {
            var dapper = QueryByFilter(filter);
            return await dapper.QueryAsync();
        }

        public async Task<IEnumerable<BriefUserInfo>> GetBriefUserInfoByFilter(BriefUserInfoFilter filter)
        {
            var dapper = QueryByFilter(filter);
            return await dapper.SelectQueryAsync(e => new BriefUserInfo
            {
                AvatarUrl = e.AvatarUrl,
                Nickname = e.Nickname,
                UserId = e.UserId,
            });
        }

        private DapperQueryComponent<MMUserSummary> QueryByFilter(UserSummaryFilter filter)
        {
            var queryComponent = ReadDb.QueryTable<MMUserSummary>();

            if (filter.UserId.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.UserId == filter.UserId);
            }
            if (filter.Category.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.Category == (UserSummaryCategoryEnum)filter.Category);
            }
            if (filter.Type.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.Type == (UserSummaryTypeEnum)filter.Type);
            }
            return queryComponent;
        }

        private DapperQueryComponent<MMUserInfo> QueryByFilter(BriefUserInfoFilter filter)
        {
            var queryComponent = ReadDb.QueryTable<MMUserInfo>();

            if (filter.UserIds.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.UserIds.Contains(e.UserId));
            }

            return queryComponent;
        }

        public async Task<IEnumerable<MMUserInfo>> GetUserInfos(IEnumerable<int> userIds)
        {
            return await WriteDb.QueryTable<MMUserInfo>().Where(e => userIds.Contains(e.UserId)).QueryAsync();
        }

        /// <summary>
        /// 取得用戶
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<MMUserInfo?> GetUserInfo(int userId)
        {
            return await WriteDb.QueryTable<MMUserInfo>().Where(e => e.UserId == userId).QueryAsync().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<bool> Insert(MMUserInfo info)
        {
            try
            {
                await WriteDb.Insert(info).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增用戶失敗。UserId：{info.UserId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新用戶的暱稱及頭像
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">用戶</param>
        /// <param name="avatar">用戶頭像</param>
        /// <returns></returns>
        public async Task<bool> UpdateNicknameAvatar(int userId, string? nickname, string? avatar)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nickname) && string.IsNullOrWhiteSpace(avatar))
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int32);

                List<string> paramCmd = new List<string>();
                if (string.IsNullOrWhiteSpace(nickname) == false)
                {
                    paramCmd.Add("Nickname=@Nickname");
                    parameters.Add("@Nickname", nickname, DbType.String, size: 50);
                }

                if (string.IsNullOrWhiteSpace(avatar) == false)
                {
                    paramCmd.Add("AvatarUrl=@AvatarUrl");
                    parameters.Add("@AvatarUrl", avatar, DbType.String, size: 250);
                }

                string sqlCmd = $@" UPDATE MMUserInfo SET ";
                sqlCmd += string.Join(',', paramCmd);
                sqlCmd += " WHERE UserId = @UserId ";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，更新用戶暱稱及頭像失敗。UserId：{userId}、Nickname：{nickname}、Avatar：{avatar}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增修改用戶
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="visitType">訪問類型</param>
        /// <returns></returns>
        public async Task<bool> UserUpsert(MMUserInfo entity, VisitType visitType = VisitType.Login)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", entity.UserId, DbType.Int32);
                parameters.Add("@Nickname", entity.Nickname, DbType.String, size: 50);
                parameters.Add("@AvatarUrl", entity.AvatarUrl, DbType.String, size: 250);
                parameters.Add("@UserIdentity", entity.UserIdentity, DbType.Int32);
                parameters.Add("@UserLevel", entity.UserLevel, DbType.Int32);
                parameters.Add("@RewardsPoint", entity.RewardsPoint, DbType.Int32);
                parameters.Add("@CreateTime", entity.CreateTime, DbType.DateTime);
                parameters.Add("@UpdateTime", entity.UpdateTime, DbType.DateTime);
                parameters.Add("@RegisterTime", entity.RegisterTime, DbType.DateTime);

                parameters.Add("@VisitType", visitType, DbType.Int32);

                string sqlCmd = $@"
IF NOT EXISTS(SELECT TOP 1 1 FROM MMUserInfo WIHT(NOLOCK) WHERE UserId=@UserId)
BEGIN
    INSERT INTO MMUserInfo (
         UserId
        ,Nickname
        ,AvatarUrl
        ,UserIdentity
        ,UserLevel
        ,RewardsPoint
        ,CreateTime
        ,UpdateTime
        ,RegisterTime
    ) VALUES (
        @UserId
        ,@Nickname
        ,@AvatarUrl
        ,@UserIdentity
        ,@UserLevel
        ,@RewardsPoint
        ,@CreateTime
        ,@UpdateTime
        ,@RegisterTime
    );
END;
ELSE
BEGIN
    DECLARE @TmpNickname  NVARCHAR(50) = @Nickname;
    DECLARE @TmpAvatarUrl NVARCHAR(250) = @AvatarUrl;

    IF ISNULL(@Nickname, '') = ''
    BEGIN
        SET @TmpNickname = NULL;
    END;

    IF ISNULL(@AvatarUrl, '') = ''
    BEGIN
        SET @TmpAvatarUrl = NULL;
    END;

    UPDATE MMUserInfo
    SET Nickname = ISNULL(@TmpNickname, Nickname)
        ,AvatarUrl = ISNULL(@TmpAvatarUrl, AvatarUrl)
    WHERE UserId = @UserId;
END;

INSERT INTO MMUserLoginLog (
    UserId
    ,VisitType
) VALUES (
    @UserId
    ,@VisitType
)
";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增用戶失敗。UserId：{entity.UserId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 後台取得會員資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>會員資料</returns>
        public async Task<PageResultModel<MMUserInfo>> GetUserInfos(AdminUserManagerUsersListParam param)
        {
            var component = ReadDb.QueryTable<MMUserInfo>();
            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }

            // 查詢會員卡
            if (param.UserIds != null)
            {
                var userIds = param.UserIds;
                component.Where(x => userIds.Contains(x.UserId));
            }

            if (param.UserIdentity.HasValue)
            {
                component.Where(x => x.UserIdentity == param.UserIdentity);
            }

            if (param.IsOpen.HasValue)
            {
                component.Where(x => x.IsOpen == param.IsOpen);
            }

            if (param.BeginDate != null && param.EndDate != null)
            {
                var begin = param.BeginDate;
                var end = param.EndDate;
                component.Where(e => e.RegisterTime >= begin && e.RegisterTime < end);
            }
            return await component
                .OrderByDescending(e => e.RegisterTime)
                .QueryPageResultAsync(param);
        }

        /// <summary>
        /// 身份认证申请
        /// </summary>
        /// <returns></returns>
        public async Task<DBResult> UserIdentityApply(AdminUserManagerIdentityApplyParam param)
        {
            try
            {

                var parmater = new DynamicParameters();

                parmater.Add("@UserId",param.UserId);
                parmater.Add("@EarnestMoney", param.EarnestMoney);
                parmater.Add("@ExtraPostCount", param.ExtraPostCount);
                parmater.Add("@ApplyIdentity", param.ApplyIdentity);
                parmater.Add("@Memo", param.Memo);
                parmater.Add("@Contact", param.Contact);
                parmater.Add("@ContactAPP", param.ContactApp);

                var result = await WriteDb.QueryFirstAsync<DBResult>(
                   "[dbo].[Pro_MMUserInfoAdminUpdate]",
                   paras: parmater,
                   commandTimeout: 30,
                   CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(UserIdentityApply)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，提交身份认证申请执行失敗。参数：\r\n{JsonConvert.SerializeObject(param)}");
            }
            return new DBResult(ReturnCode.SystemError);
        }
        public async Task<UserBossApplyInfoModel> QueryUserBossInfoAndIdentityApplyInfo(int userId)
        {
            try
            {
                var userInfo= await ReadDb.QueryTable<MMUserInfo>().Where(c=>c.UserId==userId).QueryFirstAsync();
                if (userInfo == null){
                    return null;
                }
                var info = new UserBossApplyInfoModel();
                var identityApplyInfo = await ReadDb.QueryTable<MMIdentityApply>().Where(c => c.UserId == userId  && c.Status == 1).QueryFirstAsync();
               
                info.UserId=userInfo.UserId;
                info.ApplyIdentity = userInfo.UserIdentity;
                info.Memo = userInfo.Memo;
                info.EarnestMoney=userInfo.EarnestMoney;
                info.ExtraPostCount=userInfo.ExtraPostCount;
                if (identityApplyInfo != null){
                    info.Contact = identityApplyInfo.ContactInfo;
                    info.ContactApp = identityApplyInfo.ContactApp;
                    
                    var bossInfo = await ReadDb.QueryTable<MMBoss>().Where(c => c.ApplyId == identityApplyInfo.ApplyId).QueryFirstAsync();
                    if(bossInfo!=null){
                        info.PlatformSharing = bossInfo.PlatformSharing.HasValue ? bossInfo.PlatformSharing : 0;
                    }
                }

                return info;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，查询用户得店铺和申请信息失败。UserId：{userId}");
                return null;
            }
        }
        /// <summary>
        /// 申請身份
        /// </summary>
        /// <param name="entity">身份申請資料</param>
        /// <returns></returns>
        public async Task<bool> ApplyIdentity(MMIdentityApply entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.ApplyId))
                {
                    entity.ApplyId = await GetSequenceIdentity<MMIdentityApply>();
                }
                await WriteDb.Insert(entity).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增身份申請失敗。UserId：{entity.UserId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 申請覓老闆身份
        /// </summary>
        /// <param name="entity">身份申請資料</param>
        /// <param name="bossEntity">覓老闆填寫資料</param>
        /// <param name="mediaEntity">上傳圖片</param>
        /// <returns></returns>
        public async Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia mediaEntity)
        {
            return await ApplyBossIdentity(entity, bossEntity, mediaEntity, false);
        }

        public async Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia[] mediaEntity)
        {
            return await ApplyBossIdentity(entity, bossEntity, mediaEntity, false);
        }

        public async Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia mediaEntity, bool IsAdminApply)
        {
            try
            {
                entity.ApplyId = await GetSequenceIdentity<MMIdentityApply>();
                bossEntity.BossId = await GetSequenceIdentity<MMBoss>();
                bossEntity.ApplyId = entity.ApplyId;
                mediaEntity.RefId = bossEntity.BossId;
                mediaEntity.ModifyDate = DateTime.Now;

                if (IsAdminApply)
                {
                    entity.Status = 1;
                    await WriteDb.Insert(entity)
                  .Insert(bossEntity)
                  .SaveChangesAsync();
                }
                else
                {
                    await WriteDb.Insert(entity)
                  .Insert(bossEntity)
                  .Update(mediaEntity)
                  .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增覓老闆身份申請失敗。UserId：{entity.UserId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新boss表内的平台分成比
        /// </summary>
        /// <returns></returns>
        public async Task<bool> BossPlatformSharingUpdate(string applyId,int platformSharing)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PlatformSharing", platformSharing, DbType.Int32);
                parameters.Add("@ApplyId", applyId, DbType.String);
                string sqlCmd = @"UPDATE MMBoss SET PlatformSharing=@PlatformSharing WHERE ApplyId=@ApplyId;";
                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，更新Boss平台拆账比失败。申请ID：{applyId}");
                return false;
            }
        }
        public async Task<bool> ApplyIdentityEdit(string applyId, byte applyIdentity)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ApplyIdentity", applyIdentity, DbType.Int32);
                parameters.Add("@ApplyId", applyId, DbType.String);

                string sqlCmd = @"UPDATE MMIdentityApply SET ApplyIdentity = @ApplyIdentity WHERE ApplyId = @ApplyId;";
                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();

                return true;
            }catch(Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，修改申请记录的身份失败。申请ID：{applyId}");
                return false;
            }
        }
        public async Task<int> GetBossPlatformSharing(int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int32);
                string sqlStr = " SELECT MB.PlatformSharing FROM  MMBoss AS MB WITH(NOLOCK) INNER JOIN MMIdentityApply AS MA WITH(NOLOCK) ON MB.[ApplyId]=MA.[ApplyId] WHERE MA.Status=1 AND MA.UserId=@UserId AND MA.ApplyIdentity=5 ";
                return await ReadDb.QueryScalarAsync<int>(sqlStr, parameters);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，查询相关用户的平台分成。UserId：{userId}");
                return 0;
            }
        }

        public async Task<string> GetMMIdentityApplySequenceIdentity()
        {
            return await GetSequenceIdentity<MMIdentityApply>();
        }

        public async Task<bool> ApplyBossIdentity(MMIdentityApply entity, MMBoss bossEntity, MMMedia[] mediaEntityList, bool IsAdminApply)
        {
            try
            {
                entity.ApplyId = await GetSequenceIdentity<MMIdentityApply>();
                bossEntity.BossId = await GetSequenceIdentity<MMBoss>();
                bossEntity.ApplyId = entity.ApplyId;
                //mediaEntity.RefId = bossEntity.BossId;
                //mediaEntity.ModifyDate = DateTime.Now;
                foreach (var item in mediaEntityList)
                {
                    item.RefId = bossEntity.BossId;
                    item.ModifyDate = DateTime.Now;
                }

                if (IsAdminApply)
                {
                    entity.Status = 1;
                    await WriteDb.Insert(entity)
                  .Insert(bossEntity)
                  .SaveChangesAsync();
                }
                else
                {
                    await WriteDb.Insert(entity)
                  .Insert(bossEntity)
                  .Update(mediaEntityList)
                  .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增覓老闆身份申請失敗。UserId：{entity.UserId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 修改申请状态
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> UpdateApplyStatusByApplyId(string applyId, ReviewStatus status, string contactApp, string contactInfo)
        {
            try
            {
                var entity = await ReadDb.QueryTable<MMIdentityApply>().Where(c => c.ApplyId == applyId).QueryFirstAsync();
                entity.Status = (byte)status;
                entity.ContactApp = contactApp;
                entity.ContactInfo = contactInfo;
                await WriteDb.Update(entity).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，修改申请状态失败。applyId：{applyId}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新店铺的觀看次數
        /// </summary>
        /// <param name="applyId">申请id</param>
        /// <returns></returns>
        public async Task<bool> UpdateShopViewsCount(string applyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(applyId))
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@ApplyId", applyId, DbType.String, size: 32);

                string sqlCmd = $@"UPDATE MMBoss SET Views = Views + 1 WHERE ApplyId = @ApplyId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否已送出
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<bool> IsAlreadyIdentityApply(int userId)
        {
            bool isExists = false;
            try
            {
                isExists = (await WriteDb.QueryTable<MMIdentityApply>()
                    .Where(e => e.UserId == userId && e.Status == (byte)ReviewStatus.UnderReview)
                    .QueryAsync()).Any();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，確認已有身份申請失敗。UserId：{userId}");
            }

            return isExists;
        }

        /// <summary>
        /// 取得申請資訊
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<MMIdentityApply[]> GetIdentityApplyData(int userId)
        {
            MMIdentityApply[] result = Array.Empty<MMIdentityApply>();
            try
            {
                result = (await WriteDb.QueryTable<MMIdentityApply>()
                    .Where(e => e.UserId == userId)
                    .QueryAsync()).ToArray();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，取得身份申請資訊失敗。UserId：{userId}");
            }

            return result;
        }

        /// <summary>
        /// 設定商場營業中(或關閉)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ShopOpened(int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int32);

                string sqlCmd = $@"UPDATE MMUserInfo SET IsOpen = ~IsOpen WHERE UserId = @UserId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，關閉商場營業中設置失敗。UserId：{userId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 設定商場營業中/关闭
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateShopStatus(int userId, bool isOpen)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.Int32);
                parameters.Add("@IsOpen", isOpen, DbType.Boolean);

                string sqlCmd = $@"UPDATE MMUserInfo SET IsOpen = @IsOpen WHERE UserId = @UserId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，關閉商場營業中設置失敗。UserId：{userId},IsOpen：{isOpen}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保证金调整记录
        /// </summary>
        /// <returns>保证金调整记录</returns>
        public async Task<IEnumerable<MMEarnestMoneyHistory>> GetEarnestMoneyData(int userId)
        {
            var component = ReadDb.QueryTable<MMEarnestMoneyHistory>();
            return await component
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ExamineTime)
                .QueryAsync();
        }

        /// <summary>
        /// 保证金修改申请
        /// </summary>
        /// <returns></returns>
        public async Task<bool> EarnestMoneyAudit(AdminUserManagerEarnestMoneyChangeParam param)
        {
            try
            {
                var entity = new MMEarnestMoneyHistory
                {
                    EarnestMoney = param.EarnestMoney,
                    UserId = param.UserId,
                    ExamineMan = param.ExamineMan,
                    ExamineTime = DateTimeProvider.Now
                };
                var userInfo = await GetUserInfo(param.UserId);
                userInfo.EarnestMoney += param.EarnestMoney;
                await WriteDb.Insert(entity).Update(userInfo).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增保证金修改申請失敗。UserId：{param.UserId}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据applyId数组获取boss信息
        /// </summary>
        /// <param name="applyIdArray"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MMBoss>> GetFavoriteBoss(string[] applyIdArray)
        {
            var bossData = await WriteDb.QueryTable<MMBoss>().Where(c => applyIdArray.Contains(c.ApplyId)).QueryAsync();
            return bossData;
        }

        public async Task<IEnumerable<MMPost>> GetFavoritePost(string[] applyIdArray, int postType)
        {
            var postData = await WriteDb.QueryTable<MMPost>().Where(c => applyIdArray.Contains(c.PostId) && (int)c.PostType == postType).QueryAsync();
            return postData;
        }

        public async Task<IEnumerable<MMIdentityApply>> GetFavoriteApply(string[] applyIdArray)
        {
            var applyData = await WriteDb.QueryTable<MMIdentityApply>().Where(c => applyIdArray.Contains(c.ApplyId)).QueryAsync();
            return applyData;
        }

        /// <summary>
        /// 删除收藏的店铺
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFavorite(MMPostFavorite entity)
        {
            try
            {
                await WriteDb.Delete<MMPostFavorite>(entity).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，删除官方店铺收藏记录失败 FavoriteId：{entity.FavoriteId}");
                return false;
            }
            return true;
        }

        public async Task<MMBoss> GetByApplyId(string applyId)
        {
            return await ReadDb.QueryTable<MMBoss>().Where(x => x.ApplyId == applyId).QueryFirstAsync();
        }

        public async Task<(IEnumerable<MMOfficialShopList>, int)> OfficialShopList(ReqOfficialShop param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Keyword", param.Keyword, DbType.String);
            parameters.Add("@SortType", param.SortType, DbType.Int32);
            parameters.Add("@PageNo", param.PageNo, DbType.Int32);
            parameters.Add("@PageSize", param.PageSize, DbType.Int32);
            parameters.Add("@RowCount", DbType.Int32, direction: ParameterDirection.Output);

            int rowCount = 0;
            MMOfficialShopList[] result = new MMOfficialShopList[0];
            try
            {
                result = (await WriteDb.QueryAsync<MMOfficialShopList>(
                    "[dbo].[Pro_MMOfficialShopList]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure)).ToArray();

                rowCount = parameters.Get<int>("@RowCount");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，官方店铺列表帖子查询失敗。UserId：{param.Keyword}");
            }

            return (result, rowCount);
        }

        /// <summary>
        /// 后台店铺编辑
        /// </summary>
        /// <returns></returns>

        public async Task<bool> StoreEdit(AdminUserBossParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ApplyId", param.ApplyId, DbType.String);
            parameters.Add("@BossId", param.BossId, DbType.String);
            parameters.Add("@ShopName", param.ShopName, DbType.String);
            parameters.Add("@ShopYears", param.ShopYears, DbType.Int32);
            parameters.Add("@Girls", param.Girls, DbType.Int32);
            parameters.Add("@DealOrder", param.DealOrder, DbType.Int32);
            parameters.Add("@SelfPopularity", param.SelfPopularity, DbType.Int32);
            parameters.Add("@Introduction", param.Introduction, DbType.String);
            parameters.Add("@BusinessDate", param.BusinessDate, DbType.String);
            parameters.Add("@BusinessHour", param.BusinessHour, DbType.String);
            parameters.Add("@BusinessPhotoUrls", param.BusinessPhotoUrls, DbType.String);
            parameters.Add("@ShopAvatarIds", param.ShopAvatar, DbType.String);
            parameters.Add("@TelegramGroupId", param.TelegramGroupId, DbType.String);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMBossAdminEdit]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result.IsSuccess;
        }
    }
}
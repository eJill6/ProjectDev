using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Entities.BossShop;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using System.Data;
using System.Reflection;

namespace MS.Core.MM.Repos
{
    public class BossShopRepo : BaseInlodbRepository<MMBossShop>, IBossShopRepo
    {
        public BossShopRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<bool> Create(MMBossShop param)
        {
            param.Id = await GetSequenceIdentity();
            await WriteDb.Insert(param).SaveChangesAsync();

            try
            {
                var boss = await ReadDb.QueryTable<MMBoss>().Where(x => x.ApplyId == param.ApplyId).QueryFirstAsync();
                var user = await ReadDb.QueryTable<MMUserInfo>().Where(x => x.UserId == param.UserId).QueryFirstAsync();
                var media = (await ReadDb.QueryTable<MMMedia>().Where(x => x.RefId == boss.BossId && x.MediaType == 0).QueryAsync()).ToList();

                var shopAvatarSource = string.Join(",", media.Where(a => a.SourceType == 4).Select(a => a.Id));
                var businessPhotoSource = string.Join(",", media.Where(a => a.SourceType == 6).Select(a => a.Id));

                var insertDate = new MMBossHistory()
                {
                    BossShopId = param.Id,
                    ApplyId = boss.ApplyId,
                    UserId = param.UserId,
                    BossId = boss.BossId,
                    ContactApp = user.ContactApp,
                    ContactInfo = user.Contact,
                    ShopName = boss.ShopName,
                    Girls = Convert.ToInt32(boss.Girls),
                    ShopYears = boss.ShopYears.Value,
                    DealOrder = boss.DealOrder.Value,
                    SelfPopularity = boss.SelfPopularity.Value,
                    Introduction = boss.Introduction,
                    ShopAvatarSource = shopAvatarSource,
                    BusinessPhotoSource = businessPhotoSource,
                    Status = ReviewStatus.UnderReview,
                    CreateTime = DateTime.Now,
                    ExamineMan = "system"
                };

                await CreateBossCurrent(insertDate);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，添加MMBossHistory失败。param：{param}");
            }

            return true;
        }

        /// <summary>
        /// 增加店铺编辑审核当前记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<bool> CreateBossCurrent(MMBossHistory param)
        {
            param.Id = await GetSequenceIdentity();
            await WriteDb.Insert(param).SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 查询店铺编辑列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MMBossShop>> List(AdminBossShopListParam param)
        {
            var component = ReadDb.QueryTable<MMBossShop>();
            var beginDate = param.BeginDate;
            var endDate = param.EndDate;
            component.Where(x => x.CreateTime >= beginDate && x.CreateTime < endDate);

            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }
            return await component.OrderBy(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        /// <summary>
        /// 查询店铺编辑信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<MMBossShop> GetBossShopDetail(string id)
        {
            return await ReadDb.QueryTable<MMBossShop>().Where(x => x.Id == id).QueryFirstAsync();
        }

        /// <summary>
        /// 查询当前店铺信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<MMBoss> GetBossDetail(string id)
        {
            return await ReadDb.QueryTable<MMBoss>().Where(x => x.BossId == id).QueryFirstAsync();
        }

        /// <summary>
        /// 查询当前店铺信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<MMBoss> GetBossDetailByApplyId(string applyId)
        {
            return await ReadDb.QueryTable<MMBoss>().Where(x => x.ApplyId == applyId).QueryFirstAsync();
        }
        /// <summary>
        /// 修改超级觅老板店铺平台分成
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="platformSharing"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBossPlatformSharing(MMBoss info)
        {
            try
            {
                await WriteDb.Update<MMBoss>(info).SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，修改觅老板的平台分成失败。申请ID：{info.ApplyId}");
                return false;
            }
        }

        public async Task<bool> InsertBossInfo(MMBoss info)
        {
            try
            {
                await WriteDb.Insert<MMBoss>(info).SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，新增觅老板店铺信息的时候失败。申请ID：{info.ApplyId}");
                return false;
            }
        }

        /// <summary>
        /// 查询店铺编辑审核信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MMBossHistory> GetBossHistoryDetail(string id)
        {
            return await ReadDb.QueryTable<MMBossHistory>().Where(x => x.BossShopId == id).QueryFirstAsync();
        }

        /// <summary>
        /// 后台审核店铺编辑
        /// </summary>
        /// <returns></returns>

        public async Task<bool> BossShopAudit(AdminUserBossParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", param.Id, DbType.String);
            parameters.Add("@ApplyId", param.ApplyId, DbType.String);
            parameters.Add("@BossId", param.BossId, DbType.String);
            parameters.Add("@UserId", param.UserId, DbType.String);
            parameters.Add("@ShopName", param.ShopName, DbType.String);
            parameters.Add("@ShopYears", param.ShopYears, DbType.Int32);
            parameters.Add("@Girls", param.Girls, DbType.Int32);
            parameters.Add("@DealOrder", param.DealOrder, DbType.Int32);
            parameters.Add("@SelfPopularity", param.SelfPopularity, DbType.Int32);
            parameters.Add("@Introduction", param.Introduction, DbType.String);
            parameters.Add("@ExamineMan", param.ExamineMan, DbType.String);
            parameters.Add("@Status", param.Status, DbType.Int32);
            parameters.Add("@ContactApp", param.ContactApp, DbType.String);
            parameters.Add("@ContactInfo", param.ContactInfo, DbType.String);
            parameters.Add("@Memo", param.Memo, DbType.String);
            parameters.Add("@ShopAvatarSource", param.ShopAvatar, DbType.String);
            parameters.Add("@BusinessPhotoSource", param.BusinessPhotoUrls, DbType.String);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMBossShopAudit]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，后台审核店铺编辑失败。param：{param}");
            }

            return result.IsSuccess;
        }

        /// <summary>
        /// 根据条件查询单个Boss
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MMBossShop>> GetBossShopByFilter(QuerySingleBossShopFilter query)
        {
            var component = QueryByFilter(query);
            return await component.QueryAsync();
        }

        private DapperQueryComponent<MMBossShop> QueryByFilter(QuerySingleBossShopFilter query)
        {
            var queryComponent = ReadDb.QueryTable<MMBossShop>();
            if (query.UserId.HasValue)
            {
                queryComponent = queryComponent.Where(c => c.UserId == query.UserId);
            }
            if (query.Status != null)
            {
                queryComponent = queryComponent.Where(c => c.Status == query.Status);
            }
            if (!string.IsNullOrEmpty(query.ApplyId))
            {
                queryComponent = queryComponent.Where(c => c.ApplyId == query.ApplyId);
            }
            if (!string.IsNullOrEmpty(query.BossId))
            {
                queryComponent = queryComponent.Where(c => c.BossId == query.BossId);
            }

            return queryComponent;
        }

        public async Task<bool> Update(MMBossShop param)
        {
            await WriteDb.Update(param).SaveChangesAsync();
            return true;
        }

        public async Task<PageResultModel<AdminStoreManageList>> GetStoreList(AdminStoreManageParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@IsOpen", param.IsOpen, DbType.Int32);

            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string querySql = @"
            SELECT
			[UserId], [Nickname] ,[IsOpen]
			INTO #TMP_MMUserInfo
			FROM MMUserInfo WITH(NOLOCK) WHERE (UserIdentity = 3 OR UserIdentity=5) ";

            if (param.UserId.HasValue)
                querySql += " AND UserId = @UserId";
            if (param.IsOpen.HasValue)
                querySql += " AND IsOpen = @IsOpen";

            //根据筛选后的临时表数据查询相关联的觅老板申请记录和店铺信息
            querySql += @"
            SELECT *
			INTO #TMP_InIncomeDATA
			FROM (
			SELECT
					IE.UserId,
					PT.ExamineTime,
					IE.Nickname,
					PR.ShopName,
					IE.IsOpen,
					PT.ApplyId,
					PR.BossId,
                    PR.TelegramGroupId
			FROM #TMP_MMUserInfo IE
			LEFT JOIN (
				SELECT
					T2.UserId,
					T2.Status,
					T2.ExamineTime,
					T2.ApplyId
				FROM MMIdentityApply T2 WITH(NOLOCK)
			) PT ON IE.UserId = PT.UserId AND PT.Status = 1
			INNER JOIN (
				SELECT
					T2.ShopName,
					T2.ApplyId,
					T2.BossId,
                    T2.TelegramGroupId
				FROM MMBoss T2 WITH(NOLOCK)
			) PR ON PT.ApplyId = PR.ApplyId ) list;

            SET @TotalCount=(SELECT COUNT(*) FROM #TMP_InIncomeDATA)

            --分页
            SELECT
                *
            FROM(
                SELECT *,ROW_NUMBER() OVER(ORDER BY [ExamineTime] DESC) AS RowNumber
                FROM  #TMP_InIncomeDATA
            ) AS T
            WHERE T.RowNumber BETWEEN @StartNo AND @EndNo

            DROP TABLE IF EXISTS #TMP_MMUserInfo;
            DROP TABLE IF EXISTS #TMP_InIncomeDATA;";

            var result = await ReadDb.QueryAsync<AdminStoreManageList>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<AdminStoreManageList>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
    }
}
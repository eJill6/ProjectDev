using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Refund;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace MS.Core.MM.Repos
{
    public class IdentityApplyRepo : BaseInlodbRepository, IIdentityApplyRepo
    {
        private IUserInfoRepo _userInfoRepo { get; }
        private IDateTimeProvider DateTimeProvider { get; }

        /// <summary>
        /// 用戶相關
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        public IdentityApplyRepo(IOptionsMonitor<MsSqlConnections> setting,
            IRequestIdentifierProvider provider,
            IUserInfoRepo userInfoRepo,
            IDateTimeProvider dateTimeProvider,
            ILogger logger) : base(setting, provider, logger)
        {
            _userInfoRepo = userInfoRepo;
            DateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 查询身份申请列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MMIdentityApply>> List(AdminUserManagerIdentityApplyListParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@ApplyIdentity", param.ApplyIdentity, DbType.Int32);
            parameters.Add("@OriginalIdentity", param.OriginalIdentity, DbType.Int32);
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@BeginDate", param.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", param.EndDate, DbType.DateTime);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string querySql = @"
                    DROP TABLE IF EXISTS #TMP_MMIdentityApply
                    SELECT
                        [CreateTime]
                       ,[ExamineTime]
                       ,[UserId]
                       ,[ApplyIdentity]
                       ,[ApplyId]
                       ,[Status]

                    INTO #TMP_MMIdentityApply
                    FROM MMIdentityApply WITH(NOLOCK)
                    WHERE CreateTime>=@BeginDate AND CreateTime<@EndDate
             ";

            if (param.ApplyIdentity.HasValue)
            {
                querySql += " AND ApplyIdentity=@ApplyIdentity ";
            }

            if (param.OriginalIdentity.HasValue)
            {
                querySql += " AND OriginalIdentity=@OriginalIdentity ";
            }

            if (param.UserId.HasValue)
            {
                querySql += " AND UserId=@UserId ";
            }

            querySql += $@"

            SET @TotalCount=(SELECT COUNT(*) FROM #TMP_MMIdentityApply );
            --分页
            SELECT
                *
            FROM(
                SELECT *,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNumber
                FROM #TMP_MMIdentityApply
            ) AS T
            WHERE T.RowNumber BETWEEN @StartNo AND @EndNo";

            var result = await ReadDb.QueryAsync<MMIdentityApply>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<MMIdentityApply>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }

        public async Task<PageResultModel<MMIdentityApply>> QueryBossOrSuperBossIdentityApplyRecord(AdminUserManagerIdentityApplyListParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@ApplyIdentity", param.ApplyIdentity, DbType.Int32);
            parameters.Add("@OriginalIdentity", param.OriginalIdentity, DbType.Int32);
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@BeginDate", param.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", param.EndDate, DbType.DateTime);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string querySql = @"
                    DROP TABLE IF EXISTS #TMP_MMIdentityApply
                    SELECT
                        MA.[CreateTime]
                       ,MA.[ExamineTime]
                       ,MA.[UserId]
                       ,MA.[ApplyIdentity]
                       ,MA.[ApplyId]
                       ,MA.[Status]

                    INTO #TMP_MMIdentityApply
                    FROM MMIdentityApply AS MA WITH(NOLOCK) INNER JOIN  MMBoss AS MB WITH(NOLOCK) ON MA.[ApplyId]=MB.[ApplyId]
                    WHERE  (MA.ApplyIdentity=3 OR MA.ApplyIdentity=5) AND MA.CreateTime>=@BeginDate AND MA.CreateTime<@EndDate
             ";

            if (param.OriginalIdentity.HasValue)
            {
                querySql += " AND MA.OriginalIdentity=@OriginalIdentity ";
            }

            if (param.UserId.HasValue)
                querySql += " AND MA.UserId=@UserId ";

            querySql += $@"

            SET @TotalCount=(SELECT COUNT(*) FROM #TMP_MMIdentityApply );
            --分页
            SELECT
                *
            FROM(
                SELECT *,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNumber
                FROM #TMP_MMIdentityApply
            ) AS T
            WHERE T.RowNumber BETWEEN @StartNo AND @EndNo";

            var result = await ReadDb.QueryAsync<MMIdentityApply>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<MMIdentityApply>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }

        /// <summary>
        /// 查询身份申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MMIdentityApply> Detail(string id)
        {
            var component = ReadDb.QueryTable<MMIdentityApply>();
            return await component.Where(x => x.ApplyId == id).QueryAsync().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询身份申请信息ByUserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MMIdentityApply> DetailByUserId(int id)
        {
            return await DetailByUserId(id, 1);
        }

        public async Task<MMIdentityApply> DetailByUserId(int id, int? status)
        {
            var component = WriteDb.QueryTable<MMIdentityApply>();
            if (status != null)
            {
                component.Where(a => a.Status == status && (a.ApplyIdentity == (int)IdentityType.Boss || a.ApplyIdentity == (int)IdentityType.SuperBoss));
            }

            return await component.Where(x => x.UserId == id).QueryAsync().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 根据用户的ID查询该用户申请的 觅老板或者超觅老板的申请记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<MMIdentityApply> QueryBossOrSuperBoss(int id, int? status)
        {
            var component = WriteDb.QueryTable<MMIdentityApply>();
            if (status != null)
            {
                component.Where(a => a.Status == status && (a.ApplyIdentity == (int)IdentityType.Boss || a.ApplyIdentity == (int)IdentityType.SuperBoss));
            }

            return await component.Where(x => x.UserId == id).QueryAsync().FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateIdentityApply(string applyId, int applyIdentity)
        {
            var identityApplyInfo = await ReadDb.QueryTable<MMIdentityApply>().Where(c => c.ApplyId == applyId).QueryFirstAsync();
            identityApplyInfo.ApplyIdentity = (byte)applyIdentity;
            if (identityApplyInfo == null)
            {
                return false;
            }

            try
            {
                await WriteDb.Update<MMIdentityApply>(identityApplyInfo).SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，修改申请身份失败。申请ID：\r\n{applyId}");
                return false;
            }
        }

        /// <summary>
        /// 身份申请审核
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<DBResult> UserIdentityAudit(AdminUserIdentityAuditParam param)
        {
            try
            {
                int? userId = null;
                if (param.Status == 1)
                {
                    var apply = await Detail(param.ApplyId);
                    var userInfo = await _userInfoRepo.GetUserInfo(apply.UserId);
                    userId = apply.UserId;
                    var entity = new MMEarnestMoneyHistory
                    {
                        EarnestMoney = param.EarnestMoney.Value,
                        UserId = userInfo.UserId,
                        ExamineMan = param.ExamineMan,
                        ExamineTime = DateTimeProvider.Now
                    };
                    await WriteDb.Insert(entity).SaveChangesAsync();
                    param.EarnestMoney += userInfo.EarnestMoney;
                }
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMIdentityApplyAdminUpdate]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(UserIdentityAudit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                else
                {
                    //觅老板审核通过默认关闭店铺
                    if (param.Status == 1 && userId != null)
                    {
                        await _userInfoRepo.UpdateShopStatus(userId.Value, false);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，身份认证审核执行失敗。参数：\r\n{JsonConvert.SerializeObject(param)}");
            }
            return new DBResult(ReturnCode.SystemError);
        }
    }
}
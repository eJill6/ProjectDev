using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructure.Redis;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Model.Banner;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.SystemSettings;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Favorite;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.My.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.ReportMessage;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using MS.Core.Utils;
using Newtonsoft.Json;
using System.Data;
using System.Numerics;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace MS.Core.MM.Repos
{
    /// <summary>
    /// 贴子相關
    /// </summary>
    public class PostRepo : BaseInlodbRepository, IPostRepo
    {
        private readonly IPostTransactionRepo _trans = null;
        private readonly IRedisService _cache;

        /// <summary>
        /// 快取用的CacheKey
        /// </summary>
        private readonly string _cacheKey = "MMService:VipPostDayCacheKey";

        /// <summary>
        /// 快取用的Db Index
        /// </summary>
        private readonly int _cacheIndexes = 10;

        /// <summary>
        /// 贴子相關
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <param name="trans"></param>
        public PostRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger, IPostTransactionRepo trans, IRedisService cache) : base(setting, provider, logger)
        {
            _trans = trans;
            _cache = cache;
        }

        /// <summary>
        /// 取得單一贴子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<MMPost?> GetById(string postId)
        {
            return await ReadDb.QueryTable<MMPost>()
                .Where(e => e.PostId == postId)
                .QueryAsync()
                .FirstOrDefaultAsync();
        }

        public async Task<PageResultModel<MMOfficialPost>> GetPageByFilter(PageOfficialPostFilter filter)
        {
            DapperQueryComponent<MMOfficialPost> queryComponent = QueryByFilter(filter);

            return await queryComponent.QueryPageResultAsync(filter.Pagination);
        }

        public async Task<IEnumerable<MMOfficialPost>> GetByFilter(OfficialPostFilter filter)
        {
            DapperQueryComponent<MMOfficialPost> queryComponent = QueryByFilter(filter);

            return await queryComponent.QueryAsync();
        }

        private DapperQueryComponent<MMOfficialPost> QueryByFilter(OfficialPostFilter filter)
        {
            var queryComponent = ReadDb.QueryTable<MMOfficialPost>();

            if (filter.PostIds.IsNotEmpty())
            {
                queryComponent.Where(e => filter.PostIds.Contains(e.PostId));
            }

            return queryComponent;
        }

        /// <summary>
        /// 新增或編輯贴子
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<DBResult> PostUpsert(PostUpsertData info)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PostId", info.PostId, DbType.String);
            parameters.Add("@PostType", info.PostType, DbType.Int32);
            parameters.Add("@UserId", info.UserId, DbType.Int32);
            parameters.Add("@Nickname", info.Nickname, DbType.String);
            parameters.Add("@MessageId", info.MessageId, DbType.Int32);
            parameters.Add("@ApplyAmount", info.ApplyAmount, DbType.Decimal);
            parameters.Add("@ApplyAdjustPrice", info.ApplyAdjustPrice, DbType.Boolean);
            parameters.Add("@Title", info.Title, DbType.String);
            parameters.Add("@AreaCode", info.AreaCode, DbType.String);
            parameters.Add("@Quantity", info.Quantity, DbType.String);
            parameters.Add("@Age", info.Age, DbType.Int32);
            parameters.Add("@Height", info.Height, DbType.Int32);
            parameters.Add("@Cup", info.Cup, DbType.Int32);
            parameters.Add("@BusinessHours", info.BusinessHours, DbType.String);
            parameters.Add("@LowPrice", info.LowPrice, DbType.Decimal);
            parameters.Add("@HighPrice", info.HighPrice, DbType.Decimal);
            parameters.Add("@Address", info.Address, DbType.String);
            parameters.Add("@ServiceDescribe", info.ServiceDescribe, DbType.String);
            //會寫入其他table的資料
            parameters.Add("@ContactInfos", info.ContactInfos, DbType.String);
            parameters.Add("@ServiceIds", info.ServiceIds, DbType.String);
            parameters.Add("@PhotoIds", info.PhotoIds, DbType.String);
            parameters.Add("@VideoIds", info.VideoIds, DbType.String);
            parameters.Add("@OldViewData", info.OldViewData, DbType.String);
            parameters.Add("@IsVipPostDay", info.IsVipPostDay, DbType.Boolean);
            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostOperate]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 編輯官方贴子(Admin)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<DBResult> AdminOfficialPostUpdate(AdminOfficialPostData info)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PostId", info.PostId, DbType.String);
            parameters.Add("@UserId", info.UserId, DbType.Int32);
            parameters.Add("@ExamineMan", info.ExamineMan, DbType.String);
            parameters.Add("@Title", info.Title, DbType.String);
            parameters.Add("@BusinessHours", info.BusinessHours, DbType.String);
            parameters.Add("@ServiceDescribe", info.ServiceDescribe, DbType.String);
            parameters.Add("@Memo", info.Memo, DbType.String);
            parameters.Add("@PostStatus", info.PostStatus, DbType.Int32);
            parameters.Add("@IsHomePost", info.IsHomePost, DbType.Boolean);
            parameters.Add("@Weight", info.Weight, DbType.Int32);
            parameters.Add("@ViewBaseCount", info.ViewBaseCount, DbType.Int32);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMOfficialPostAdminUpdate]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 編輯官方贴编辑锁定状态
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> AdminOfficialPostEditLock(string postId, bool status)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PostId", postId, DbType.String);
                parameters.Add("@LockStatus", status, DbType.Boolean);
                string sqlCmd = $@"UPDATE MMOfficialPost SET LockStatus = @LockStatus WHERE PostId = @PostId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}，編輯官方贴编辑锁定状态失敗。PostId：{postId}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新增舉報單
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<DBResult> InsertReport(ReportCreate info)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ReportType", info.ReportType, DbType.Int32);
            parameters.Add("@ComplainantUserId", info.ComplainantUserId, DbType.Int32);
            parameters.Add("@PostId", info.PostId, DbType.String);
            parameters.Add("@Describe", info.Describe, DbType.String);

            //會寫入其他table的資料
            parameters.Add("@PhotoIds", info.PhotoIds, DbType.String);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostReportCreate]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 新增/修改評論單
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<DBResult> CommentUpsert(CommentUpsertData info)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CommentId", info.CommentId, DbType.String);
            parameters.Add("@PostId", info.PostId, DbType.String);
            parameters.Add("@AvatarUrl", info.AvatarUrl, DbType.String);
            parameters.Add("@UserId", info.UserId, DbType.Int32);
            parameters.Add("@Nickname", info.Nickname, DbType.String);
            parameters.Add("@AreaCode", info.AreaCode, DbType.String);
            parameters.Add("@Comment", info.Comment, DbType.String);
            parameters.Add("@SpentTime", info.SpentTime, DbType.DateTime);

            //會寫入其他table的資料
            parameters.Add("@PhotoIds", info.PhotoIds, DbType.String);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostCommentUpsert]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 取得信息類型相關的贴子Id
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<string[]> GetPostIdByMessageId(PostType postType, int messageId)
        {
            return (await ReadDb.QueryTable<MMPost>()
                .Where(x => x.MessageId == messageId).QueryAsync())
                .Select(x => x.PostId)
                .ToArray();
        }

        /// <summary>
        /// 取得解鎖的贴子Id
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<string[]> GetPostIdByUnlock(int userId)
        {
            return (await ReadDb.QueryTable<MMPostTransactionModel>()
                .Where(x => x.UserId == userId).QueryAsync())
                .Select(x => x.PostId)
                .ToArray();
        }

        /// <inheritdoc/>
        public async Task<int> GetPostCount(int userId, ReviewStatus status)
        {
            var byteStatus = status;
            return await ReadDb.QueryTable<MMPost>()
                .Where(e => e.UserId == userId && e.Status == byteStatus)
                .CountAsync();
        }

        /// <summary>
        /// 查询用户当日发帖数是否大于0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetPostDailyCount(int userId)
        {
            DateTime startOfDay = DateTime.Today; // 今天的零点
            var now = DateTime.Now;
            var next = now.AddDays(1).Date;

            var ts = next - now;

            var vipPostDay = await _cache.GetOrSetAsync(
                     string.Format($"{_cacheKey}:{userId}"),
                     ts,
                     async () =>
                     {
                         return await ReadDb.QueryTable<MMPost>()
                        .Where(e => e.UserId == userId &&
                            e.CreateTime >= startOfDay && e.CreateTime < next)
                            .CountAsync();
                     }, _cacheIndexes);
            return vipPostDay;
        }

        /// <summary>
        /// 取得官方帖發佈上架數量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> GetOfficialPostCount(int userId, ReviewStatus status)
        {
            var byteStatus = status;
            return await ReadDb.QueryTable<MMOfficialPost>()
                .Where(e => e.UserId == userId && e.Status == byteStatus && e.IsDelete == false)
                .CountAsync();
        }

        public async Task<int> GetPostCountByPostId(string postId)
        {
            return await ReadDb.QueryTable<MMPost>()
            .Where(e => e.PostId == postId)
            .CountAsync();
        }

        /// <summary>
        /// 搜尋資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns></returns>
        public async Task<(MMPost[], int)> PostSearch(PostSearchCondition param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PostType", param.PostType, DbType.Int32);
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@IsRecommend", param.IsRecommend, DbType.Boolean);
            parameters.Add("@AreaCode", param.AreaCode, DbType.String);
            parameters.Add("@MessageId", param.MessageId, DbType.Int32);
            parameters.Add("@SortType", param.SortType, DbType.Int32);
            parameters.Add("@LockStatus", param.LockStatus, DbType.Boolean);

            if (param.Age?.Any() == true)
            {
                parameters.Add("@Age", string.Join(',', param.Age), DbType.String);
            }

            if (param.Height?.Any() == true)
            {
                parameters.Add("@Height", string.Join(',', param.Height), DbType.String);
            }

            if (param.Cup?.Any() == true)
            {
                parameters.Add("@Cup", string.Join(',', param.Cup), DbType.String);
            }

            if (param.Price?.Any() == true)
            {
                parameters.Add("@Price", JsonConvert.SerializeObject(param.Price), DbType.String);
            }

            if (param.ServiceIds?.Any() == true)
            {
                parameters.Add("@ServiceIds", string.Join(',', param.ServiceIds), DbType.String);
            }

            // 先使用容錯判斷，避免上線時同步差
            if (param.QueryTs != null)
            {
                parameters.Add("@QueryTs", param.QueryTs, DbType.DateTime);
            }

            parameters.Add("@PageNo", param.PageNo, DbType.Int32);
            parameters.Add("@PageSize", param.PageSize, DbType.Int32);
            parameters.Add("@RowCount", DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@IsCertified", param.IsCertified, DbType.Boolean);

            int rowCount = 0;
            MMPost[] result = new MMPost[0];
            try
            {
                result = (await WriteDb.QueryAsync<MMPost>(
                    "[dbo].[Pro_MMPostSearch]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure)).ToArray();

                rowCount = parameters.Get<int>("@RowCount");
            }
            catch (Exception ex)
            {
            }

            return (result, rowCount);
        }

        /// <summary>
        /// 從Id取得贴子資訊
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        public async Task<MMPost[]> GetPostInfoById(string[] postIds)
        {
            return (await ReadDb.QueryTable<MMPost>()
                .Where(x => postIds.Contains(x.PostId)).QueryAsync())
                .ToArray();
        }

        public async Task<int> GetPostCountByIdAndStatus(string[] postIds, ReviewStatus status)
        {
            return await ReadDb.QueryTable<MMPost>()
                .Where(x => postIds.Contains(x.PostId) && x.Status == status).CountAsync();
        }

        /// <summary>
        /// 修改官方帖子
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>

        public async Task<bool> EditMMOfficialPost(MMOfficialPost post)
        {
            try
            {
                await WriteDb.Update<MMOfficialPost>(post).SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取得贴子相關的服務
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        public async Task<MMPostServiceMapping[]> GetPostMappingService(string[] postIds)
        {
            return (await ReadDb.QueryTable<MMPostServiceMapping>()
                .Where(x => postIds.Contains(x.PostId)).QueryAsync())
                .ToArray();
        }

        public async Task<IEnumerable<QueryPostTypeCount>> QueryPostCountSummary(int userId)
        {
            string querySql = @"
SELECT [PostType],[Status], COUNT(1) Count
FROM [Inlodb].[dbo].[MMPost] WITH(NOLOCK)
WHERE UserId = @UserId
GROUP BY [PostType],[Status]
UNION ALL
SELECT 3 [PostType],[Status], COUNT(1) Count
FROM [Inlodb].[dbo].[MMOfficialPost] WITH(NOLOCK)
WHERE UserId = @UserId
GROUP BY [Status]
";
            return await ReadDb.QueryAsync<QueryPostTypeCount>(querySql, new { UserId = userId });
        }

        public async Task<IEnumerable<QueryCommentPostTypeCount>> QueryCommentCountSummary(int userId)
        {
            string querySql = @"
SELECT [PostType],[Status], COUNT(1) Count
FROM [Inlodb].[dbo].[MMPostComment] WITH(NOLOCK)
WHERE UserId = @UserId
GROUP BY [PostType],[Status]
UNION ALL
SELECT 3 [PostType],[Status], COUNT(1) Count
FROM [Inlodb].[dbo].[MMOfficialPostComment] WITH(NOLOCK)
WHERE UserId = @UserId
GROUP BY [Status]
";
            return await ReadDb.QueryAsync<QueryCommentPostTypeCount>(querySql, new { UserId = userId });
        }

        private DapperQueryComponent<MMPost> GetComponentByFilter(PostFilter filter)
        {
            var component = ReadDb.QueryTable<MMPost>();
            if (filter == null)
            {
                return component;
            }

            if (filter.UserIds.Any())
            {
                component.Where(e => filter.UserIds.Contains(e.UserId));
            }

            if (filter.Status.HasValue)
            {
                component.Where(e => e.Status == filter.Status);
            }

            return component;
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMPost>> PostSearch(MyPostQueryParam param)
        {
            var type = param.PostType;
            var component = WriteDb.QueryTable<MMPost>()
                .Where(x => x.UserId == param.UserId
                        && x.PostType == type);

            if (param.PostStatus.HasValue)
            {
                var status = param.PostStatus;
                component.Where(x => x.Status == status);
            }

            switch (param.SortType)
            {
                case MyPostSortType.CreateDateAsc:
                    component.OrderBy(x => x.UpdateTime);
                    break;

                case MyPostSortType.CreateDateDesc:
                    component.OrderByDescending(x => x.UpdateTime);
                    break;

                case MyPostSortType.UolockCountAsc:
                    component.OrderBy(x => x.UnlockCount);
                    break;

                case MyPostSortType.UolockCountDesc:
                    component.OrderByDescending(x => x.UnlockCount);
                    break;
            }

            return await component.QueryPageResultAsync(param);
        }

        /// <summary>
        /// 获取我的店铺收藏信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MyFavoriteOfficialShop>> GetMyFavoriteBossShop(MyFavoriteBossShopQueryParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@PageNo", param.PageNo, DbType.Int32);
            parameters.Add("@PageSize", param.PageSize, DbType.Int32);
            parameters.Add("@RowCount", DbType.Int32, direction: ParameterDirection.Output);

            int rowCount = 0;
            MyFavoriteOfficialShop[] result = new MyFavoriteOfficialShop[0];
            try
            {
                result = (await WriteDb.QueryAsync<MyFavoriteOfficialShop>(
                    "[dbo].[Pro_MMUserCollectShop]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure)).ToArray();

                rowCount = parameters.Get<int>("@RowCount");

                return new PageResultModel<MyFavoriteOfficialShop>()
                {
                    Data = result,
                    Page = param.Page,
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    TotalCount = rowCount,
                    TotalPage = (int)Math.Ceiling((decimal)rowCount / param.PageSize),
                };
            }
            catch (Exception ex)
            {
            }

            return new PageResultModel<MyFavoriteOfficialShop>()
            {
                Data = result,
            };
        }

        /// <summary>
        /// 获取我收藏的帖子信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MyFavoritePost>> GetMyFavoritePost(MyFavoritePostQueryParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@PostType", param.PostType, DbType.Int32);
            parameters.Add("@PageNo", param.PageNo, DbType.Int32);
            parameters.Add("@PageSize", param.PageSize, DbType.Int32);
            parameters.Add("@RowCount", DbType.Int32, direction: ParameterDirection.Output);

            int rowCount = 0;
            MyFavoritePost[] result = new MyFavoritePost[0];
            try
            {
                result = (await WriteDb.QueryAsync<MyFavoritePost>(
                    "[dbo].[Pro_MMUserCollectPost]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure)).ToArray();

                rowCount = parameters.Get<int>("@RowCount");

                return new PageResultModel<MyFavoritePost>()
                {
                    Data = result,
                    Page = param.Page,
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    TotalCount = rowCount,
                    TotalPage = (int)Math.Ceiling((decimal)rowCount / param.PageSize),
                };
            }
            catch (Exception ex)
            {
            }

            return new PageResultModel<MyFavoritePost>()
            {
                Data = result,
            };
        }

        public async Task<PageResultModel<UnlockPostList>> GetMyUnlockPost(MyUnlockQueryParam param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@PostStatus", param.Status, DbType.Boolean);
            parameters.Add("@PostType", param.PostType, DbType.String);
            parameters.Add("@PageNo", param.PageNo, DbType.Int32);
            parameters.Add("@PageSize", param.PageSize, DbType.Int32);
            parameters.Add("@RowCount", DbType.Int32, direction: ParameterDirection.Output);

            int rowCount = 0;
            UnlockPostList[] result = new UnlockPostList[0];
            try
            {
                result = (await WriteDb.QueryAsync<UnlockPostList>(
                    "[dbo].[Pro_MMUnlockPost]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure)).ToArray();

                rowCount = parameters.Get<int>("@RowCount");

                return new PageResultModel<UnlockPostList>()
                {
                    Data = result,
                    Page = param.Page,
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    TotalCount = rowCount,
                    TotalPage = (int)Math.Ceiling((decimal)rowCount / param.PageSize),
                };
            }
            catch (Exception ex)
            {
            }

            return new PageResultModel<UnlockPostList>()
            {
                Data = result,
            };
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMPost>> PostSearch(MyUnlockQueryParam param)
        {
            var unlockTrans = (await _trans.GetExpensePosts(param.UserId)).ToArray();
            if (unlockTrans.Length > 0)
            {
                var type = param.PostType;
                return await ReadDb.QueryTable<MMPost>()
                    .Where(x => x.PostType == type && x.Status == param.Status
                            && unlockTrans.Contains(x.PostId))
                    .OrderByDescending(x => x.CreateTime) // 最新的在最上面
                    .QueryPageResultAsync(param);
            }
            return new PageResultModel<MMPost>()
            {
                Data = new MMPost[0],
            };
        }

        /// <summary>
        /// 取得贴子相關的聯絡方式
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        public async Task<MMPostContact[]> GetPostContact(string[] postIds)
        {
            return (await ReadDb.QueryTable<MMPostContact>()
                .Where(x => postIds.Contains(x.PostId)).QueryAsync())
                .ToArray();
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMPost>> PostSearch(AdminPostListParam param)
        {
            var component = ReadDb.QueryTable<MMPost>();
            var begin = param.BeginDate;
            var end = param.EndDate;
            switch (param.DateTimeType)
            {
                case 0:
                    component.Where(x => x.CreateTime >= begin && x.CreateTime < end);
                    break;

                case 1:
                    component.Where(x => x.UpdateTime >= begin && x.UpdateTime < end);
                    break;

                case 2:
                    component.Where(x => x.ExamineTime >= begin && x.ExamineTime < end);
                    break;

                default:
                    return new PageResultModel<MMPost>()
                    {
                        Data = new MMPost[0]
                    };
            }
            if (string.IsNullOrEmpty(param.PostId) == false)
            {
                component.Where(x => x.PostId == param.PostId);
            }
            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }
            if (param.Status.HasValue)
            {
                component.Where(x => x.Status == param.Status);
            }
            if (param.PostType.HasValue)
            {
                component.Where(x => x.PostType == param.PostType);
            }
            if (!string.IsNullOrEmpty(param.Title))
            {
                component.Where(x => x.Title.Contains(param.Title));
            }
            switch (param.SortType)
            {
                default:
                    component.OrderByDescending(x => x.CreateTime);
                    break;
            }
            return await component.QueryPageResultAsync(param);
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMOfficialPost>> OfficialAdminPostSearch(AdminPostListParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@BeginDate", param.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", param.EndDate, DbType.String);
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@PostId", param.PostId, DbType.String);
     
            parameters.Add("@Status", param.Status, DbType.Int32);
            parameters.Add("@UserIdentity", param.UserIdentity, DbType.Int32);
            parameters.Add("@BeginDate", param.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", param.EndDate, DbType.DateTime);

            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string sqlStr = @"
                                    DROP TABLE IF EXISTS #TPM_MMOfficialPost
                                    SELECT 
                                        MP.[PostId], MP.[CoverUrl], MP.[UserId], MP.[Nickname], MP.[Status], MP.[AppointmentCount],
                                        MP.[Title], MP.[AreaCode], MP.[Age], MP.[Height], MP.[Cup], MP.[BusinessHours], MP.[LowPrice],
                                        MP.[HighPrice], MP.[Address], MP.[ServiceDescribe], MP.[Favorites], MP.[Comments], MP.[TotalFacialScore],
                                        MP.[TotalServiceQuality], MP.[FacialScore], MP.[CreateTime], MP.[UpdateTime], MP.[ExamineMan], MP.[ExamineTime],
                                        MP.[Memo], MP.[OldViewData], MP.[IsDelete], MP.[LockStatus], MP.[ViewBaseCount], MP.[PostCount], MP.[Views] 
                                    INTO #TPM_MMOfficialPost FROM MMOfficialPost AS MP WITH(NOLOCK) INNER JOIN MMUserInfo AS MU WITH(NOLOCK) ON MP.UserId=MU.UserId WHERE  ";

            var component = ReadDb.QueryTable<MMOfficialPost>();
            var begin = param.BeginDate;
            var end = param.EndDate;
            switch (param.DateTimeType)
            {
                case 0:
                    //component.Where(x => x.CreateTime >= begin && x.CreateTime < end);
                    sqlStr += @" MP.CreateTime >=@BeginDate AND  MP.CreateTime < @EndDate ";

                    break;

                case 1:
                    //component.Where(x => x.UpdateTime >= begin && x.UpdateTime < end && x.CreateTime != x.UpdateTime);
                    sqlStr += @" MP.UpdateTime >=@BeginDate AND  MP.UpdateTime < @EndDate AND MP.CreateTime != MP.UpdateTime ";
                    break;

                case 2:
                    //component.Where(x => x.ExamineTime >= begin && x.ExamineTime < end);
                    sqlStr += @" MP.ExamineTime >=@BeginDate AND  MP.ExamineTime < @EndDate ";
                    break;

                default:
                    sqlStr += @" MP.CreateTime >=@BeginDate AND  MP.CreateTime < @EndDate ";
                    break;
            }


            if (string.IsNullOrEmpty(param.PostId) == false)
            {
                //component.Where(x => x.PostId == param.PostId);
                sqlStr += " AND  MP.PostId=@PostId ";
            }
            if (param.UserId.HasValue)
            {
                // component.Where(x => x.UserId == param.UserId);
                sqlStr += " AND  MP.UserId=@UserId ";
            }
            if (!string.IsNullOrEmpty(param.Title))
            {
                //component.Where(x => x.Title.Contains(param.Title));
                sqlStr += $" AND  MP.Title LIKE '%{param.Title}%' ";
            }
            if (param.Status.HasValue)
            {
                //component.Where(x => x.Status == param.Status);
                sqlStr += " AND  MP.Status=@Status ";
            }
            if (param.UserIdentity.HasValue)
            {
                sqlStr += " AND  MU.UserIdentity=@UserIdentity ";
            }

            sqlStr += " ORDER BY MP.CreateTime DESC ";

            sqlStr += @"
                SET @TotalCount=(SELECT COUNT(*) FROM #TPM_MMOfficialPost);
                    --分页
                SELECT
                    *
                FROM(
                    SELECT *,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNumber
                    FROM  #TPM_MMOfficialPost
                ) AS T
                WHERE T.RowNumber BETWEEN @StartNo AND @EndNo
            ";

            var result = await ReadDb.QueryAsync<MMOfficialPost>(sqlStr, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<MMOfficialPost>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }

        public async Task<PageResultModel<MMOfficialPost>> OfficialPostList(ReqOfficialStorePost param)
        {
            var component = ReadDb.QueryTable<MMOfficialPost>();
            component.Where(c => c.IsDelete == false && c.Status == ReviewStatus.Approval);
            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }
            if (param.AreaCode.Any())
            {
                component.Where(x => param.AreaCode.Contains(x.AreaCode));
            }

            component.Where(x => x.Status == ReviewStatus.Approval);
            component.OrderByDescending(x => x.PostId);

            return await component.QueryPageResultAsync(param);
        }

        public async Task<PageResultModel<MMOfficialPost>> MyOfficialPostList(ReqMyOfficialStorePost param)
        {
            var component = WriteDb.QueryTable<MMOfficialPost>();
            component.Where(c => c.IsDelete == false && c.Status == ReviewStatus.Approval);
            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }

            if (param.AreaCode.Any())
            {
                component = component.Where(x => param.AreaCode.Contains(x.AreaCode));
            }
            component.OrderByDescending(x => x.CreateTime);

            return await component.QueryPageResultAsync(param);
        }

        /// <summary>
        /// 取得贴子評論從id
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <returns></returns>
        public async Task<MMPostComment?> GetPostCommentByCommentId(string commentId)
        {
            return (await ReadDb.QueryTable<MMPostComment>()
                .Where(x => x.CommentId == commentId).QueryAsync())
                .FirstOrDefault();
        }

        /// <summary>
        /// 取得贴子評論從PostId and UserId
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<MMPostComment?> GetPostCommentByPostIdAndUserId(string postId, int userId)
        {
            return (await ReadDb.QueryTable<MMPostComment>()
               .Where(x => x.PostId == postId && x.UserId == userId).QueryAsync())
               .FirstOrDefault();
        }

        /// <summary>
        /// 取得用戶評論的贴子
        /// </summary>
        /// <param name="postIds">贴子id list</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<MMPostComment[]> GetPostCommentByUser(string[] postIds, int userId)
        {
            return (await ReadDb.QueryTable<MMPostComment>()
                .Where(x => postIds.Contains(x.PostId) && x.UserId == userId).QueryAsync())?
                .ToArray() ?? Array.Empty<MMPostComment>();
        }

        /// <summary>
        /// 取得贴子評論列表
        /// </summary>
        /// <param name="postId">贴id</param>
        /// <param name="pageSetting">分頁設定</param>
        /// <returns></returns>
        public async Task<PageResultModel<MMPostComment>> GetPostComment(string postId, PaginationModel pageSetting)
        {
            return await ReadDb.QueryTable<MMPostComment>()
                .Where(x => x.PostId == postId && x.Status == (int)CommentStatus.Approval)
                .OrderByDescending(x => x.ExamineTime)  //使用審核通過時間
                .QueryPageResultAsync(pageSetting);
        }

        /// <summary>
        /// 檢用使用者是否已評論
        /// </summary>
        /// <param name="postId">贴id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<bool> CheckUserAlreadyComment(string postId, int userId)
        {
            return (await ReadDb.QueryTable<MMPostComment>()
               .Where(x => x.PostId == postId && x.UserId == userId).QueryAsync())?
               .Any() == true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<DBResult> PostEdit(AdminPostData param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PostId", param.PostId, DbType.String);
            parameters.Add("@ExamineMan", param.ExamineMan, DbType.String);
            parameters.Add("@IsApplyAdjustPrice", param.IsApplyAdjustPrice, DbType.Boolean);
            parameters.Add("@IsFeatured", param.IsFeatured, DbType.Boolean);
            parameters.Add("@PostStatus", (int)param.PostStatus, DbType.Int32);
            parameters.Add("@Memo", param.Memo, DbType.String);
            parameters.Add("@Title", param.Title, DbType.String);
            parameters.Add("@BusinessHours", param.BusinessHours, DbType.String);
            parameters.Add("@Address", param.Address, DbType.String);
            parameters.Add("@ContactInfos", param.ContactInfos, DbType.String);
            parameters.Add("@ServiceDescribe", param.ServiceDescribe, DbType.String);
            parameters.Add("@IsHomePost", param.IsHomePost, DbType.Boolean);
            parameters.Add("@Weight", param.Weight, DbType.Int32);
            parameters.Add("@UnlockBaseCount", param.UnlockBaseCount, DbType.Int32);
            parameters.Add("@ViewBaseCount", param.ViewBaseCount, DbType.Int32);
            parameters.Add("@IsCertified", param.IsCertified, DbType.Boolean);
            var result = new DBResult(ReturnCode.SystemError);
            try   
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostAdminUpdate]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(PostEdit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public async Task<DBResult> PostEditAllData(AdminPostData param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PostId", param.PostId, DbType.String);
            parameters.Add("@ExamineMan", param.ExamineMan, DbType.String);
            parameters.Add("@IsApplyAdjustPrice", param.IsApplyAdjustPrice, DbType.Boolean);
            parameters.Add("@Title", param.Title, DbType.String);
            parameters.Add("@BusinessHours", param.BusinessHours, DbType.String);
            parameters.Add("@Address", param.Address, DbType.String);
            parameters.Add("@ContactInfos", param.ContactInfos, DbType.String);
            parameters.Add("@ServiceDescribe", param.ServiceDescribe, DbType.String);
            parameters.Add("@PostType", param.PostType, DbType.Int32);
            parameters.Add("@MessageId", param.MessageId, DbType.Int32);
            parameters.Add("@ApplyAmount", param.ApplyAmount, DbType.Decimal);
            parameters.Add("@AreaCode", param.AreaCode, DbType.String);
            parameters.Add("@Quantity", param.Quantity, DbType.String);
            parameters.Add("@Age", param.Age, DbType.Int32);
            parameters.Add("@Height", param.Height, DbType.Int32);
            parameters.Add("@Cup", param.Cup, DbType.Int32);
            parameters.Add("@LowPrice", param.LowPrice, DbType.Decimal);
            parameters.Add("@HighPrice", param.HighPrice, DbType.Decimal);
            parameters.Add("@ServiceIds", param.ServiceIds, DbType.String);
            parameters.Add("@PhotoIds", param.PhotoIds, DbType.String);
            parameters.Add("@VideoIds", param.VideoIds, DbType.String);
            parameters.Add("@IsCertified", param.IsCertified, DbType.Boolean);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostAdminEdit]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<DBResult> PostBatchEdit(AdminPostBatchData param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostAdminBatchReview]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(PostEdit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(PostEdit)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMPostComment>> CommentSearch(AdminCommentListParam param)
        {
            var component = GetByParam(param);

            var begin = param.BeginDate;
            var end = param.EndDate;
            switch (param.DateTimeType)
            {
                case 1:
                    component.Where(x => x.CreateTime >= begin && x.CreateTime < end);
                    break;

                case 2:
                    component.Where(x => x.UpdateTime >= begin && x.UpdateTime < end);
                    break;

                case 3:
                    component.Where(x => x.ExamineTime >= begin && x.ExamineTime < end);
                    break;

                case 4:
                    component.Where(x => x.SpentTime >= begin && x.SpentTime < end);
                    break;

                default:
                    throw new MMException(ReturnCode.ParameterIsInvalid);
                    break;
            }

            return await component.OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMOfficialPostComment>> OfficialCommentSearch(AdminCommentListParam param)
        {
            var component = GetByOfficialParam(param);

            var begin = param.BeginDate;
            var end = param.EndDate;
            switch (param.DateTimeType)
            {
                case 0:
                    component.Where(x => x.CreateTime >= begin && x.CreateTime < end);
                    break;

                case 1:
                    component.Where(x => x.UpdateTime >= begin && x.UpdateTime < end);
                    break;

                case 2:
                    component.Where(x => x.ExamineTime >= begin && x.ExamineTime < end);
                    break;

                default:
                    throw new MMException(ReturnCode.ParameterIsInvalid);
                    break;
            }

            return await component.OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        private DapperQueryComponent<MMPostComment> GetByParam(AdminCommentListParam param)
        {
            var component = ReadDb.QueryTable<MMPostComment>();

            if (!string.IsNullOrEmpty(param.Id))
            {
                var id = param.Id;
                component.Where(x => x.CommentId == id);
            }

            if (!string.IsNullOrEmpty(param.PostId))
            {
                var id = param.PostId;
                component.Where(x => x.PostId == id);
            }

            if (param.UserId.HasValue)
            {
                var id = param.UserId;
                component.Where(x => x.UserId == id);
            }

            if (param.PostType.HasValue)
            {
                var type = (byte)param.PostType;
                component.Where(x => x.PostType == type);
            }

            if (param.Status.HasValue)
            {
                var type = (byte)param.Status;
                component.Where(x => x.Status == type);
            }

            return component;
        }

        private DapperQueryComponent<MMOfficialPostComment> GetByOfficialParam(AdminCommentListParam param)
        {
            var component = ReadDb.QueryTable<MMOfficialPostComment>();

            if (!string.IsNullOrEmpty(param.Id))
            {
                var id = param.Id;
                component.Where(x => x.CommentId == id);
            }

            if (!string.IsNullOrEmpty(param.PostId))
            {
                var id = param.PostId;
                component.Where(x => x.PostId == id);
            }

            if (param.UserId.HasValue)
            {
                var id = param.UserId;
                component.Where(x => x.UserId == id);
            }

            if (param.Status.HasValue)
            {
                var type = (byte)param.Status;
                component.Where(x => x.Status == type);
            }

            return component;
        }

        /// <inheritdoc/>
        public async Task<MMPostComment?> CommentDetail(string commentId)
        {
            return (await ReadDb.QueryTable<MMPostComment>()
                .Where(x => x.CommentId == commentId)
                .QueryAsync())
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<MMOfficialPostComment?> OfficialCommentDetail(string commentId)
        {
            return (await ReadDb.QueryTable<MMOfficialPostComment>()
                .Where(x => x.CommentId == commentId)
                .QueryAsync())
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<DBResult> CommentEdit(AdminCommentData param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostCommentAdminUpdate]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(CommentEdit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(CommentEdit)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        /// <inheritdoc/>
        public async Task<DBResult> OfficialCommentEdit(AdminCommentData param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMOfficialPostCommentAdminUpdate]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(CommentEdit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(CommentEdit)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        public async Task<PageResultModel<MMPostReport>> ReportMessage(ReportMessageParam messageParam)
        {
            var component = WriteDb.QueryTable<MMPostReport>();
            component = component.Where(x => x.Memo != null && x.Memo != "" && x.Status == 2 && x.ComplainantUserId == messageParam.UserId);
            if (messageParam.DeleteMessageIds.IsNotEmpty())
            {
                foreach (var item in messageParam.DeleteMessageIds)
                {
                    component = component.Where(c => c.ReportId != item);
                }
            }
            return await component.OrderByDescending(x => x.ExamineTime)
                .QueryPageResultAsync(messageParam);
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMPostReport>> ReportSearch(AdminReportListParam param)
        {
            var component = ReadDb.QueryTable<MMPostReport>();

            if (!string.IsNullOrEmpty(param.Id))
            {
                var id = param.Id;
                component.Where(e => e.ReportId == id);
            }

            if (!string.IsNullOrEmpty(param.PostId))
            {
                var id = param.PostId;
                component.Where(e => e.PostId == id);
            }

            if (param.UserId.HasValue)
            {
                var id = param.UserId;
                component.Where(e => e.ComplainantUserId == id);
            }

            if (param.PostType.HasValue)
            {
                var type = (byte)param.PostType;
                component.Where(e => e.PostType == type);
            }
            else
            {
                component.Where(e => e.PostType == 1 || e.PostType == 2);
            }
            if (param.ReportType.HasValue)
            {
                var type = (byte)param.ReportType;
                component.Where(e => e.ReportType == type);
            }

            if (param.Status.HasValue)
            {
                var type = (byte)param.Status;
                component.Where(e => e.Status == type);
            }
            var begin = param.BeginDate;
            var end = param.EndDate;
            return await component.Where(e => e.CreateTime >= begin && e.CreateTime < end)
                .OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        public async Task<int> GetReportCount(int UserId)
        {
            var component = ReadDb.QueryTable<MMPostReport>();
            return await component.Where(e => e.ComplainantUserId == UserId).CountAsync();
        }

        public async Task<IEnumerable<string>> GetReportAllByUserId(int UserId)
        {
            var component = ReadDb.QueryTable<MMPostReport>();
            return await component.Where(e => e.ComplainantUserId == UserId).QueryAsync().SelectAsync(c => c.ReportId);
        }

        /// <inheritdoc/>
        public async Task<MMPostReport?> ReportDetail(string reportId)
        {
            return (await ReadDb.QueryTable<MMPostReport>()
                .Where(x => x.ReportId == reportId)
                .QueryAsync())
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<List<MMPostReport>?> GetReports(string postId, int userId)
        {
            return (await ReadDb.QueryTable<MMPostReport>()
                .Where(x => x.PostId == postId && x.ComplainantUserId == userId)
                .QueryAsync())
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<DBResult> ReportEdit(AdminReportData param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostReportAdminUpdate]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(ReportEdit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(ReportEdit)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        /// <inheritdoc/>
        public async Task<DBResult> OfficialReportEdit(AdminReportData param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMOfficialPostReportAdminUpdate]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(ReportEdit)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(ReportEdit)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        /// <summary>
        /// 更新贴子的觀看次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        public async Task<bool> UpdatePostViewsCount(string postId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@PostId", postId, DbType.String, size: 32);

                string sqlCmd = $@"UPDATE MMPost SET Views = Views + 1 WHERE PostId = @PostId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新官方贴子的觀看次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        public async Task<bool> UpdateOfficialPostViewsCount(string postId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@PostId", postId, DbType.String, size: 32);

                string sqlCmd = $@"UPDATE MMOfficialPost SET Views = Views + 1 WHERE PostId = @PostId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<MMPost> GetPostStatusByPostId(string postId)
        {
            try
            {
                var paramter = new DynamicParameters();
                paramter.Add("@PostId", postId);
                string sql = "SELECT PostId,Status FROM MMPost WITH(NOLOCK) WHERE PostId=@PostId";
                return await ReadDb.QueryAsync<MMPost>(sql, paramter)?.FirstAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<MMOfficialPost> GetOfficialPostStatusByPostId(string postId)
        {
            try
            {
                var paramter = new DynamicParameters();
                paramter.Add("@PostId", postId);
                string sql = "SELECT PostId,Status,IsDelete FROM MMOfficialPost WITH(NOLOCK) WHERE PostId=@PostId";
                return await ReadDb.QueryAsync<MMOfficialPost>(sql, paramter)?.FirstAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新贴子的評論次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="isAdd">true=增加，false=減少</param>
        /// <returns></returns>
        public async Task<bool> UpdatePostCommentsCount(string postId, bool isAdd)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@PostId", postId, DbType.String, size: 32);

                string sqlCmd = $@"UPDATE MMPost SET Comments = Comments {(isAdd ? "+" : "-")} 1 WHERE PostId = @PostId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新官方贴子的評論次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="facialScore">該評論顏值</param>
        /// <param name="serviceQuality">該評論服務品質</param>
        /// <param name="isAdd">true=增加，false=減少</param>
        /// <returns></returns>
        public async Task<bool> UpdateOfficialPostCommentsCount(string postId, decimal facialScore, decimal serviceQuality, bool isAdd)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postId))
                {
                    return false;
                }

                var parameters = new DynamicParameters();
                parameters.Add("@PostId", postId, DbType.String, size: 32);
                parameters.Add("@FacialScore", facialScore, DbType.Decimal);
                parameters.Add("@ServiceQuality", serviceQuality, DbType.Decimal);

                string sqlCmd = $@"
                    UPDATE MMOfficialPost
                    SET TotalFacialScore = TotalFacialScore {(isAdd ? "+" : "-")} @FacialScore,
                        TotalServiceQuality = TotalServiceQuality {(isAdd ? "+" : "-")} @ServiceQuality,
                        Comments = Comments {(isAdd ? "+" : "-")} 1
                    WHERE PostId = @PostId;";

                await WriteDb.AddExecuteSQL(sqlCmd, parameters).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<MMPostReport[]> GetReports(string[] reportIds)
        {
            return (await ReadDb.QueryTable<MMPostReport>()
                .Where(x => reportIds.Contains(x.ReportId))
                .QueryAsync()).ToArray();
        }

        /// <summary>
        /// 贴子收藏新增/刪除
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="postId">贴子id</param>
        /// <param name="type">类型。1：帖子、2：店铺</param>
        /// <returns></returns>
        public async Task<DBResult> PostFavoriteUpsert(int userId, string postId, int type)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.Int32);
            parameters.Add("@PostId", postId, DbType.String);
            parameters.Add("@Type", type, DbType.Int32);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostFavoriteUpsert]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 用戶是否有舉報過
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        public async Task<bool> UserHasReported(int userId, string postId)
        {
            return (await ReadDb.QueryTable<MMPostReport>()
                .Where(x => x.PostId == postId && x.ComplainantUserId == userId).QueryAsync())?
                .Any() == true;
        }

        /// <summary>
        /// 取得用戶對於該贴的投訴
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="postId">贴子id</param>
        /// <param name="postType">贴子類型</param>
        /// <returns></returns>
        public async Task<MMPostReport[]> GetUserReported(int userId, string postId, PostType? postType)
        {
            var result = await ReadDb.QueryTable<MMPostReport>()
                .Where(x => x.PostId == postId && x.ComplainantUserId == userId)
                .QueryAsync();

            if (postType != null && result?.Any() == true)
            {
                result = result.Where(p => p.PostType == (byte)postType);
            }

            return result?.ToArray() ?? Array.Empty<MMPostReport>();
        }

        public async Task<IEnumerable<PostWeightResult>> GetMMPostWeight()
        {
            string querySql = @"
SELECT Id
    ,PostId
    ,Weight
    ,Operator
    ,CreateTime
    ,UpdateTime
    ,Status
    ,PostType
FROM (
    SELECT mpw.Id
        ,mpw.PostId
        ,mpw.Weight
        ,mpw.Operator
        ,mpw.CreateTime
        ,mpw.UpdateTime
        ,mp.Status
        ,mp.PostType
    FROM [Inlodb].[dbo].[MMPostWeight] mpw WITH(NOLOCK)
    INNER JOIN [Inlodb].[dbo].[MMPost] mp WITH(NOLOCK)
    ON mpw.PostId=mp.PostId

    UNION

    SELECT mpw.Id
        ,mpw.PostId
        ,mpw.Weight
        ,mpw.Operator
        ,mpw.CreateTime
        ,mpw.UpdateTime
        ,mp.Status
        ,PostType = 3
    FROM [Inlodb].[dbo].[MMPostWeight] mpw WITH(NOLOCK)
    INNER JOIN [Inlodb].[dbo].[MMOfficialPost] mp WITH(NOLOCK)
    ON mpw.PostId=mp.PostId
) AS CombinedResults
ORDER BY Weight DESC, CreateTime DESC;
";

            return await ReadDb.QueryAsync<PostWeightResult>(querySql);
        }

        public async Task<IEnumerable<GoldStoreResult>> GetMMGoldStoreIsOpen()
        {
            string querySql = @"SELECT MG.Id, MG.[Top], MG.UserId, MG.CreateTime, MG.UpdateTime, MG.Operator, MG.[Type] FROM MMGoldStore MG WITH(NOLOCK) LEFT JOIN MMUserInfo MU WITH(NOLOCK) ON MG.UserId = MU.UserId WHERE MU.IsOpen = 1;";

            return await ReadDb.QueryAsync<GoldStoreResult>(querySql);
        }

        public async Task<IEnumerable<GoldStoreResult>> GetMMGoldStoreAll()
        {
            string querySql = @"SELECT Id, [Top], UserId, CreateTime, UpdateTime, Operator, [Type] FROM MMGoldStore;";

            return await ReadDb.QueryAsync<GoldStoreResult>(querySql);
        }

        public async Task<IEnumerable<PostWeightResult>> GetMMOfficialPostWeight()
        {
            string querySql = @"
  SELECT mpw.Id
      ,mpw.PostId
      ,mpw.Weight
      ,mpw.Operator
      ,mpw.CreateTime
      ,mpw.UpdateTime
	  ,mp.Status
      ,PostType = 3
  FROM [Inlodb].[dbo].[MMPostWeight] mpw
  INNER JOIN [Inlodb].[dbo].[MMOfficialPost] mp
  ON mpw.PostId=mp.PostId
  ORDER BY mpw.Weight DESC,mpw.CreateTime DESC
";

            return await ReadDb.QueryAsync<PostWeightResult>(querySql);
        }

        /// <summary>
        /// 获取我的分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MMPostFavorite>> GetMyFavoritePageAsync(FavoriteListParam param, string[]? postIds)
        {
            var component = WriteDb.QueryTable<MMPostFavorite>();
            if (param.UserId.HasValue)
            {
                component.Where(c => c.UserId == param.UserId);
            }
            if (param.FavoriteType.HasValue)
            {
                component.Where(c => c.Type == param.FavoriteType);
            }
            if (postIds != null && postIds.Length > 0)
            {
                component.Where(c => postIds.Contains(c.PostId));
            }

            return await component.OrderByDescending(x => x.CreateTime).QueryPageResultAsync(param);
        }

        public async Task<IEnumerable<MMPostFavorite>> GetAllMyFavorite(FavoriteListParam param)
        {
            var component = ReadDb.QueryTable<MMPostFavorite>();
            if (param.UserId.HasValue)
            {
                component.Where(c => c.UserId == param.UserId);
            }
            if (param.FavoriteType.HasValue)
            {
                component.Where(c => c.Type == param.FavoriteType);
            }

            return await component.QueryAsync();
        }

        public async Task<int> GetMyFavoritePostCount(int userId, int PostType)
        {
            string sql = @" SELECT COUNT(1) FROM MMPostFavorite AS PF WITH(NOLOCK)
                            INNER JOIN [dbo].[MMPost] AS MP WITH(NOLOCK) ON PF.PostId=MP.PostId
                            WHERE PF.[Type]=1 AND PF.[UserId]=@UserId AND MP.[Status]=1 AND MP.[PostType]=@PostType ";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@PostType", PostType);
            return await WriteDb.QueryScalarAsync<int>(sql, parameters);
        }

        public async Task<int> GetMyFavoriteShopCount(int userId)
        {
            string sql = @" SELECT COUNT(1) FROM MMPostFavorite AS PF  WITH(NOLOCK)
                            INNER JOIN [dbo].[MMIdentityApply] AS MA  WITH(NOLOCK) ON PF.PostId=MA.ApplyId
                            INNER JOIN [dbo].[MMUserInfo] AS MU  WITH(NOLOCK) ON MA.UserId=MU.UserId
                            WHERE PF.[Type]=2 AND MU.UserIdentity IN (3, 5) AND PF.UserId=@UserId ";
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            return await WriteDb.QueryScalarAsync<int>(sql, parameters);
        }

        public Task<IEnumerable<MMPostFavorite>> GetMyFavoriteByUserId(int userId, int type)
        {
            return WriteDb.QueryTable<MMPostFavorite>().Where(c => c.UserId == userId && c.Type == type).QueryAsync();
        }

        public async Task<IEnumerable<MMPostFavorite>> GetMMPostFavorite(string? postId)
        {
            string querySql = $"SELECT [FavoriteId], [PostId], [UserId], [CreateTime], [Type] FROM MMPostFavorite WHERE PostId = '{postId}';";

            return await ReadDb.QueryAsync<MMPostFavorite>(querySql);
        }

        public async Task<IEnumerable<MMPostFavorite>> GetMMPostFavorite(int? userId, string[] postIds)
        {
            string querySql = $"SELECT [FavoriteId], [PostId], [UserId], [CreateTime], [Type] FROM MMPostFavorite WHERE UserId = {userId} AND PostId IN ('{string.Join("','", postIds)}')";

            return await ReadDb.QueryAsync<MMPostFavorite>(querySql);
        }

        public async Task<bool> CanCelFavorite(string favoriteId)
        {
            try
            {
                var entity = await ReadDb.QueryTable<MMPostFavorite>().Where(c => c.FavoriteId == favoriteId).QueryFirstAsync();
                await WriteDb.Delete<MMPostFavorite>(entity).SaveChangesAsync();
            }
            catch
            {
                Logger.LogError($"取消收藏帖子操作失败,favoriteId:{favoriteId}");
                return false;
            }
            return true;
        }

        public async Task<DBResult> InsertMMPostWeight(MMPostWeight param)
        {
            DBResult result = new DBResult();
            int maxCountByPostType = 50;
            PostType postType;
            string postId = param.PostId;
            var mmPost = GetById(postId);
            //先判斷有沒有在MMPost
            if (mmPost.Result == null)
            {
                //再判斷有沒有在MMOfficialPost
                if (GetOfficialPostById(postId).Result == null)
                {
                    result = new DBResult(ReturnCode.PostIsNotExist);
                    return result;
                }
                else
                {
                    postType = PostType.Official;
                }
            }
            else
            {
                postType = mmPost.Result.PostType;
            }

            int postWeightCount = await GetPostWeightCount(postId);
            if (postWeightCount > 0)
            {
                result = new DBResult(ReturnCode.PostRepeat);
                return result;
            }
            //取得此Id贴子的PostType

            //再去比對有沒有超過此PostType上限
            int postCountByPostType = await GetPostWeightCountByPostType(postType);
            if (postCountByPostType >= maxCountByPostType)
            {
                if (postType == PostType.Square)
                    result = new DBResult(ReturnCode.LimitSquare);
                else if (postType == PostType.Agency)
                    result = new DBResult(ReturnCode.LimitAgency);
                else if (postType == PostType.Official)
                    result = new DBResult(ReturnCode.LimitOfficial);
                else
                    result = new DBResult(ReturnCode.LimitReached);
                return result;
            }

            string sql = @"INSERT INTO MMPostWeight (PostId, Weight, Operator, CreateTime, UpdateTime)
                           VALUES (@PostId, @Weight, @Operator, @CreateTime, @UpdateTime)";
            await WriteDb.AddExecuteSQL(sql, param).SaveChangesAsync();
            result = new DBResult(ReturnCode.Success);
            return result;
        }

        public async Task<DBResult> DeleteMMPostWeight(int id)
        {
            await WriteDb.Delete(new MMPostWeight()
            {
                Id = id,
            }).SaveChangesAsync();
            return new DBResult(ReturnCode.Success);
        }

        public async Task<DBResult> UpdateMMPostWeight(UpdateMMPostParam param)
        {
            DBResult result = new DBResult();
            string sql = @"UPDATE [Inlodb].[dbo].[MMPostWeight]
                           SET [UpdateTime] = GETDATE(),
                           [Weight] = @Weight,
                           [Operator]=@Operator
                           WHERE [Id] = @Id;";
            await WriteDb.AddExecuteSQL(sql, param).SaveChangesAsync();
            result = new DBResult(ReturnCode.Success);
            return result;
        }

        public async Task<DBResult> UpdateMMGoldStore(List<UpdateMMGoldStoreParam> paramList)
        {
            DBResult result = new DBResult();
            string sql = @"UPDATE [Inlodb].[dbo].[MMGoldStore]
                   SET [UpdateTime] = GETDATE(),
                   [UserId] = @UserId,
                   [Operator] = @Operator
                   WHERE [Top] = @Top AND [Type] = @Type ;";

            foreach (var param in paramList)
            {
                await WriteDb.AddExecuteSQL(sql, param).SaveChangesAsync();
            }

            result = new DBResult(ReturnCode.Success);
            return result;
        }

        public async Task<int> GetPostWeightCount(string postId)
        {
            return await ReadDb.QueryTable<MMPostWeight>()
            .Where(e => e.PostId == postId)
            .CountAsync();
        }

        public async Task<int> GetWeightCount(int weight)
        {
            return await ReadDb.QueryTable<MMPostWeight>()
            .Where(e => e.Weight == weight)
            .CountAsync();
        }

        public async Task<int> GetPostWeightCountByPostType(PostType postType)
        {
            var post = await GetMMPostWeight();
            return post.Where(p => p.PostType == (byte)postType).Count();
        }

        /// <inheritdoc/>
        public async Task<DBResult> PostWeightBatchRemove(AdminPostBatchData param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMPostWeightBatchRemove]",
                    paras: new { PostIds = param.PostIds },
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(PostWeightBatchRemove)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(PostWeightBatchRemove)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        /// <summary>
        /// 取得官方單一贴子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<MMOfficialPost?> GetOfficialPostById(string postId)
        {
            return await WriteDb.QueryTable<MMOfficialPost>()
               .Where(e => e.PostId == postId)
               .QueryAsync()
               .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 取得官方贴
        /// </summary>
        /// <param name="postIds"></param>
        /// <returns></returns>
        public async Task<MMOfficialPost[]> GetOfficialPostByIds(string[] postIds)
        {
            return (await ReadDb.QueryTable<MMOfficialPost>()
               .Where(e => postIds.Contains(e.PostId))
               .QueryAsync())?.ToArray() ?? new MMOfficialPost[0];
        }

        /// <summary>
        /// 根据发帖人取得官方贴
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<MMOfficialPost[]> GetOfficialPostByUserId(int userId)
        {
            return (await WriteDb.QueryTable<MMOfficialPost>()
               .Where(e => e.UserId == userId && e.Status == ReviewStatus.Approval && e.IsDelete == false)
               .QueryAsync()).OrderByDescending(e => e.PostId)?.ToArray() ?? new MMOfficialPost[0];
        }

        /// <summary>
        /// 新增或編輯官方贴子
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<DBResult> OfficialPostUpsert(OfficialPostUpsertData info)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PostId", info.PostId, DbType.String);
            parameters.Add("@UserId", info.UserId, DbType.Int32);
            parameters.Add("@Nickname", info.Nickname, DbType.String);
            parameters.Add("@Title", info.Title, DbType.String);
            parameters.Add("@AreaCode", info.AreaCode, DbType.String);
            parameters.Add("@Age", info.Age, DbType.Int32);
            parameters.Add("@Height", info.Height, DbType.Int32);
            parameters.Add("@Cup", info.Cup, DbType.Int32);
            parameters.Add("@BusinessHours", info.BusinessHours, DbType.String);
            parameters.Add("@LowPrice", info.LowPrice, DbType.Decimal);
            parameters.Add("@HighPrice", info.HighPrice, DbType.Decimal);
            parameters.Add("@Address", info.Address, DbType.String);
            parameters.Add("@ServiceDescribe", info.ServiceDescribe, DbType.String);

            //會寫入其他table的資料
            parameters.Add("@ServiceIds", info.ServiceIds, DbType.String);
            parameters.Add("@PhotoIds", info.PhotoIds, DbType.String);
            parameters.Add("@VideoIds", info.VideoIds, DbType.String);
            parameters.Add("@Combo", info.Combo, DbType.String);
            //--

            parameters.Add("@OldViewData", info.OldViewData, DbType.String);

            var result = new DBResult(ReturnCode.SystemError);
            try
            {
                result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMOfficialPostOperate]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// 搜尋官方贴資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns></returns>
        public async Task<(MMOfficialPost[], int)> OfficialPostSearch(PostSearchCondition param)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@IsRecommend", param.IsRecommend, DbType.Boolean);
            parameters.Add("@AreaCode", param.AreaCode, DbType.String);
            parameters.Add("@SortType", param.SortType, DbType.Int32);

            if (param.Age?.Any() == true)
            {
                parameters.Add("@Age", string.Join(',', param.Age), DbType.String);
            }

            if (param.Height?.Any() == true)
            {
                parameters.Add("@Height", string.Join(',', param.Height), DbType.String);
            }

            if (param.Cup?.Any() == true)
            {
                parameters.Add("@Cup", string.Join(',', param.Cup), DbType.String);
            }

            if (param.Price?.Any() == true)
            {
                parameters.Add("@Price", JsonConvert.SerializeObject(param.Price), DbType.String);
            }

            if (param.ServiceIds?.Any() == true)
            {
                parameters.Add("@ServiceIds", string.Join(',', param.ServiceIds), DbType.String);
            }

            if (param.BookingStatus != null)
            {
                parameters.Add("@BookingStatus", param.BookingStatus, DbType.Int32);
            }

            if (param.UserIdentity != null)
            {
                parameters.Add("@UserIdentity", param.UserIdentity, DbType.Int32);
            }

            if (param.QueryTs != null)
            {
                parameters.Add("@QueryTs", param.QueryTs, DbType.DateTime);
            }

            parameters.Add("@PageNo", param.PageNo, DbType.Int32);
            parameters.Add("@PageSize", param.PageSize, DbType.Int32);
            parameters.Add("@RowCount", DbType.Int32, direction: ParameterDirection.Output);

            int rowCount = 0;
            MMOfficialPost[] result = new MMOfficialPost[0];
            try
            {
                result = (await WriteDb.QueryAsync<MMOfficialPost>(
                    "[dbo].[Pro_MMOfficialPostSearch]",
                    parameters,
                    commandTimeout: 30,
                    CommandType.StoredProcedure)).ToArray();

                rowCount = parameters.Get<int>("@RowCount");
            }
            catch (Exception ex)
            {
            }

            return (result, rowCount);
        }

        /// <summary>
        /// 查询我的官方帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<PageResultModel<MMOfficialPost>> OfficialBossPostPageAsync(MyOfficialQueryParam param)
        {
            var component = WriteDb.QueryTable<MMOfficialPost>()
               .Where(x => x.UserId == param.UserId);

            switch (param.PostStatus)
            {
                case MyBossPostStatus.RemovedFromShelves:
                    component.Where(x => x.Status == ReviewStatus.Approval && x.IsDelete == true);
                    break;

                case MyBossPostStatus.OnDisplay:
                    component.Where(x => x.Status == ReviewStatus.Approval && x.IsDelete == false);
                    break;

                case MyBossPostStatus.UnderReview:
                    component.Where(x => x.Status == ReviewStatus.UnderReview);
                    break;

                case MyBossPostStatus.DidNotPass:
                    component.Where(x => x.Status == ReviewStatus.NotApproved);
                    break;
            }

            switch (param.SortType)
            {
                case MyPostSortType.CreateDateAsc:
                    component.OrderBy(x => x.CreateTime);
                    break;

                case MyPostSortType.CreateDateDesc:
                    component.OrderByDescending(x => x.CreateTime);
                    break;

                case MyPostSortType.UolockCountAsc:
                    component.OrderBy(x => x.AppointmentCount);
                    break;

                case MyPostSortType.UolockCountDesc:
                    component.OrderByDescending(x => x.AppointmentCount);
                    break;
            }

            return component.QueryPageResultAsync(param);
        }

        /// <summary>
        /// 搜尋我的官方發贴
        /// </summary>
        /// <param name="param">我的發贴參數</param>
        /// <returns></returns>
        public async Task<PageResultModel<MMOfficialPost>> OfficialPostSearch(MyPostQueryParam param)
        {
            var component = WriteDb.QueryTable<MMOfficialPost>()
                .Where(x => x.UserId == param.UserId && x.IsDelete == false);

            if (param.PostStatus.HasValue)
            {
                component.Where(x => x.Status == param.PostStatus);
            }

            switch (param.SortType)
            {
                case MyPostSortType.CreateDateAsc:
                    component.OrderBy(x => x.UpdateTime);
                    break;

                case MyPostSortType.CreateDateDesc:
                    component.OrderByDescending(x => x.UpdateTime);
                    break;

                case MyPostSortType.UolockCountAsc:
                    component.OrderBy(x => x.AppointmentCount);
                    break;

                case MyPostSortType.UolockCountDesc:
                    component.OrderByDescending(x => x.AppointmentCount);
                    break;
            }

            return await component.QueryPageResultAsync(param);
        }

        /// <summary>
        /// 取得官方贴價格設定
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        public async Task<MMOfficialPostPrice[]> GetOfficialPostPrice(string[] postIds)
        {
            return (await ReadDb.QueryTable<MMOfficialPostPrice>()
               .Where(x => postIds.Contains(x.PostId)).QueryAsync())
               .ToArray();
        }

        /// <summary>
        /// 新增/修改評論單
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> OfficialCommentUpsert(OfficialCommentModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Comment.CommentId))
                {
                    // 這邊與廣場的產id進行共用，切勿修改
                    model.Comment.CommentId = await GetSequenceIdentity<MMPostComment>();

                    await WriteDb.Update(model.Booking)
                        .Insert(model.Comment)
                        .SaveChangesAsync();
                }
                else
                {
                    await WriteDb.Update(model.Booking)
                        .Update(model.Comment)
                        .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得官方贴子評論從id
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <returns></returns>
        public async Task<MMOfficialPostComment?> GetOfficialPostCommentByCommentId(string commentId)
        {
            return (await ReadDb.QueryTable<MMOfficialPostComment>()
                .Where(x => x.CommentId == commentId).QueryAsync())
                .FirstOrDefault();
        }

        public async Task<MMBoss?> GetMyBossInfo(string bossId)
        {
            return (await ReadDb.QueryTable<MMBoss>()
                 .Where(x => x.BossId == bossId).QueryAsync())
                 .FirstOrDefault();
        }

        /// <summary>
        /// 修改Boss信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBossInfo(MMBoss entity)
        {
            try
            {
                await WriteDb.Update<MMBoss>(entity).SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得官方贴子評論從預約單id
        /// </summary>
        /// <param name="bookingId">預約單id</param>
        /// <returns></returns>
        public async Task<MMOfficialPostComment?> GetOfficialPostCommentByBookingId(string bookingId)
        {
            return (await ReadDb.QueryTable<MMOfficialPostComment>()
                .Where(x => x.BookingId == bookingId).QueryAsync())
                .FirstOrDefault();
        }

        public async Task<MMOfficialPostComment[]> GetOfficialPostCommentsByFilter(OfficialPostCommentsFilter filter)
        {
            var query = ReadDb.QueryTable<MMOfficialPostComment>();

            if (filter.BookingIds.IsNotEmpty())
            {
                query = query.Where(e => filter.BookingIds.Contains(e.BookingId));
            }

            return (await query.QueryAsync()).ToArray();
        }

        /// <summary>
        /// 取得官方贴子評論列表
        /// </summary>
        /// <param name="postId">贴id</param>
        /// <param name="pageSetting">分頁設定</param>
        /// <returns></returns>
        public async Task<PageResultModel<MMOfficialPostComment>> GetOfficialPostComment(string postId, PaginationModel pageSetting)
        {
            return await ReadDb.QueryTable<MMOfficialPostComment>()
                .Where(x => x.PostId == postId && x.Status == (int)CommentStatus.Approval)
                .OrderByDescending(x => x.ExamineTime)  //使用審核通過時間
                .QueryPageResultAsync(pageSetting);
        }

        /// <summary>
        /// 上下架官方帖子
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public async Task<bool> SetShelfOfficialPost(string[] postIds, int IsDelete)
        {
            try
            {
                if (postIds.Any())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostIds", postIds);
                    parameters.Add("@IsDelete", IsDelete);

                    string sqlCmd = $@"UPDATE MMOfficialPost SET IsDelete = @IsDelete,[Status]=1 WHERE PostId IN @PostIds;";

                    await WriteDb
                        .AddExecuteSQL(sqlCmd, parameters)
                        .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 設置官方贴為刪除狀態
        /// </summary>
        /// <param name="postIds">贴id list</param>
        /// <returns></returns>
        public async Task<bool> SetOfficialPostIsDeleted(string[] postIds)
        {
            try
            {
                if (postIds.Any())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostIds", postIds);

                    string sqlCmd = $@"UPDATE MMOfficialPost SET IsDelete = 1 WHERE PostId IN @PostIds;";

                    await WriteDb
                        .AddExecuteSQL(sqlCmd, parameters)
                        .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 設置官方贴為刪除狀態（Admin）
        /// </summary>
        /// <param name="postIds">贴id list</param>
        /// <returns></returns>
        public async Task<bool> SetOfficialPostIsDeletedStatus(string[] postIds, int? IsDelete)
        {
            try
            {
                if (postIds.Any())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostIds", postIds);
                    parameters.Add("@IsDelete", IsDelete);

                    string sqlCmd = $@"UPDATE MMOfficialPost SET IsDelete = @IsDelete WHERE PostId IN @PostIds;";

                    await WriteDb
                        .AddExecuteSQL(sqlCmd, parameters)
                        .SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 設置官方贴為刪除狀態ByUserId（Admin）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<bool> SetOfficialPostIsDeletedStatusByUserId(int userId, int? IsDelete)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@IsDelete", IsDelete);

                string sqlCmd = $@"UPDATE MMOfficialPost SET IsDelete = @IsDelete WHERE UserId = @UserId;";

                await WriteDb
                    .AddExecuteSQL(sqlCmd, parameters)
                    .SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 官方店铺列表帖子
        /// </summary>
        /// <param name="param">搜寻参数</param>
        /// <returns></returns>

        public async Task<IEnumerable<MMOfficialPost>> OfficialShopList(ReqOfficialShop param)
        {
            var component = ReadDb.QueryTable<MMOfficialPost>()
               .Where(x => x.UserId == param.UserId && x.IsDelete == false);

            if (string.IsNullOrWhiteSpace(param.Keyword))
            {
                component.Where(x => x.Title.Contains(param.Keyword));
            }

            return await component.QueryAsync();
        }

        /// <summary>
        /// 根据发帖人取得广场寻芳阁发帖数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetPostCountByUserId(int userId)
        {
            var sumPostCount = await ReadDb.QueryTable<MMPost>()
                 .Where(e => e.UserId == userId)
                 .SumAsync<int>(a => a.PostCount);

            return sumPostCount;
        }

        /// <summary>
        /// 根据发帖人取得广场寻芳阁发帖数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetPostCountByUserIdAndPostType(int userId, PostType postType)
        {
            var sumPostCount = await ReadDb.QueryTable<MMPost>()
                 .Where(e => e.UserId == userId && e.PostType == postType)
                 .SumAsync<int>(a => a.PostCount);

            return sumPostCount;
        }

        /// <summary>
        /// 根据发帖人取得官方贴发帖数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns

        public async Task<int> GetOfficialPostCountByUserId(int userId)
        {
            var sumPostCount = await ReadDb.QueryTable<MMOfficialPost>()
                           .Where(e => e.UserId == userId)
                           .SumAsync<int>(a => a.PostCount);

            return sumPostCount;
        }

        /// <summary>
        /// 获取官方发帖数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetOfficialPostCount(int userId)
        {
            var postCount = await ReadDb.QueryTable<MMOfficialPost>()
                           .Where(e => e.UserId == userId)
                           .CountAsync();

            return postCount;
        }

        /// <inheritdoc/>

        public async Task<bool> PostContactUpdate(AdminPostContactUpdateParam param)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", param.UserId);
                parameters.Add("@WeChat", param.WeChat ?? "");
                parameters.Add("@QQ", param.QQ ?? "");
                parameters.Add("@Phone", param.Phone ?? "");

                /// 聯絡方式。1：微信、2：QQ、3：手機號
                string sqlCmd = @"
                -- 删除现有的联系方式记录
                DELETE FROM MMPostContact
                WHERE ContactType IN (1, 2, 3)
                  AND PostId IN (SELECT PostId FROM MMPost WHERE UserId = @UserId);

                -- 插入新的联系方式记录，包括空值
                INSERT INTO MMPostContact (PostId, ContactType, Contact)
                SELECT p.PostId,
                       c.ContactType,
                       CASE
                           WHEN c.ContactType = 1 THEN @WeChat
                           WHEN c.ContactType = 2 THEN @QQ
                           WHEN c.ContactType = 3 THEN @Phone
                           ELSE ''
                       END AS Contact
                FROM MMPost AS p
                CROSS JOIN (VALUES (1), (2), (3)) AS c(ContactType)
                WHERE p.UserId = @UserId;";

                // 执行 SQL 查询
                await WriteDb
                    .AddExecuteSQL(sqlCmd, parameters)
                    .SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
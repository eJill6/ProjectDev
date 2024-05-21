using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.SystemSettings;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Favorite;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.ReportMessage;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    /// <summary>
    /// 贴子相關
    /// </summary>
    public interface IPostRepo
    {
        Task<int> GetReportCount(int UserId);

        /// <summary>
        /// 根据用户ID获取所有举报的ID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetReportAllByUserId(int UserId);

        /// <summary>
        /// 获取帖子的状态
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<MMPost> GetPostStatusByPostId(string postId);

        /// <summary>
        /// 获取官方帖子的状态是否上下架状态
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<MMOfficialPost> GetOfficialPostStatusByPostId(string postId);

        /// <summary>
        /// 取得單一贴子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<MMPost?> GetById(string postId);

        /// <summary>
        /// 新增或編輯贴子
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<DBResult> PostUpsert(PostUpsertData info);

        /// <summary>
        /// 編輯官方贴子(Admin)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<DBResult> AdminOfficialPostUpdate(AdminOfficialPostData info);

        /// <summary>
        /// 編輯官方贴编辑锁定状态(Admin)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> AdminOfficialPostEditLock(string postId, bool status);

        /// <summary>
        /// 新增舉報單
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<DBResult> InsertReport(ReportCreate info);

        /// <summary>
        /// 新增/修改評論單
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<DBResult> CommentUpsert(CommentUpsertData info);

        /// <summary>
        /// 取得信息類型相關的贴子Id
        /// </summary>
        /// <param name="postType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<string[]> GetPostIdByMessageId(PostType postType, int messageId);

        /// <summary>
        /// 取得解鎖的贴子Id
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<string[]> GetPostIdByUnlock(int userId);

        /// <inheritdoc/>
        Task<int> GetPostCount(int userId, ReviewStatus status);

        /// <summary>
        /// 当日发帖数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> GetPostDailyCount(int userId);

        /// <summary>
        /// 取得官方帖發佈上架數量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> GetOfficialPostCount(int userId, ReviewStatus status);

        /// <summary>
        /// 搜尋資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns></returns>
        Task<(MMPost[], int)> PostSearch(PostSearchCondition param);

        /// <summary>
        /// 從Id取得贴子資訊
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        Task<MMPost[]> GetPostInfoById(string[] postIds);

        /// <summary>
        /// 获取帖子数
        /// </summary>
        /// <param name="postIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> GetPostCountByIdAndStatus(string[] postIds, ReviewStatus status);

        /// <summary>
        /// 修改官方帖子
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<bool> EditMMOfficialPost(MMOfficialPost post);

        /// <summary>
        /// 取得贴子相關的服務
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        Task<MMPostServiceMapping[]> GetPostMappingService(string[] postIds);

        /// <summary>
        /// 搜尋我的發贴
        /// </summary>
        /// <param name="param">我的發贴參數</param>
        /// <returns></returns>
        Task<PageResultModel<MMPost>> PostSearch(MyPostQueryParam param);

        /// <summary>
        /// 搜尋我的
        /// </summary>
        /// <param name="param">我的解鎖參數</param>
        /// <returns></returns>
        Task<PageResultModel<MMPost>> PostSearch(MyUnlockQueryParam param);

        /// <summary>
        /// 获取我的解锁帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<UnlockPostList>> GetMyUnlockPost(MyUnlockQueryParam param);

        /// <summary>
        /// 获取我的收藏的店铺数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MyFavoriteOfficialShop>> GetMyFavoriteBossShop(MyFavoriteBossShopQueryParam param);

        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MyFavoritePost>> GetMyFavoritePost(MyFavoritePostQueryParam param);

        /// <summary>
        /// 取得贴子相關的聯絡方式
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        Task<MMPostContact[]> GetPostContact(string[] postIds);

        /// <summary>
        /// 後台取得贴子的列表
        /// </summary>
        /// <param name="param">後台搜尋條件</param>
        /// <returns>贴子的列表</returns>
        Task<PageResultModel<MMPost>> PostSearch(AdminPostListParam param);

        /// <summary>
        /// 後台取得官方贴子的列表
        /// </summary>
        /// <param name="param">後台搜尋條件</param>
        /// <returns>贴子的列表</returns>
        Task<PageResultModel<MMOfficialPost>> OfficialAdminPostSearch(AdminPostListParam param);

        Task<PageResultModel<MMOfficialPost>> OfficialPostList(ReqOfficialStorePost param);

        Task<PageResultModel<MMOfficialPost>> MyOfficialPostList(ReqMyOfficialStorePost param);

        /// <summary>
        /// 取得贴子評論從id
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <returns></returns>
        Task<MMPostComment?> GetPostCommentByCommentId(string commentId);

        /// <summary>
        /// 取得贴子評論從PostId and UserId
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<MMPostComment?> GetPostCommentByPostIdAndUserId(string postId, int userId);

        /// <summary>
        /// 取得用戶評論的贴子
        /// </summary>
        /// <param name="postIds">贴子id list</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<MMPostComment[]> GetPostCommentByUser(string[] postIds, int userId);

        /// <summary>
        /// 取得贴子評論列表
        /// </summary>
        /// <param name="postId">贴id</param>
        /// <param name="pageSetting">分頁設定</param>
        /// <returns></returns>
        Task<PageResultModel<MMPostComment>> GetPostComment(string postId, PaginationModel pageSetting);

        /// <summary>
        /// 檢查使用者是否已評論
        /// </summary>
        /// <param name="postId">贴id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<bool> CheckUserAlreadyComment(string postId, int userId);

        /// <summary>
        /// 後台編輯使用（审核）
        /// </summary>
        /// <param name="post">修改後的Model</param>
        /// <returns></returns>
        Task<DBResult> PostEdit(AdminPostData param);

        /// <summary>
        /// 後台編輯使用（编辑所有参数）
        /// </summary>
        /// <param name="post">修改後的Model</param>
        /// <returns></returns>
        Task<DBResult> PostEditAllData(AdminPostData param);

        /// <summary>
        /// 後台批量編輯使用
        /// </summary>
        /// <param name="post">修改後的Model</param>
        /// <returns></returns>
        Task<DBResult> PostBatchEdit(AdminPostBatchData param);

        /// <summary>
        /// 後台取得評論列表
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>評論列表</returns>
        Task<PageResultModel<MMPostComment>> CommentSearch(AdminCommentListParam param);

        /// <summary>
        /// 後台取得官方評論列表
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>評論列表</returns>
        Task<PageResultModel<MMOfficialPostComment>> OfficialCommentSearch(AdminCommentListParam param);

        /// <summary>
        /// 後台取得評論詳細
        /// </summary>
        /// <param name="commentId">評論編號</param>
        /// <returns>評論詳細</returns>
        Task<MMPostComment?> CommentDetail(string commentId);

        /// <summary>
        /// 後台取得官方評論詳細
        /// </summary>
        /// <param name="commentId">評論編號</param>
        /// <returns>評論詳細</returns>
        Task<MMOfficialPostComment?> OfficialCommentDetail(string commentId);

        /// <summary>
        /// 後台審核評論使用
        /// </summary>
        /// <param name="param">審核評論參數</param>
        /// <returns>資料庫結果</returns>
        Task<DBResult> CommentEdit(AdminCommentData param);

        /// <summary>
        /// 後台官方審核評論使用
        /// </summary>
        /// <param name="param">審核評論參數</param>
        /// <returns>資料庫結果</returns>
        Task<DBResult> OfficialCommentEdit(AdminCommentData param);

        /// <summary>
        /// 後台取得舉報列表
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns>舉報列表</returns>
        Task<PageResultModel<MMPostReport>> ReportSearch(AdminReportListParam param);

        /// <summary>
        /// 根据用户消息记录查询举报数据
        /// </summary>
        /// <param name="messageParam"></param>
        /// <returns></returns>
        Task<PageResultModel<MMPostReport>> ReportMessage(ReportMessageParam messageParam);

        /// <summary>
        /// 後台取得舉報詳細
        /// </summary>
        /// <param name="reportId">舉報編號</param>
        /// <returns>舉報詳細</returns>
        Task<MMPostReport?> ReportDetail(string reportId);

        /// <summary>
        /// 同人同帖投诉单
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MMPostReport>?> GetReports(string postId, int userId);

        /// <summary>
        /// 後台審核舉報使用
        /// </summary>
        /// <param name="param">審核舉報參數</param>
        /// <returns>資料庫結果</returns>
        Task<DBResult> ReportEdit(AdminReportData param);

        /// <summary>
        /// 後台官方審核舉報使用
        /// </summary>
        /// <param name="param">審核舉報參數</param>
        /// <returns>資料庫結果</returns>
        Task<DBResult> OfficialReportEdit(AdminReportData param);

        /// <summary>
        /// 後台贴權重表
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<IEnumerable<PostWeightResult>> GetMMPostWeight();

        /// <summary>
        /// 後台金牌店铺表（全部）
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<IEnumerable<GoldStoreResult>> GetMMGoldStoreAll();

        /// <summary>
        /// 後台金牌店铺表（营业）
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<IEnumerable<GoldStoreResult>> GetMMGoldStoreIsOpen();

        /// <summary>
        /// 店铺/帖子收藏记录
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<IEnumerable<MMPostFavorite>> GetMMPostFavorite(string postId);

        /// <summary>
        /// 店铺/帖子收藏记录
        /// </summary>
        /// <param name="postIds">多个帖子Id</param>
        /// <returns></returns>
        Task<IEnumerable<MMPostFavorite>> GetMMPostFavorite(int? userId, string[] postIds);

        Task<bool> CanCelFavorite(string favoriteId);

        /// <summary>
        /// 後台官方贴權重表
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<IEnumerable<PostWeightResult>> GetMMOfficialPostWeight();

        /// <summary>
        /// 获取我的收藏分页信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MMPostFavorite>> GetMyFavoritePageAsync(FavoriteListParam param, string[]? postIds);

        Task<IEnumerable<MMPostFavorite>> GetAllMyFavorite(FavoriteListParam param);

        /// <summary>
        /// 根据用户ID获取收藏信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<MMPostFavorite>> GetMyFavoriteByUserId(int userId, int type);

        Task<int> GetMyFavoriteShopCount(int userId);

        Task<int> GetMyFavoritePostCount(int userId, int postType);

        /// <summary>
        /// 後台贴權重表新增
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<DBResult> InsertMMPostWeight(MMPostWeight param);

        /// <summary>
        /// 後台贴權重表刪除
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<DBResult> DeleteMMPostWeight(int id);

        /// <summary>
        /// 後台贴權重表更新
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<DBResult> UpdateMMPostWeight(UpdateMMPostParam param);

        /// <summary>
        /// 後台金牌店铺表更新
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<DBResult> UpdateMMGoldStore(List<UpdateMMGoldStoreParam> param);

        /// <summary>
        /// 後台權重表批量刪除
        /// </summary>
        /// <param name="post">修改後的Model</param>
        /// <returns></returns>
        Task<DBResult> PostWeightBatchRemove(AdminPostBatchData param);

        /// <summary>
        /// 更新贴子的觀看次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        Task<bool> UpdatePostViewsCount(string postId);

        /// <summary>
        /// 更新官方贴子的觀看次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        Task<bool> UpdateOfficialPostViewsCount(string postId);

        /// <summary>
        /// 更新贴子的評論次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="isAdd">true=增加，false=減少</param>
        /// <returns></returns>
        Task<bool> UpdatePostCommentsCount(string postId, bool isAdd);

        /// <summary>
        /// 更新官方贴子的評論次數
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="facialScore">該評論顏值</param>
        /// <param name="serviceQuality">該評論服務品質</param>
        /// <param name="isAdd">true=增加，false=減少</param>
        /// <returns></returns>
        Task<bool> UpdateOfficialPostCommentsCount(string postId, decimal facialScore, decimal serviceQuality, bool isAdd);

        /// <summary>
        /// 依據舉報編號取得舉報單
        /// </summary>
        /// <param name="reportIds">舉報編號</param>
        /// <returns>舉報單</returns>
        Task<MMPostReport[]> GetReports(string[] reportIds);

        /// <summary>
        /// 使用者發贴摘要
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<QueryPostTypeCount>> QueryPostCountSummary(int userId);

        /// <summary>
        /// 使用者評論數摘要
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<IEnumerable<QueryCommentPostTypeCount>> QueryCommentCountSummary(int userId);

        /// <summary>
        /// 贴子收藏新增/刪除
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        Task<DBResult> PostFavoriteUpsert(int userId, string postId, int type);

        /// <summary>
        /// 用戶是否有舉報過
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="postId">贴子id</param>
        /// <returns></returns>
        Task<bool> UserHasReported(int userId, string postId);

        /// <summary>
        /// 取得用戶對於該贴的投訴
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="postId">贴子id</param>
        /// <param name="postType">贴子類型</param>
        /// <returns></returns>
        Task<MMPostReport[]> GetUserReported(int userId, string postId, PostType? postType);

        Task<PageResultModel<MMOfficialPost>> GetPageByFilter(PageOfficialPostFilter filter);

        Task<IEnumerable<MMOfficialPost>> GetByFilter(OfficialPostFilter filter);

        /// <summary>
        /// 取得官方單一贴子
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<MMOfficialPost?> GetOfficialPostById(string postId);

        /// <summary>
        /// 取得官方贴
        /// </summary>
        /// <param name="postIds"></param>
        /// <returns></returns>
        Task<MMOfficialPost[]> GetOfficialPostByIds(string[] postIds);

        /// <summary>
        /// 根据发帖人取得官方贴
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MMOfficialPost[]> GetOfficialPostByUserId(int userId);

        /// <summary>
        /// 新增或編輯官方贴子
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<DBResult> OfficialPostUpsert(OfficialPostUpsertData info);

        /// <summary>
        /// 搜尋官方贴資料
        /// </summary>
        /// <param name="param">搜尋參數</param>
        /// <returns></returns>
        Task<(MMOfficialPost[], int)> OfficialPostSearch(PostSearchCondition param);

        /// <summary>
        /// 搜尋我的官方發贴
        /// </summary>
        /// <param name="param">我的發贴參數</param>
        /// <returns></returns>
        Task<PageResultModel<MMOfficialPost>> OfficialPostSearch(MyPostQueryParam param);

        /// <summary>
        /// 搜尋我的官方發贴 (新增了是否上架)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MMOfficialPost>> OfficialBossPostPageAsync(MyOfficialQueryParam param);

        /// <summary>
        /// 取得官方贴價格設定
        /// </summary>
        /// <param name="postIds">贴 ids</param>
        /// <returns></returns>
        Task<MMOfficialPostPrice[]> GetOfficialPostPrice(string[] postIds);

        /// <summary>
        /// 新增/修改評論單
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> OfficialCommentUpsert(OfficialCommentModel data);

        /// <summary>
        /// 取得官方贴子評論從id
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <returns></returns>
        Task<MMOfficialPostComment?> GetOfficialPostCommentByCommentId(string commentId);

        /// <summary>
        /// 获取我的boss信息
        /// </summary>
        /// <param name="bossId"></param>
        /// <param name="applyId"></param>
        /// <returns></returns>
        Task<MMBoss?> GetMyBossInfo(string bossId);

        /// <summary>
        /// 修改Boss信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateBossInfo(MMBoss entity);

        /// <summary>
        /// 取得官方贴子評論從預約單id
        /// </summary>
        /// <param name="bookingId">預約單id</param>
        /// <returns></returns>
        Task<MMOfficialPostComment?> GetOfficialPostCommentByBookingId(string bookingId);

        /// <summary>
        /// 取得官方贴子評論列表
        /// </summary>
        /// <param name="postId">贴id</param>
        /// <param name="pageSetting">分頁設定</param>
        /// <returns></returns>
        Task<PageResultModel<MMOfficialPostComment>> GetOfficialPostComment(string postId, PaginationModel pageSetting);

        /// <summary>
        /// 設置官方贴為刪除狀態
        /// </summary>
        /// <param name="postId">贴id list</param>
        /// <returns></returns>
        Task<bool> SetOfficialPostIsDeleted(string[] postIds);

        /// <summary>
        /// 设置官方帖子为上架状态
        /// </summary>
        /// <param name="postIds"></param>
        /// <returns></returns>
        Task<bool> SetShelfOfficialPost(string[] postIds, int IsDelete);

        /// <summary>
        /// 設置官方贴刪除狀態（Admin）
        /// </summary>
        /// <param name="postId">贴id list</param>
        /// <returns></returns>
        Task<bool> SetOfficialPostIsDeletedStatus(string[] postIds, int? IsDelete);

        /// <summary>
        ///  設置官方贴刪除狀態ByUserId（Admin）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        Task<bool> SetOfficialPostIsDeletedStatusByUserId(int userId, int? IsDelete);

        Task<MMOfficialPostComment[]> GetOfficialPostCommentsByFilter(OfficialPostCommentsFilter filter);

        /// <summary>
        /// 根据UserId获取Post发帖次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetPostCountByUserId(int userId);

        Task<int> GetPostCountByUserIdAndPostType(int userId, PostType postType);

        /// <summary>
        /// 根据UserId获取OfficialPost发帖次数
        /// </summary>
        /// <param name="userId"></param>
        Task<int> GetOfficialPostCountByUserId(int userId);

        /// <summary>
        /// 获取官方发帖数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetOfficialPostCount(int userId);

        Task<bool> PostContactUpdate(AdminPostContactUpdateParam param);
    }
}
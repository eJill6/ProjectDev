using MMService.Models.My;
using MMService.Models.Post;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.User;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Threading.Tasks;

namespace MMService.Services
{
    /// <summary>
    /// 跟贴子相關的
    /// </summary>
    public interface IPostService
    {
        Task<BaseReturnDataModel<UnlockPostResModel>> UnlockPost(UnlockPostReqModel req);

        /// <summary>
        /// 宣傳內容
        /// </summary>
        /// <param name="contentType">宣傳類型</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<WhatIsData>> WhatIs(AdvertisingContentType contentType);

        /// <summary>
        /// 新增贴子
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">發贴人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> AddPost(int? userId, string? nickname, PostData model);

        /// <summary>
        /// 取得編輯贴子的資訊
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PostEditData>> GetPostEditData(string postId, int? userId);

        /// <summary>
        /// 編輯贴子
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">發贴人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> EditPost(string postId, int? userId, string? nickname, PostData model);

        /// <summary>
        /// 檢舉/投訴
        /// </summary>
        /// <param name="complainantUserId">投訴人 UserId</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> Report(int? complainantUserId, ReportData model);

        /// <summary>
        /// 評論
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">評論人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> AddComment(int? userId, string? nickname, CommentData model);

        /// <summary>
        /// 取得編輯評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<CommentEditData>> GetCommentEditData(string commentId, int? userId);

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">評論人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> EditComment(string commentId, int? userId, string? nickname, CommentData model);

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">贴子 id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<CommentList>>> CommentList(string postId, int pageNo);

        /// <summary>
        /// 贴的清單
        /// </summary>
        /// <param name="userId">用戶Id</param>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PostListViewModel>> PostSearch(int? userId, PostSearchParam model);

        /// <summary>
        /// 首页帖子推荐
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<List<PostList>>> RecommendPostList(int? userId);

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="userId">用戶 Id</param>
        /// <param name="postId">贴子 Id</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PostDetail>> PostDetail(int? userId, string postId);

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="postId">贴子 Id</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<AdminPostDetail>> PostAdminDetail(string postId);

        /// <summary>
        /// 取得贴子服務項目的文字
        /// </summary>
        /// <param name="postId">贴子 Id</param>
        /// <returns></returns>
        Task<string[]> GetServiceItemsText(string postId);

        /// <summary>
        /// 後台查詢贴子列表
        /// </summary>
        /// <param name="param">搜尋條件</param>
        /// <returns>贴子列表</returns>
        Task<BaseReturnDataModel<PageResultModel<AdminPostList>>> PostSearch(AdminPostListParam param);

        /// <summary>
        /// 官方贴的清單后台使用
        /// </summary>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<AdminOfficialPostList>>> OfficialAdminPostSearch(AdminPostListParam param);

        /// <summary>
        /// 後台更新贴子狀態（审核）
        /// </summary>
        /// <param name="param">異動參數</param>
        /// <returns>更新結果</returns>
        Task<BaseReturnModel> PostEdit(AdminPostData param);

        /// <summary>
        /// 後台编辑帖子（编辑）
        /// </summary>
        /// <param name="param">異動參數</param>
        /// <returns>更新結果</returns>
        Task<BaseReturnModel> PostEditAllData(AdminPostData param);

        /// <summary>
        /// 後台批量更新贴子狀態
        /// </summary>
        /// <param name="param">異動參數</param>
        /// <returns>更新結果</returns>
        Task<BaseReturnModel> PostBatchEdit(AdminPostBatchData param);

        /// <summary>
        /// 取得評論的列表
        /// </summary>
        /// <param name="param">搜尋條件</param>
        /// <returns>評論的列表</returns>
        Task<BaseReturnDataModel<PageResultModel<AdminCommentList>>> CommentSearch(AdminCommentListParam param);

        /// <summary>
        /// 取得官方評論的列表
        /// </summary>
        /// <param name="param">搜尋條件</param>
        /// <returns>評論的列表</returns>
        Task<BaseReturnDataModel<PageResultModel<AdminOfficialCommentList>>> OfficialCommentSearch(AdminCommentListParam param);

        /// <summary>
        /// 取得評論的列表
        /// </summary>
        /// <param name="commentId">評論編號</param>
        /// <returns>評論詳細</returns>
        Task<BaseReturnDataModel<AdminCommentDetail>> CommentDetail(string commentId);

        /// <summary>
        /// 取得官方評論的列表
        /// </summary>
        /// <param name="commentId">評論編號</param>
        /// <returns>評論詳細</returns>
        Task<BaseReturnDataModel<AdminOfficialCommentDetail>> OfficialCommentDetail(string commentId);

        /// <summary>
        /// 後台審核評論
        /// </summary>
        /// <param name="param">後台審核參數</param>
        /// <returns>審核結果</returns>
        Task<BaseReturnModel> CommentEdit(AdminCommentData param);

        /// <summary>
        /// 後台審核官方評論
        /// </summary>
        /// <param name="param">後台審核參數</param>
        /// <returns>審核結果</returns>
        Task<BaseReturnModel> OfficialCommentEdit(AdminCommentData param);

        /// <summary>
        /// 後台取得舉報資訊
        /// </summary>
        /// <param name="param">搜尋條件</param>
        /// <returns>舉報資訊</returns>
        Task<BaseReturnDataModel<PageResultModel<AdminReportList>>> ReportSearch(AdminReportListParam param);

        /// <summary>
        /// 後台取得舉報詳細資訊
        /// </summary>
        /// <param name="reportId">舉報編號</param>
        /// <returns>舉報詳細資訊</returns>
        Task<BaseReturnDataModel<AdminReportDetail>> ReportDetail(string reportId);

        /// <summary>
        /// 後台審核舉報
        /// </summary>
        /// <param name="param">後台審核參數</param>
        /// <returns>審核結果</returns>
        Task<BaseReturnModel> ReportEdit(AdminReportData param);

        /// <summary>
        /// 後台官方審核舉報
        /// </summary>
        /// <param name="param">後台審核參數</param>
        /// <returns>審核結果</returns>
        Task<BaseReturnModel> OfficialReportEdit(AdminReportData param);

        /// <summary>
        /// 後台贴權重表
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<BaseReturnDataModel<IEnumerable<PostWeightResult>>> GetMMPostWeight();

        /// <summary>
        /// 後台金牌店铺表
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<BaseReturnDataModel<IEnumerable<GoldStoreResult>>> GetMMGoldStore();

        /// <summary>
        /// 店铺/帖子收藏记录
        /// </summary>
        /// <returns>資料庫結果</returns>
        Task<BaseReturnDataModel<IEnumerable<MMPostFavorite>>> GetMMPostFavorite(string postId);

        /// <summary>
        /// 後台贴權重表新增
        /// </summary>
        Task<BaseReturnModel> InsertMMPostWeight(MMPostWeight param);

        /// <summary>
        /// 後台贴權重表刪除
        /// </summary>
        Task<BaseReturnModel> DeleteMMPostWeight(int id);

        /// <summary>
        /// 後台贴權重表修改
        /// </summary>
        Task<BaseReturnModel> UpdateMMPostWeight(UpdateMMPostParam param);

        /// <summary>
        /// 後台金牌店铺表修改
        /// </summary>
        Task<BaseReturnModel> UpdateMMGoldStore(List<UpdateMMGoldStoreParam> param);

        /// <summary>
        /// 後台權重表批量刪除
        /// </summary>
        Task<BaseReturnModel> PostWeightBatchRemove(AdminPostBatchData param);

        /// <summary>
        /// 收藏新增/刪除
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="postId">贴子id</param>
        /// <param name="type">类型。1：帖子、2：店铺</param>
        /// <returns></returns>
        Task<BaseReturnModel> PostFavoriteUpsert(int? userId, string postId, int type);

        /// <summary>
        /// 新增官方贴子
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> AddOfficialPost(ReqOfficialPostData model);

        /// <summary>
        /// 取得編輯官方贴子的資訊
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialPostEditData>> GetOfficialPostEditData(ReqPostIdUserId model);

        /// <summary>
        /// 編輯官方贴子
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> EditOfficialPost(ReqOfficialPostData model);

        /// <summary>
        /// 編輯官方贴子（Admin）
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> AdminEditOfficialPost(AdminOfficialPostData model);

        /// <summary>
        /// 編輯官方贴编辑锁定状态
        /// </summary>
        /// <param name="postId">參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialPostEditLock>> AdminOfficialPostEditLock(string postId, bool status);

        /// <summary>
        /// 官方贴的清單
        /// </summary>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialPostListViewModel>> OfficialPostSearch(ReqOfficialPostSearchParam model);

        /// <summary>
        /// 根据发帖人取得官方贴
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MMOfficialPost[]> GetOfficialPostByUserId(int userId);

        /// <summary>
        /// 官方店铺帖子列表
        /// </summary>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialPostListViewModel>> OfficialPostList(ReqOfficialStorePost model);

        /// <summary>
        /// 获取自己的官方店铺帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialPostListViewModel>> GetMyOfficialPostList(ReqMyOfficialStorePost model);

        /// <summary>
        /// 官方覓贴詳情
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialPostDetail>> OfficialPostDetail(ReqPostIdUserId model);

        /// <summary>
        /// 官方覓贴詳情(Admin)
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<AdminOfficialPostDetail>> AdminOfficialPostDetail(ReqPostIdUserId model);

        /// <summary>
        /// 官方私信
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialDM>> GetOfficialDM(ReqPostIdUserId model);

        /// <summary>
        /// 評論
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> AddOfficialComment(ReqOfficialCommentData model);

        /// <summary>
        /// 取得編輯評論資料
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OfficialCommentEditData>> GetOfficialCommentEditData(ReqCommentIdUserId model);

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> EditOfficialComment(ReqOfficialCommentData model);

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<OfficialCommentList>>> OfficialCommentList(ReqPostIdPageNo model);
    }
}
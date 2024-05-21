
using MMService.Models.My;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.User;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.HomeAnnouncement;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Services
{
    /// <summary>
    /// 個人相關
    /// </summary>
    public interface IMyService
    {
        /// <summary>
        /// 個人中心
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<CenterInfo>> Center(int userId);

        /// <summary>
        /// 總覽
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<Overview>> Overview(int userId, string nickname);

        /// <summary>
        /// 查詢解鎖的贴子
        /// </summary>
        /// <param name="param">查詢參數</param>
        /// <returns>解鎖的贴子</returns>
        Task<BaseReturnDataModel<PageResultModel<MyUnlockPostList>>> UnlockPost(MyUnlockQueryParam param);

        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<MyMessageList>>> MyMessage(MyMessageQueryParamForClient param);

        /// <summary>
        /// 查詢發贴
        /// </summary>
        /// <param name="param">查詢參數</param>
        /// <returns>發贴</returns>
        Task<BaseReturnDataModel<PageResultModel<MyPostList>>> ManagePost(MyPostQueryParam param);
        /// <summary>
        /// 查询官方帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<MyOfficialPostList>>> OfficialBossPostPageAsync(MyOfficialQueryParam param);

        /// <summary>
        ///
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<UserInfoRes>> GetUserInfo(UserInfoReq req);

        /// <summary>
        /// 設定營業中開關
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ShopOpenClosed>> ShopOpen(UserInfoReq req);

        /// <summary>
        /// 刪除官方贴
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> DeleteOfficialPost(ReqOfficialPostDelete req);

        /// <summary>
        /// 上架官方帖子
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> SetShelfOfficialPost(ReqSetShelfOfficialPost req);
        /// <summary>
        /// 编辑商铺的营业时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnModel> EditShopDoBusinessTime(EditDoBusinessTimeParamter req);
        /// <summary>
        /// 用户已读消息记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseReturnModel> UserToMessageOperation(MessageOperationParamForClient param);
        /// <summary>
        /// 用户已读消息记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseReturnModel> UserToMessageAll(MessageOperationParamForClient param);
        /// <summary>
        /// 根据ID获取单个公告消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<MyAnnouncementViewModel>> GetAnnouncementById(int Id);
        /// <summary>
        /// 根据ID获取举报详情
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ReportDetailViewModel>> ReportDetail(string reportId);

        /// <summary>
        /// 获取我的收藏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PageResultModel<MyFavorite>>> GetMyFavorite(MyFavoriteQueryParamForClient param);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<BaseReturnModel> CanCelFavorite(string reportId);

        /// <summary>
        /// 修改官方贴刪除状态
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        Task<BaseReturnModel> ModifyDeleteStatusOfficialPost(ReqOfficialPostDelete req);
    }
}
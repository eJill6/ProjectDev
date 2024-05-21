using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Auth;
using MS.Core.MM.Models.Booking.Res;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Booking;
using MS.Core.MMModel.Models.Chat;
using MS.Core.MMModel.Models.HomeAnnouncement;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.SystemSettings;
using MS.Core.MMModel.Models.User;
using MS.Core.MMModel.Models.Vip;
using MS.Core.MMModel.Models.Wallet;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MS.Core.MMClient.Services
{
    /// <summary>
    /// MM Service
    /// </summary>
    public interface IMMService
    {
        /// <summary>
        /// 由外部提供Url
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// MS Service的Token
        /// </summary>
        string Token { get; set; }

        #region Auth - 權限相關類別

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="param">登入參數</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns>登入返還值</returns>
        Task<ApiResponse<SignInResponse>> SignIn(SignInData param, TimeSpan? timeout = null);

        /// <summary>
        /// 取得認證資訊
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<CertificationResponse>> CertificationInfo(TimeSpan? timeout = null);

        /// <summary>
        /// 覓經紀申請
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> AgentIdentityApply(AgentContactInfoForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 覓老闆申請
        /// </summary>
        /// <param name="param">申請資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> BossIdentityApply(BossIdentityApplyDataForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 觅老板申请或更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ApiResponse> BossIdentityApplyOrUpdate([FromBody] OfficialShopDetailForclient param, TimeSpan? timeout = null);

        /// <summary>
        /// 覓女郎申請
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> GirlIdentityApply(TimeSpan? timeout = null);

        #endregion Auth - 權限相關類別

        /// <summary>
        /// 取得Banner
        /// </summary>
        /// <returns>Banner的結果</returns>
        Task<ApiResponse<BannerViewModel[]>> GetBanner(TimeSpan? timeout = null);

        /// <summary>
        /// 上傳媒體
        /// </summary>
        /// <returns>Banner的結果</returns>
        Task<ApiResponse<MediaViewModel>> CreateMedia(SaveMediaToOssParamForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 上傳媒體
        /// </summary>
        /// <returns>Banner的結果</returns>
        Task<ApiResponse<MSIMOneOnOneChatMessageViewModel[]>> GetRoomMessages(QueryRoomMessageParam param, TimeSpan? timeout = null);

        /// <summary>
        /// 批量上
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<MediaViewModel>> CreateMedias(SaveBossApplyMedia param, TimeSpan? timeout = null);

        /// <summary>
        /// 分批上傳
        /// </summary>
        /// <param name="param">上傳內容</param>
        /// <param name="timeout">逾期時間</param>
        /// <returns>分批路徑</returns>
        Task<ApiResponse<string>> SplitUpload(SaveMediaToOssParamForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 合併媒體
        /// </summary>
        /// <param name="param">上傳內容</param>
        /// <param name="timeout">逾期時間</param>
        /// <returns>媒體結果</returns>
        Task<ApiResponse<MediaViewModel>> MergeUpload(MergeUpload param, TimeSpan? timeout = null);

        /// <summary>
        /// 取得上傳網址及資料
        /// </summary>
        /// <param name="timeout">逾期時間</param>
        /// <returns>上傳網址及資料</returns>
        Task<ApiResponse<VideoUrlModel>> GetUploadVideoUrl(TimeSpan? timeout = null);

        /// <summary>
        /// 取得公告訊息
        /// </summary>
        /// <returns>公告訊息</returns>
        Task<ApiResponse<HomeAnnouncement[]>> GetHomeAnnouncement(TimeSpan? timeout = null);

        /// <summary>
        /// 首页公告信息
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<HomeAnnouncement>> GetFrontsideAnnouncement(TimeSpan? timeout = null);

        /// <summary>
        /// 取得快捷入口列表
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<BannerViewModel[]>> ShortcutList(TimeSpan? timeout = null);

        /// <summary>
        /// 官方店铺banner
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<BannerViewModel[]>> OfficialBanner(TimeSpan? timeout = null);

        /// <summary>
        /// 官方店铺banner
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageRedirectForClient>> GetPageRedirect(int id, TimeSpan? timeout = null);

        #region 選項設定

        /// <summary>
        /// 取得發贴頁面的所有選項設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostTypeOptionsForClient>> OptionsByPostType(int postType, TimeSpan? timeout = null);

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OptionItemForClient[]>> PriceOptions(int postType, TimeSpan? timeout = null);

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OptionItemForClient[]>> MessageTypeOptions(int postType, TimeSpan? timeout = null);

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OptionItemForClient[]>> ServiceOptions(int postType, TimeSpan? timeout = null);

        /// <summary>
        /// 取得年齡設定項
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OptionItemForClient[]>> AgeOptions(TimeSpan? timeout = null);

        /// <summary>
        /// 身高設定項
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OptionItemForClient[]>> BodyHeightOptions(TimeSpan? timeout = null);

        /// <summary>
        /// Cup 設定項
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OptionItemForClient[]>> CupOptions(TimeSpan? timeout = null);

        /// <summary>
        /// 取得管理員聯繫方式
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<AdminContactForClient>> GetAdminContact(TimeSpan? timeout = null);

        /// <summary>
        /// 取得24小时客服跳转链接
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<string>> GetCSLink(TimeSpan? timeout = null);

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostFilterOptionsForClient>> GetPostFilterOptions(int? postType, TimeSpan? timeout = null);

        /// <summary>
        /// 取得预设解锁价格，从 PostType
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostTypeUnlockAmount>> BaseUnlockAmountByType(PostType postType, TimeSpan? timeout = null);

        /// <summary>
        /// 取得所有的预设解锁价格
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostTypeUnlockAmount[]>> AllBaseUnlockAmount(TimeSpan? timeout = null);

        #endregion 選項設定

        #region 跟贴子相關接口

        /// <summary>
        /// 發贴說明
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="contentType">宣傳類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<WhatIsDataForClient>> WhatIs(PostType postType, AdvertisingContentType contentType, TimeSpan? timeout = null);

        /// <summary>
        /// 新增發佈贴
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> AddPost(PostDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 取得編輯贴子的資訊
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostEditDataForClient>> GetPostEditData(string postId, TimeSpan? timeout = null);

        /// <summary>
        /// 編輯發佈贴子
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> EditPost(string postId, PostDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 舉報/投訴
        /// </summary>
        /// <param name="param">投訴資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> Report(ReportDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 評論贴子
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> AddComment(CommentDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 取得編輯評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<CommentEditDataForClient>> GetCommentEditData(string commentId, TimeSpan? timeout = null);

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> EditComment(string commentId, CommentDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">評論的贴子id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<CommentListForClient>>> CommentList(string postId, int? pageNo, TimeSpan? timeout = null);

        /// <summary>
        /// 查找贴子。適用首頁、廣場
        /// </summary>
        /// <param name="param">查詢條件</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostListViewModelForClient>> PostSearch(PostSearchParamForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PostDetailForClient>> PostDetail(string postId, TimeSpan? timeout = null);

        /// <summary>
        /// 解鎖贴子
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<UnlockPostForClient>> UnlockPost(UnlockPostDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 首页推荐帖子列表
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<List<PostListForClient>>> RecommendPostList(TimeSpan? timeout = null);

        /// <summary>
        /// 覓贴收藏
        /// </summary>
        /// <param name="param">贴子Id</param>
        /// <returns></returns>
        Task<ApiResponse> SetFavorite(PostSetFavoriteDataForClient param, TimeSpan? timeout = null);

        #endregion 跟贴子相關接口

        #region 會員中心相關接口

        /// <summary>
        /// 會員中心
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<CenterViewModel>> Center(TimeSpan? timeout = null);
        /// <summary>
        /// 用户是否申请过觅老板
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> GetUserIdIsApplyBoss(TimeSpan? timeout = null);

        /// <summary>
        /// 總攬
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OverviewViewModel>> Overview(TimeSpan? timeout = null);

        /// <summary>
        /// 發贴管理
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<MyPostList>>> ManagePost(MyPostQueryParamForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 發贴管理
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<MyOfficialPostList>>> OfficialBossManagePost(MyOfficialPostQueryParamForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 取的會員中心的解鎖清單
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<MyUnlockPostListViewModelForClient>> UnlockPostList(MyUnlockQueryParamForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 获得我的消息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<MyMessageList>>> MyMessage(MyMessageQueryParamForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 用户已读消息记录
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse> UserToMessageOperation(MessageOperationParamForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 根据ID获取单个公告信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<MyAnnouncementViewModel>> GetAnnouncementById(string Id, TimeSpan? timeout = null);
        /// <summary>
        /// 根据ID获取举报详情
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ReportDetailViewModel>> GetReportDetailById(string reportId, TimeSpan? timeout = null);

        /// <summary>
        /// 商店營業開關
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<ShopOpenClosedForClient>> ShopOpen(TimeSpan? timeout = null);

        /// <summary>
        /// 刪除官方贴
        /// </summary>
        /// <param name="param">刪除資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> DeleteOfficialPost(OfficialPostDeleteForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 上架官方帖子
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse> SetShelfOfficialPost(OfficialPostDeleteForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 修改商铺时间或者是时间段
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse> EditShopDoBusinessTime(EditDoBusinessTimeParamter param, TimeSpan? timeout = null);

        #endregion 會員中心相關接口

        #region Vip

        /// <summary>
        /// 取得販賣的會員卡
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<VipViewModel[]>> GetVips(TimeSpan? timeout = null);

        /// <summary>
        /// 會員卡購買
        /// </summary>
        /// <param name="buyVip"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<BuyVipViewModel>> BuyVip(BuyVipData buyVip, TimeSpan? timeout = null);

        /// <summary>
        /// 覓錢包
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<WalletInfoViewModel>> GetWalletInfo(TimeSpan? timeout = null);

        /// <summary>
        /// Vip 購買紀錄
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<UserVipTransLogViewModel[]>> GetUserVipTransLogs(TimeSpan? timeout = null);

        /// <summary>
        /// 消費明細
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ExpenseSummaryViewModel>> GetExpenseInfo(ExpenseInfoData data, TimeSpan? timeout = null);

        /// <summary>
        /// 收益明細
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<IncomeSummaryViewModel>> GetIncomeInfo(IncomeInfoData data, TimeSpan? timeout = null);

        #endregion Vip

        #region 跟官方贴子相關接口

        /// <summary>
        /// 新增官方發佈贴
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> AddOfficialPost(OfficialPostDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 取得編輯官方贴的資訊
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OfficialPostEditDataForClient>> GetOfficialPostEditData(string postId, TimeSpan? timeout = null);

        /// <summary>
        /// 編輯官方贴
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> EditOfficialPost(string postId, OfficialPostDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 評論官方贴子
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> AddOfficialComment(OfficialCommentDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 取得編輯官方評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OfficialCommentEditDataForClient>> GetOfficialCommentEditData(string commentId, TimeSpan? timeout = null);

        /// <summary>
        /// 編輯官方評論
        /// </summary>
        /// <param name="param">評論Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> EditOfficialComment(string commentId, OfficialCommentDataForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 官方評論清單
        /// </summary>
        /// <param name="postId">評論的贴子id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<OfficialCommentListForClient>>> OfficialCommentList(string postId, int? pageNo, TimeSpan? timeout = null);

        /// <summary>
        /// 查找官方贴子。適用官方
        /// </summary>
        /// <param name="postId">查詢條件</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OfficialPostListViewModelForClient>> OfficialPostSearch(OfficialPostSearchParamForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 获取我收藏的店铺
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<MyFavoriteOfficialShop>>> GetMyFavoriteBossShop([FromBody] MyFavoriteBossShopQueryParam param, TimeSpan? timeout = null);
        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<MyFavoritePost>>> GetMyFavoritePost(MyFavoritePostQueryParam param, TimeSpan? timeout = null);

        /// <summary>
        /// 官方覓贴詳情
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse<OfficialPostDetailForClient>> OfficialPostDetail(string postId, TimeSpan? timeout = null);

        /// <summary>
        /// 首页推荐官方店铺
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<List<GoldStoreViewModelForClient>>> OfficialRecommendShopList(TimeSpan? timeout = null);

        /// <summary>
        /// 金牌店铺（官方首页）
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<List<GoldStoreViewModelForClient>>> OfficialGoldenShopList(TimeSpan? timeout = null);

        /// <summary>
        /// 店铺列表（官方首页）
        /// </summary>
        /// <param name="reqOfficialShop"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<OfficialShopViewModelForClient>>> OfficialShopList(OfficialShopParamForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <param name="applyId">申请id</param>
        /// <returns></returns>
        Task<ApiResponse<OfficialShopDetailForclient>> OfficialShopDetail(string applyId, TimeSpan? timeout = null);
        /// <summary>
        /// 获取我的商铺详情
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<OfficialShopDetailForclient>> GetMyOfficialShopDetail(TimeSpan? timeout = null);
        /// <summary>
        /// 官方帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ApiResponse<OfficialPostListViewModelForClient>> OfficialPostList(OfficialPostListParamForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 官方帖子
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<OfficialPostListViewModelForClient>> GetMyOfficialPostList(MyOfficialPostListParamForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 店铺收藏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ApiResponse> OfficialShopFollow(OfficialShopFollowForClient param, TimeSpan? timeout = null);

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse> CanCelFavorite(string favoriteId, TimeSpan? timeout = null);



        #endregion 跟官方贴子相關接口

        #region 預約相關
        /// <summary>
        /// 預約
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingForClient>> Booking(BookingOfficialDataForClient model, TimeSpan? timeout = null);
        /// <summary>
        /// 預約-管理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<ResManageBookingForClient>>> ManageBooking(ManageBookingDataForClient model, TimeSpan? timeout = null);
        /// <summary>
        /// 我的預約
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<PageResultModel<ResMyBookingForClient>>> MyBooking(MyBookingDataForClient model, TimeSpan? timeout = null);
        /// <summary>
        /// 我的預約-詳情
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResMyBookingDetailForClient>> MyBookingDetail(string bookingId, TimeSpan? timeout = null);
        /// <summary>
        /// 預約-取消
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingCancelForClient>> Cancel(string bookingId, TimeSpan? timeout = null);
        /// <summary>
        /// 預約-接單前退款
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingCancelForClient>> Refund(string bookingId, TimeSpan? timeout = null);
        /// <summary>
        /// 預約-商戶接單
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingAcceptForClient>> Accept(string bookingId, TimeSpan? timeout = null);
        /// <summary>
        /// 預約-確認完成
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingDoneForClient>> Done(string bookingId, TimeSpan? timeout = null);
        /// <summary>
        /// 預約-刪除
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingDeleteForClient>> Delete(string bookingId, TimeSpan? timeout = null);
        /// <summary>
        /// 刪除-申請退費
        /// </summary>
        /// <param name="param">申請資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        Task<ApiResponse> ApplyRefund(ApplyRefundDataForClient param, TimeSpan? timeout = null);
        /// <summary>
        /// 預約詳情(非訂單)
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ApiResponse<ResBookingDetailForClient>> BookingDetail(string postId, TimeSpan? timeout = null);
        #endregion
    }
}
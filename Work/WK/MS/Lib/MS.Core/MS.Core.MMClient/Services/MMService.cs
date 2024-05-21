using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.SMS;
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
using System.Linq;
using System.Threading.Tasks;

namespace MS.Core.MMClient.Services
{
    public class MMService : BaseApiClient, IMMService
    {
        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        public MMService()
        {
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
        }

        #region Auth - 權限相關類別

        /// <inheritdoc/>
        public async Task<ApiResponse<SignInResponse>> SignIn(SignInData param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Auth", "SignIn");

            return await PostJsonAsync<ApiResponse<SignInResponse>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得認證資訊
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<CertificationResponse>> CertificationInfo(TimeSpan? timeout = null)
        {
            var url = GetUrl("Auth", "CertificationInfo");

            return await GetJsonAsync<ApiResponse<CertificationResponse>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 覓經紀申請
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> AgentIdentityApply(AgentContactInfoForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Auth", "AgentIdentityApply");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 覓老闆申請
        /// </summary>
        /// <param name="param">申請資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> BossIdentityApply(BossIdentityApplyDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Auth", "BossIdentityApply");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 覓老闆申請或者修改
        /// </summary>
        /// <param name="param">申請資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> BossIdentityApplyOrUpdate(OfficialShopDetailForclient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Auth", "BossIdentityApplyOrUpdate");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 覓女郎申請
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> GirlIdentityApply(TimeSpan? timeout = null)
        {
            var url = GetUrl("Auth", "GirlIdentityApply");

            return await PostJsonAsync<ApiResponse>(url, requestBody: null, timeout: timeout ?? TimeOut);
        }

        #endregion Auth - 權限相關類別

        /// <inheritdoc/>
        public async Task<ApiResponse<MediaViewModel>> CreateMedia(SaveMediaToOssParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Media", "Create");

            return await PostJsonAsync<ApiResponse<MediaViewModel>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<MSIMOneOnOneChatMessageViewModel[]>> GetRoomMessages(QueryRoomMessageParam param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Chat", "GetRoomMessages");

            return await PostJsonAsync<ApiResponse<MSIMOneOnOneChatMessageViewModel[]>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<MediaViewModel>> CreateMedias(SaveBossApplyMedia param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Media", "CreateImages");

            return await PostJsonAsync<ApiResponse<MediaViewModel>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<string>> SplitUpload(SaveMediaToOssParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Media", "SplitUpload");

            return await PostJsonAsync<ApiResponse<string>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<MediaViewModel>> MergeUpload(MergeUpload param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Media", "MergeUpload");

            return await PostJsonAsync<ApiResponse<MediaViewModel>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<VideoUrlModel>> GetUploadVideoUrl(TimeSpan? timeout = null)
        {
            var url = GetUrl("Media", "GetUploadVideoUrl");

            return await GetJsonAsync<ApiResponse<VideoUrlModel>>(url, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<BannerViewModel[]>> GetBanner(TimeSpan? timeout = null)
        {
            var url = GetUrl("Banner", "GetAll");

            return await GetJsonAsync<ApiResponse<BannerViewModel[]>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<HomeAnnouncement[]>> GetHomeAnnouncement(TimeSpan? timeout = null)
        {
            var url = GetUrl("HomeAnnouncement", "List");
            var result = await GetJsonAsync<ApiResponse<HomeAnnouncement[]>>(url, timeout: timeout ?? TimeOut);
            result.Data = result.Data.Where(x => x.IsActive && x.Type == 2
                    && (x.StartTime <= DateTime.Now || x.StartTime == null)
                    && (x.EndTime >= DateTime.Now || x.StartTime == null)).OrderByDescending(x => x.Weight).ToArray();
            return result;
        }

        public async Task<ApiResponse<HomeAnnouncement>> GetFrontsideAnnouncement(TimeSpan? timeout = null)
        {
            var url = GetUrl("HomeAnnouncement", "FrontsideDetail");
            var result = await GetJsonAsync<ApiResponse<HomeAnnouncement>>(url, timeout: timeout ?? TimeOut);
            return result;
        }

        public async Task<ApiResponse<BannerViewModel[]>> ShortcutList(TimeSpan? timeout = null)
        {
            var url = GetUrl("Banner", "ShortcutList");

            return await GetJsonAsync<ApiResponse<BannerViewModel[]>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<BannerViewModel[]>> OfficialBanner(TimeSpan? timeout = null)
        {
            var url = GetUrl("Banner", "OfficialBanner");

            return await GetJsonAsync<ApiResponse<BannerViewModel[]>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<PageRedirectForClient>> GetPageRedirect(int id, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "GetPageRedirect");

            return await GetJsonAsync<ApiResponse<PageRedirectForClient>>($"{url}/{id}", timeout: timeout ?? TimeOut);
        }

        #region 選項設定

        /// <summary>
        /// 取得發贴頁面的所有選項設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostTypeOptionsForClient>> OptionsByPostType(int postType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "ByPostType");

            return await GetJsonAsync<ApiResponse<PostTypeOptionsForClient>>($"{url}?postType={postType}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OptionItemForClient[]>> PriceOptions(int postType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "Price");

            return await GetJsonAsync<ApiResponse<OptionItemForClient[]>>($"{url}?postType={postType}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OptionItemForClient[]>> MessageTypeOptions(int postType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "MessageType");

            return await GetJsonAsync<ApiResponse<OptionItemForClient[]>>($"{url}?postType={postType}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OptionItemForClient[]>> ServiceOptions(int postType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "Services");

            return await GetJsonAsync<ApiResponse<OptionItemForClient[]>>($"{url}?postType={postType}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得年齡設定項
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OptionItemForClient[]>> AgeOptions(TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "Age");

            return await GetJsonAsync<ApiResponse<OptionItemForClient[]>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 身高設定項
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OptionItemForClient[]>> BodyHeightOptions(TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "BodyHeight");

            return await GetJsonAsync<ApiResponse<OptionItemForClient[]>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// Cup 設定項
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OptionItemForClient[]>> CupOptions(TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "Cup");

            return await GetJsonAsync<ApiResponse<OptionItemForClient[]>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得管理員聯繫方式
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<AdminContactForClient>> GetAdminContact(TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "AdminContact");

            return await GetJsonAsync<ApiResponse<AdminContactForClient>>($"{url}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得24小时客服跳转链接
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> GetCSLink(TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "GetCSLink");

            return await GetJsonAsync<ApiResponse<string>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostFilterOptionsForClient>> GetPostFilterOptions(int? postType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "PostFilterOptions");

            return await GetJsonAsync<ApiResponse<PostFilterOptionsForClient>>($"{url}?postType={postType}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得预设解锁价格，从 PostType
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostTypeUnlockAmount>> BaseUnlockAmountByType(PostType postType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "BaseUnlockAmountByType");

            return await GetJsonAsync<ApiResponse<PostTypeUnlockAmount>>($"{url}?postType={(int)postType}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得所有的预设解锁价格
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostTypeUnlockAmount[]>> AllBaseUnlockAmount(TimeSpan? timeout = null)
        {
            var url = GetUrl("Settings", "AllBaseUnlockAmount");

            return await GetJsonAsync<ApiResponse<PostTypeUnlockAmount[]>>($"{url}", headers: null, timeout: timeout ?? TimeOut);
        }

        #endregion 選項設定

        #region 跟贴子相關接口

        /// <summary>
        /// 發贴說明
        /// </summary>
        /// <param name="postType">發贴類型(三期拿掉)</param>
        /// <param name="contentType">宣傳類型</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<WhatIsDataForClient>> WhatIs(PostType postType, AdvertisingContentType contentType, TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "WhatIs");

            return await GetJsonAsync<ApiResponse<WhatIsDataForClient>>($"{url}?postType={(int)postType}&contentType={(int)contentType}", timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 新增發佈贴
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> AddPost(PostDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "AddPost");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得編輯贴子的資訊
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostEditDataForClient>> GetPostEditData(string postId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "GetPostEditData", postId);

            return await GetJsonAsync<ApiResponse<PostEditDataForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 編輯發佈贴子
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> EditPost(string postId, PostDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "EditPost", postId);

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 舉報/投訴
        /// </summary>
        /// <param name="model">投訴資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> Report(ReportDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "Report");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 評論贴子
        /// </summary>
        /// <param name="model">評論資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> AddComment(CommentDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "AddComment");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得編輯評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<CommentEditDataForClient>> GetCommentEditData(string commentId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "GetCommentEditData", commentId);

            return await GetJsonAsync<ApiResponse<CommentEditDataForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="param">評論Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> EditComment(string commentId, CommentDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "EditComment", commentId);

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">評論的贴子id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<CommentListForClient>>> CommentList(string postId, int? pageNo, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "CommentList", postId);

            return await GetJsonAsync<ApiResponse<PageResultModel<CommentListForClient>>>($"{url}?page={pageNo}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 查找贴子。適用首頁、廣場
        /// </summary>
        /// <param name="postId">查詢條件</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostListViewModelForClient>> PostSearch(PostSearchParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "PostSearch");

            return await PostJsonAsync<ApiResponse<PostListViewModelForClient>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PostDetailForClient>> PostDetail(string postId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "PostDetail", postId);

            return await GetJsonAsync<ApiResponse<PostDetailForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 解鎖贴子
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UnlockPostForClient>> UnlockPost(UnlockPostDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "UnlockPost");

            return await PostJsonAsync<ApiResponse<UnlockPostForClient>>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 首页推荐帖子列表
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<PostListForClient>>> RecommendPostList(TimeSpan? timeout = null)
        {
            var url = GetUrl("Square", "RecommendPostList");

            return await GetJsonAsync<ApiResponse<List<PostListForClient>>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 覓贴收藏
        /// </summary>
        /// <param name="param">贴子Id</param>
        /// <returns></returns>
        public async Task<ApiResponse> SetFavorite(PostSetFavoriteDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "Favorite", param.PostId);

            return await PostJsonAsync<ApiResponse>(url, timeout: timeout ?? TimeOut);
        }

        #endregion 跟贴子相關接口

        #region Wallet

        /// <inheritdoc/>
        public async Task<ApiResponse<WalletInfoViewModel>> GetWalletInfo(TimeSpan? timeout = null)
        {
            var url = GetUrl("Wallet", "WalletInfo");

            return await GetJsonAsync<ApiResponse<WalletInfoViewModel>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<IncomeSummaryViewModel>> GetIncomeInfo(IncomeInfoData data, TimeSpan? timeout = null)
        {
            var url = GetUrl("Wallet", "IncomeInfo");

            return await PostJsonAsync<ApiResponse<IncomeSummaryViewModel>>(url, requestBody: data, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<ExpenseSummaryViewModel>> GetExpenseInfo(ExpenseInfoData data, TimeSpan? timeout = null)
        {
            var url = GetUrl("Wallet", "ExpenseInfo");

            return await PostJsonAsync<ApiResponse<ExpenseSummaryViewModel>>(url, requestBody: data, timeout: timeout ?? TimeOut);
        }

        #endregion Wallet

        #region 會員中心相關接口

        /// <inheritdoc/>
        public async Task<ApiResponse<CenterViewModel>> Center(TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "Center");

            return await GetJsonAsync<ApiResponse<CenterViewModel>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<bool>> GetUserIdIsApplyBoss(TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "GetUserIdIsApplyBoss");

            return await GetJsonAsync<ApiResponse<bool>>(url, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<OverviewViewModel>> Overview(TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "Overview");

            return await GetJsonAsync<ApiResponse<OverviewViewModel>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<PageResultModel<MyPostList>>> ManagePost(MyPostQueryParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "ManagePost");

            return await PostJsonAsync<ApiResponse<PageResultModel<MyPostList>>>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<PageResultModel<MyOfficialPostList>>> OfficialBossManagePost(MyOfficialPostQueryParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "OfficialBossPostPage");

            return await PostJsonAsync<ApiResponse<PageResultModel<MyOfficialPostList>>>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<MyUnlockPostListViewModelForClient>> UnlockPostList(MyUnlockQueryParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "UnlockPost");

            return await PostJsonAsync<ApiResponse<MyUnlockPostListViewModelForClient>>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 商店營業開關
        /// </summary>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<ShopOpenClosedForClient>> ShopOpen(TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "ShopOpen");

            return await PostJsonAsync<ApiResponse<ShopOpenClosedForClient>>(url, requestBody: null, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 刪除官方贴
        /// </summary>
        /// <param name="param">刪除資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> DeleteOfficialPost(OfficialPostDeleteForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "DeleteOfficialPost");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse> SetShelfOfficialPost(OfficialPostDeleteForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "SetShelfOfficialPost");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 修改商铺的营业时间
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse> EditShopDoBusinessTime(EditDoBusinessTimeParamter param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "EditShopDoBusinessTime");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, headers: null, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<PageResultModel<MyMessageList>>> MyMessage(MyMessageQueryParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "MyMessage");

            return await PostJsonAsync<ApiResponse<PageResultModel<MyMessageList>>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse> UserToMessageOperation(MessageOperationParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "UserToMessageOperation");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<MyAnnouncementViewModel>> GetAnnouncementById(string Id, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("My", "GetAnnouncementById", Id);

            return await GetJsonAsync<ApiResponse<MyAnnouncementViewModel>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<ReportDetailViewModel>> GetReportDetailById(string reportId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("My", "ReportDetail", reportId);

            return await GetJsonAsync<ApiResponse<ReportDetailViewModel>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        #endregion 會員中心相關接口

        #region Vip

        /// <inheritdoc/>
        public async Task<ApiResponse<BuyVipViewModel>> BuyVip(BuyVipData buyVip, TimeSpan? timeout = null)
        {
            var url = GetUrl("Vip", "BuyVipTransaction");

            return await PostJsonAsync<ApiResponse<BuyVipViewModel>>(url, requestBody: buyVip, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<VipViewModel[]>> GetVips(TimeSpan? timeout = null)
        {
            var url = GetUrl("Vip", "ListedVips");

            return await GetJsonAsync<ApiResponse<VipViewModel[]>>(url, timeout: timeout ?? TimeOut);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<UserVipTransLogViewModel[]>> GetUserVipTransLogs(TimeSpan? timeout = null)
        {
            var url = GetUrl("Vip", "UserVipTransLogs");

            return await GetJsonAsync<ApiResponse<UserVipTransLogViewModel[]>>(url, timeout: timeout ?? TimeOut);
        }

        #endregion Vip

        #region 跟官方贴子相關接口

        /// <summary>
        /// 新增官方發佈贴
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> AddOfficialPost(OfficialPostDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "AddPost");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得編輯官方贴的資訊
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialPostEditDataForClient>> GetOfficialPostEditData(string postId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "GetPostEditData", postId);

            return await GetJsonAsync<ApiResponse<OfficialPostEditDataForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 編輯官方贴
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="param">發佈資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> EditOfficialPost(string postId, OfficialPostDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "EditPost", postId);

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 評論官方贴子
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> AddOfficialComment(OfficialCommentDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "AddComment");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得編輯官方評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialCommentEditDataForClient>> GetOfficialCommentEditData(string commentId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "GetCommentEditData", commentId);

            return await GetJsonAsync<ApiResponse<OfficialCommentEditDataForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 編輯官方評論
        /// </summary>
        /// <param name="param">評論Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> EditOfficialComment(string commentId, OfficialCommentDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "EditComment", commentId);

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 官方評論清單
        /// </summary>
        /// <param name="postId">評論的贴子id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<OfficialCommentListForClient>>> OfficialCommentList(string postId, int? pageNo, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "CommentList", postId);

            return await GetJsonAsync<ApiResponse<PageResultModel<OfficialCommentListForClient>>>($"{url}?page={pageNo}", headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 查找官方贴子。適用官方
        /// </summary>
        /// <param name="postId">查詢條件</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialPostListViewModelForClient>> OfficialPostSearch(OfficialPostSearchParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "PostSearch");

            return await PostJsonAsync<ApiResponse<OfficialPostListViewModelForClient>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 获取我收藏的店铺
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<MyFavoriteOfficialShop>>> GetMyFavoriteBossShop(MyFavoriteBossShopQueryParam param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "GetMyFavoriteBossShop");

            return await PostJsonAsync<ApiResponse<PageResultModel<MyFavoriteOfficialShop>>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 获取我收藏的帖子
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<MyFavoritePost>>> GetMyFavoritePost(MyFavoritePostQueryParam param, TimeSpan? timeout = null)
        {
            var url = GetUrl("My", "GetMyFavoritePost");

            return await PostJsonAsync<ApiResponse<PageResultModel<MyFavoritePost>>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 官方覓贴詳情
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialPostDetailForClient>> OfficialPostDetail(string postId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "PostDetail", postId);

            return await GetJsonAsync<ApiResponse<OfficialPostDetailForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取得官方私信
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialDMForClient>> DM(string postId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "DM", postId);

            return await GetJsonAsync<ApiResponse<OfficialDMForClient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 首页推荐官方店铺
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<GoldStoreViewModelForClient>>> OfficialRecommendShopList(TimeSpan? timeout = null)
        {
            var url = GetUrl("GoldStore", "OfficialRecommendShopList");

            return await GetJsonAsync<ApiResponse<List<GoldStoreViewModelForClient>>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 金牌店铺（官方首页）
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<GoldStoreViewModelForClient>>> OfficialGoldenShopList(TimeSpan? timeout = null)
        {
            var url = GetUrl("GoldStore", "OfficialGoldenShopList");

            return await GetJsonAsync<ApiResponse<List<GoldStoreViewModelForClient>>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 店铺列表（官方首页）
        /// </summary>
        /// <param name="reqOfficialShop"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<OfficialShopViewModelForClient>>> OfficialShopList(OfficialShopParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("GoldStore", "OfficialShopList");

            return await PostJsonAsync<ApiResponse<PageResultModel<OfficialShopViewModelForClient>>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <param name="applyId">申请id</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialShopDetailForclient>> OfficialShopDetail(string applyId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("GoldStore", "OfficialShopDetail", applyId);

            return await GetJsonAsync<ApiResponse<OfficialShopDetailForclient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <param name="applyId">申请id</param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialShopDetailForclient>> GetMyOfficialShopDetail(TimeSpan? timeout = null)
        {
            var url = GetUrl("GoldStore", "GetMyOfficialShopDetail");

            return await GetJsonAsync<ApiResponse<OfficialShopDetailForclient>>(url, headers: null, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialPostListViewModelForClient>> OfficialPostList(OfficialPostListParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "OfficialPostList");

            return await PostJsonAsync<ApiResponse<OfficialPostListViewModelForClient>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse<OfficialPostListViewModelForClient>> GetMyOfficialPostList(MyOfficialPostListParamForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "GetMyOfficialPostList");

            return await PostJsonAsync<ApiResponse<OfficialPostListViewModelForClient>>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 店铺收藏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ApiResponse> OfficialShopFollow(OfficialShopFollowForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("Square", "OfficialShopFollow", param.ApplyId);

            return await PostJsonAsync<ApiResponse>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse> CanCelFavorite(string favoriteId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("My", "CanCelFavorite", favoriteId);

            return await GetJsonAsync<ApiResponse>(url, timeout: timeout ?? TimeOut);
        }

        #endregion 跟官方贴子相關接口

        #region 預約相關

        public async Task<ApiResponse<ResBookingDetailForClient>> BookingDetail(string postId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "BookingDetail", postId);

            return await GetJsonAsync<ApiResponse<ResBookingDetailForClient>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 贴子預約
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ResBookingForClient>> Booking(BookingOfficialDataForClient model, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "Booking");

            var result = await PostJsonAsync<ApiResponse<ResBookingForClient>>(url, requestBody: model, timeout: timeout ?? TimeOut);
            if (result.IsSuccess && !string.IsNullOrEmpty(result.Data.PhoneNo))
            {
                _internalMessageQueueService.Value.EnqueueSendSMS(new SendUserSMSParam
                {
                    Usage = SMSUsages.ValidateCode,
                    PhoneNo = result.Data.PhoneNo,
                    CountryCode = result.Data.CountryCode,
                    ContentParamInfo = result.Data.ContentParamInfo
                });
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<ResManageBookingForClient>>> ManageBooking(ManageBookingDataForClient model, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "ManageBooking");

            return await PostJsonAsync<ApiResponse<PageResultModel<ResManageBookingForClient>>>(url, requestBody: model, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 我的預約
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PageResultModel<ResMyBookingForClient>>> MyBooking(MyBookingDataForClient model, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "MyBooking");

            return await PostJsonAsync<ApiResponse<PageResultModel<ResMyBookingForClient>>>(url, requestBody: model, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 我的預約明細
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ResMyBookingDetailForClient>> MyBookingDetail(string bookingId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "MyBookingDetail", bookingId);

            return await GetJsonAsync<ApiResponse<ResMyBookingDetailForClient>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 取消預約
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ResBookingCancelForClient>> Cancel(string bookingId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "Cancel", bookingId);

            return await GetJsonAsync<ApiResponse<ResBookingCancelForClient>>(url, timeout: timeout ?? TimeOut);
        }

        public async Task<ApiResponse<ResBookingCancelForClient>> Refund(string bookingId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "Refund", bookingId);

            return await GetJsonAsync<ApiResponse<ResBookingCancelForClient>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 商戶接單
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ResBookingAcceptForClient>> Accept(string bookingId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "Accept", bookingId);

            return await GetJsonAsync<ApiResponse<ResBookingAcceptForClient>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 確認完成
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ResBookingDoneForClient>> Done(string bookingId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "Done", bookingId);

            return await GetJsonAsync<ApiResponse<ResBookingDoneForClient>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 刪除預約
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ResBookingDeleteForClient>> Delete(string bookingId, TimeSpan? timeout = null)
        {
            var url = GetUrlRoute("OfficialPost", "Delete", bookingId);

            return await GetJsonAsync<ApiResponse<ResBookingDeleteForClient>>(url, timeout: timeout ?? TimeOut);
        }

        /// <summary>
        /// 申請退費
        /// </summary>
        /// <param name="param">申請資料</param>
        /// <param name="timeout">逾期設定</param>
        /// <returns></returns>
        public async Task<ApiResponse> ApplyRefund(ApplyRefundDataForClient param, TimeSpan? timeout = null)
        {
            var url = GetUrl("OfficialPost", "ApplyRefund");

            return await PostJsonAsync<ApiResponse>(url, requestBody: param, timeout: timeout ?? TimeOut);
        }

        #endregion 預約相關
    }
}
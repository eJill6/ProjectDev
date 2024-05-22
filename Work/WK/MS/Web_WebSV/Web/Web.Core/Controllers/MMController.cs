using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums.User;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Auth;
using MS.Core.MMClient.Models;
using MS.Core.MMClient.Services;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Auth.Enums;
using MS.Core.MMModel.Models.Booking;
using MS.Core.MMModel.Models.Chat;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Vip;
using MS.Core.MMModel.Models.Wallet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Runtime.Caching;
using Web.Core.Models.MM;

namespace Web.Controllers
{
    public class MMController : BaseController
    {
        private readonly Lazy<ILogger<MMController>> _logger;

        private readonly IMMService _mmService;

        private readonly Lazy<IByteArrayApiService> _byteArrayApiService;

        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        private readonly Lazy<IRouteUtilService> _routeUtilService;

        private readonly string OriginProtocol = "seal://common/webview?type=";

        private readonly string OriginRecordProtocol = "seal://user/";

        protected static MemoryCache MemoryCache { get; } = MemoryCache.Default;

        private static string CacheKey => string.Format(CacheKeyHelper.MMUserToken, AuthenticationUtil.GetUserKey());

        public static string SaveLoginLogCacheKey =>
            string.Format("MMServiceLoginLogCacheKey:{0}", AuthenticationUtil.GetUserKey());

        private static string BannerCacheKey = "BannerMemoryCacheKey";

        private static string OssCdnDomainCacheKey = "OssCdnDomainCacheKey";

        public MMController()
        {
            _routeUtilService = ResolveService<IRouteUtilService>();
            _logger = ResolveService<ILogger<MMController>>();
            _mmService = ResolveService<IMMService>().Value;
            _mmService.BaseUrl = ConfigUtilService.Get("MSServiceUrl");
            _byteArrayApiService = ResolveService<IByteArrayApiService>();
            _httpContextAccessor = ResolveService<IHttpContextAccessor>();
        }

        /// <summary>
        /// 銀像投注頁
        /// </summary>
        /// <returns>投注頁</returns>
        public async Task<ActionResult> Index(string pageParamInfo = "")
        {
            TicketUserData ticket = AuthenticationUtil.GetLoginUserFromCache();

            if (ticket.UserId == 0)
            {
                return await Task.FromResult(NotFound());
            }

            ViewBag.logonMode = ticket.LogonMode;
            ViewBag.pageParamInfo = pageParamInfo;
            ViewBag.pageRedirectInfo = new
            {
                redrectType = -1,
                refId = string.Empty,
            };
            if (pageParamInfo.Contains("_"))
            {
                try
                {
                    var split = pageParamInfo.Split("_");
                    if (split[0].Equals("Redirect", StringComparison.OrdinalIgnoreCase))
                    {
                        var id = Convert.ToInt32(split[1]);
                        var queryResult = await GetDataResultRaw(async () => await _mmService.GetPageRedirect(id, TimeSpan.FromSeconds(120)));
                        if (queryResult.IsSuccess)
                        {
                            var data = queryResult.Data;
                            ViewBag.pageRedirectInfo = new
                            {
                                redrectType = data.Type,
                                refId = data.RefId
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 不影響既有功能，僅記錄失敗訊息
                    _logger.Value.LogError(ex, $"取得轉導設定失敗");
                }
            }
            ViewBag.UserId = GetUserId();

            // 提現
            var withdrawUrl = $"seal://common/webview?type=withdraw";
            // 兌換
            var exchangeUrl = $"seal://common/webview?type=exchange";
            var depositUrl = $"seal://common/webview?type=deposit";
            // 兌換紀錄
            var exchangerecordUrl = $"seal://user/exchangerecord";

            // 充提紀錄
            var dwReportUrl = $"seal://common/webview?type=dwReport";

            var bindPhoneUrl = "seal://user/bindphone";

            var vipCenter = "seal://user/vipcenter";

            if (bool.Parse(ConfigUtilService.Get("UseCDN")))
            {
                string cdnUrl = ConfigUtilService.Get("CDNSite");

                if (cdnUrl != null)
                {
                    if (cdnUrl[cdnUrl.Length - 1] != '/')
                    {
                        cdnUrl = cdnUrl + "/";
                    }
                }

                ViewBag.cdnUrl = cdnUrl + "CTS/ClientApp/dist/mimi-dating/img/";
            }

            //获取谷歌统计得标识code
            var googleStatisticsCode = ConfigUtilService.Get("GoogleStatisticsCode");
            if (!string.IsNullOrEmpty(googleStatisticsCode))
                ViewBag.googleStatisticsCode = googleStatisticsCode;
            else
                ViewBag.googleStatisticsCode = "G-N9MSVXH1BE";

            if (ticket.LogonMode == LogonMode.Lite.Value)
            {
                var prefixUrl = $"{ticket.DepositUrl}/#/msh5/";

                // 提現
                ViewBag.withdrawUrl = "withdraw";
                // 兌換
                ViewBag.exchangeUrl = "exchange";
                ViewBag.depositUrl = "deposit";
                // 兌換紀錄
                ViewBag.exchangerecordUrl = "exchangerecord";

                // 充提紀錄
                ViewBag.dwReportUrl = "dwReport";
                // 綁定手機
                ViewBag.bindPhoneUrl = "bindPhone";
                ViewBag.vipCenter = "vipcenter";
                ViewBag.originUrl = ticket.DepositUrl;
            }
            else
            {
                // 提現
                ViewBag.withdrawUrl = withdrawUrl;
                // 兌換
                ViewBag.exchangeUrl = exchangeUrl;
                ViewBag.depositUrl = depositUrl;
                // 兌換紀錄
                ViewBag.exchangerecordUrl = exchangerecordUrl;

                // 充提紀錄
                ViewBag.dwReportUrl = dwReportUrl;
                // 綁定手機
                ViewBag.bindPhoneUrl = bindPhoneUrl;
                ViewBag.vipCenter = vipCenter;
            }

            try
            {
                GetToken(VisitType.Login);
                ViewBag.Token = _routeUtilService.Value.GetMiseWebTokenName();
            }
            catch
            {
            }

            return await Task.FromResult(View());
        }

        #region 媒體相關操作

        /// <summary>
        /// 取得Banner
        /// </summary>
        /// <returns>Banner</returns>
        [HttpGet]
        public async Task<ActionResult> GetBanner()
        {
            var result = MemoryCache.Get(BannerCacheKey) as ActionResult;
            if (result == null)
            {
                result = await GetDataResult(async () => await _mmService.GetBanner());
                MemoryCache.Set(BannerCacheKey, result, DateTime.Now.AddMinutes(1));
            }

            return result;
        }

        /// <summary>
        /// 上傳媒體檔
        /// </summary>
        /// <returns>媒體檔資料</returns>
        [HttpPost]
        public async Task<ActionResult> CreateMedia([FromBody] SaveMediaToOssParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.CreateMedia(param, TimeSpan.FromSeconds(120)));
        }

        [HttpPost]
        public async Task<ActionResult> GetRoomMessages([FromBody] QueryRoomMessageParam queryRoomMessageParam)
        {
            return await GetDataResult(async () => await _mmService.GetRoomMessages(queryRoomMessageParam, TimeSpan.FromSeconds(120)));
        }

        /// <summary>
        /// 批量上
        /// </summary>
        /// <returns>媒體檔資料</returns>
        [HttpPost]
        public async Task<ActionResult> CreateMedias([FromBody] SaveBossApplyMedia param)
        {
            return await GetDataResult(async () => await _mmService.CreateMedias(param, TimeSpan.FromSeconds(120)));
        }

        /// <summary>
        /// 分批上傳媒體檔
        /// </summary>
        /// <returns>媒體檔資料</returns>
        [HttpPost]
        public async Task<ActionResult> SplitUpload([FromBody] SaveMediaToOssParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.SplitUpload(param, TimeSpan.FromSeconds(120)));
        }

        /// <summary>
        /// 合併上傳媒體檔
        /// </summary>
        /// <returns>媒體檔資料</returns>
        [HttpPost]
        public async Task<ActionResult> MergeUpload([FromBody] MergeUpload param)
        {
            return await GetDataResult(async () => await _mmService.MergeUpload(param, TimeSpan.FromSeconds(120)));
        }

        /// <summary>
        /// 取得上傳網址及資料
        /// </summary>
        /// <returns>上傳網址及資料</returns>
        [HttpGet]
        public async Task<ActionResult> GetUploadVideoUrl()
        {
            return await GetDataResult(async () => await _mmService.GetUploadVideoUrl());
        }

        #endregion 媒體相關操作

        /// <summary>
        /// 取得首頁公告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetHomeAnnouncement()
        {
            return await GetDataResult(async () => await _mmService.GetHomeAnnouncement(TimeSpan.FromSeconds(120)));
        }

        /// <summary>
        /// 取得首頁公告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetFrontsideAnnouncement()
        {
            return await GetDataResult(async () => await _mmService.GetFrontsideAnnouncement(TimeSpan.FromSeconds(120)));
        }

        #region 選項設定

        /// <summary>
        /// 取得發帖頁面的所有選項設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> OptionsByPostType(int postType)
        {
            return await GetDataResult(async () => await _mmService.OptionsByPostType(postType));
        }

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PriceOptions(int postType)
        {
            return await GetDataResult(async () => await _mmService.PriceOptions(postType));
        }

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MessageTypeOptions(int postType)
        {
            return await GetDataResult(async () => await _mmService.MessageTypeOptions(postType));
        }

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ServiceOptions(int postType)
        {
            return await GetDataResult(async () => await _mmService.ServiceOptions(postType));
        }

        /// <summary>
        /// 取得年齡項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AgeOptions()
        {
            return await GetDataResult(async () => await _mmService.AgeOptions());
        }

        /// <summary>
        /// 取得身高項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> BodyHeightOptions()
        {
            return await GetDataResult(async () => await _mmService.BodyHeightOptions());
        }

        /// <summary>
        /// 取得cup項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CupOptions()
        {
            return await GetDataResult(async () => await _mmService.CupOptions());
        }

        /// <summary>
        /// 取得管理員聯繫方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AdminContact()
        {
            return await GetDataResult(async () => await _mmService.GetAdminContact());
        }

        /// <summary>
        /// 取得24h客服链接地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetCSLink()
        {
            return await GetDataResult(async () => await _mmService.GetCSLink());
        }

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PostFilterOptions(int? postType)
        {
            return await GetDataResult(async () => await _mmService.GetPostFilterOptions(postType));
        }

        /// <summary>
        /// 取得预设解锁价格，从 PostType
        /// </summary>
        /// <param name="postType">帖的类型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> BaseUnlockAmountByType(PostType postType)
        {
            return await GetDataResult(async () => await _mmService.BaseUnlockAmountByType(postType));
        }

        /// <summary>
        /// 取得所有的预设解锁价格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AllBaseUnlockAmount()
        {
            return await GetDataResult(async () => await _mmService.AllBaseUnlockAmount());
        }

        /// <summary>
        /// 首页快捷入口列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ShortcutList()
        {
            return await GetDataResult(async () => await _mmService.ShortcutList());
        }

        /// <summary>
        /// 官方店铺banner
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialBanner()
        {
            return await GetDataResult(async () => await _mmService.OfficialBanner());
        }

        #endregion 選項設定

        #region 跟帖子相關接口

        /// <summary>
        /// 發帖說明
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <param name="contentType">宣傳類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> WhatIs(PostType postType, AdvertisingContentType contentType)
        {
            return await GetDataResult(async () => await _mmService.WhatIs(postType, contentType));
        }

        /// <summary>
        /// 新增發佈帖
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddPost([FromBody] PostDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.AddPost(param));
        }

        /// <summary>
        /// 取得編輯帖子資料
        /// </summary>
        /// <param name="postId">帖 Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetPostEditData(string postId)
        {
            return await GetDataResult(async () => await _mmService.GetPostEditData(postId));
        }

        /// <summary>
        /// 編輯發佈帖子
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditPost([FromBody] EditPostData param)
        {
            return await GetDataResult(async () => await _mmService.EditPost(param.PostId, param));
        }

        /// <summary>
        /// 舉報/投訴
        /// </summary>
        /// <param name="param">投訴資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Report([FromBody] ReportDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.Report(param));
        }

        /// <summary>
        /// 評論帖子
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] CommentDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.AddComment(param));
        }

        /// <summary>
        /// 取得編輯評論資訊
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetCommentEditData(string commentId)
        {
            return await GetDataResult(async () => await _mmService.GetCommentEditData(commentId));
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditComment([FromBody] EditCommentData param)
        {
            return await GetDataResult(async () => await _mmService.EditComment(param.CommentId, param));
        }

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">評論的帖子id</param>
        /// <param name="page">目前頁數</param>
        /// <param name="pageNo">目前頁數</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CommentList(string postId, int? page, int? pageNo)
        {
            return await GetDataResult(async () => await _mmService.CommentList(postId, pageNo ?? page ?? 1));
        }

        /// <summary>
        /// 查找帖子。適用首頁、廣場
        /// </summary>
        /// <param name="param">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostSearch([FromBody] PostSearchParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.PostSearch(param));
        }

        /// <summary>
        /// 查找帖子。適用首頁、廣場
        /// </summary>
        /// <param name="postId">帖子Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PostDetail(string postId)
        {
            return await GetDataResult(async () =>
            {
                var result = await _mmService.PostDetail(postId);
                if (!string.IsNullOrWhiteSpace(result.Data.VideoUrl) &&
                    !Path.GetExtension(result.Data.VideoUrl).ToLower().Contains(".m3u8"))
                {
                    result.Data.VideoUrl = string.Empty;
                }

                return result;
            });
            //return await GetDataResult(async () => await _service.PostDetail(postId));
        }

        /// <summary>
        /// 解鎖帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UnlockPost([FromBody] UnlockPostDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.UnlockPost(param));
        }

        /// <summary>
        /// 首页广场帖子推荐列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RecommendPostList()
        {
            return await GetDataResult(async () => await _mmService.RecommendPostList());
        }

        /// <summary>
        /// 覓贴收藏
        /// </summary>
        /// <param name="param">贴子Id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SetFavorite([FromBody] PostSetFavoriteDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.SetFavorite(param));
        }

        #endregion 跟帖子相關接口

        #region 官方帖子相關

        /// <summary>
        /// 新增發佈帖
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddOfficialPost([FromBody] OfficialPostDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.AddOfficialPost(param));
        }

        /// <summary>
        /// 取得編輯帖子資料
        /// </summary>
        /// <param name="postId">帖 Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetOfficialPostEditData(string postId)
        {
            return await GetDataResult(async () => await _mmService.GetOfficialPostEditData(postId));
        }

        /// <summary>
        /// 編輯發佈帖子
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditOfficialPost([FromBody] EditOfficialPostData param)
        {
            return await GetDataResult(async () => await _mmService.EditOfficialPost(param.PostId, param));
        }

        /// <summary>
        /// 評論帖子
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddOfficialComment([FromBody] OfficialCommentDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.AddOfficialComment(param));
        }

        /// <summary>
        /// 取得編輯評論資訊
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetOfficialCommentEditData(string commentId)
        {
            return await GetDataResult(async () => await _mmService.GetOfficialCommentEditData(commentId));
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditOfficialComment([FromBody] EditOfficialCommentData param)
        {
            return await GetDataResult(async () => await _mmService.EditOfficialComment(param.CommentId, param));
        }

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">評論的帖子id</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> OfficialCommentList(string postId, int? pageNo)
        {
            return await GetDataResult(async () => await _mmService.OfficialCommentList(postId, pageNo));
        }

        /// <summary>
        /// 查找帖子。適用首頁、廣場
        /// </summary>
        /// <param name="param">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> OfficialPostSearch([FromBody] OfficialPostSearchParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.OfficialPostSearch(param));
        }

        /// <summary>
        /// 查找帖子。適用首頁、廣場
        /// </summary>
        /// <param name="postId">帖子Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> OfficialPostDetail(string postId)
        {
            return await GetDataResult(async () =>
            {
                var result = await _mmService.OfficialPostDetail(postId);
                if (result.IsSuccess &&
                    !string.IsNullOrWhiteSpace(result.Data.VideoUrl) &&
                    !Path.GetExtension(result.Data.VideoUrl).ToLower().Contains(".m3u8"))
                {
                    result.Data.VideoUrl = string.Empty;
                }

                return result;
            });

            //return await GetDataResult(async () => await _service.OfficialPostDetail(postId));
        }

        /// <summary>
        /// 首页官方店铺推荐列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialRecommendShopList()
        {
            return await GetDataResult(async () => await _mmService.OfficialRecommendShopList());
        }

        /// <summary>
        /// 金牌店铺（官方首页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialGoldenShopList()
        {
            return await GetDataResult(async () => await _mmService.OfficialGoldenShopList());
        }

        /// <summary>
        /// 店铺与帖子列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> OfficialShopList([FromBody] OfficialShopParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.OfficialShopList(param));
        }

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <param name="applyId">审核id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialShopDetail(string applyId)
        {
            return await GetDataResult(async () => await _mmService.OfficialShopDetail(applyId));
        }

        /// <summary>
        /// 官方店铺详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMyOfficialShopDetail()
        {
            return await GetDataResult(async () => await _mmService.GetMyOfficialShopDetail());
        }

        /// <summary>
        /// 官方店铺帖子列表
        /// </summary>
        /// <param name="param">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> OfficialPostList([FromBody] OfficialPostListParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.OfficialPostList(param));
        }

        /// <summary>
        /// 获取我的官方店铺帖子列表
        /// </summary>
        /// <param name="param">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMyOfficialPostList([FromBody] MyOfficialPostListParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.GetMyOfficialPostList(param));
        }

        /// <summary>
        /// 店铺收藏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> OfficialShopFollow([FromBody] OfficialShopFollowForClient param)
        {
            return await GetDataResult(async () => await _mmService.OfficialShopFollow(param));
        }

        #endregion 官方帖子相關

        #region 預約相關

        /// <summary>
        /// 預約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Booking([FromBody] BookingOfficialDataForClient model)
        {
            return await GetDataResult(async () => await _mmService.Booking(model));
        }

        [HttpGet]
        public async Task<ActionResult> BookingDetail(string postId)
        {
            return await GetDataResult(async () => await _mmService.BookingDetail(postId));
        }

        /// <summary>
        /// 我的預約
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> MyBooking([FromBody] MyBookingDataForClient model)
        {
            return await GetDataResult(async () => await _mmService.MyBooking(model));
        }

        /// <summary>
        /// 預約管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ManageBooking([FromBody] ManageBookingDataForClient model)
        {
            return await GetDataResult(async () => await _mmService.ManageBooking(model));
        }

        /// <summary>
        /// 預約-商戶接單
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Accept(string bookingId)
        {
            return await GetDataResult(async () => await _mmService.Accept(bookingId));
        }

        /// <summary>
        /// 預約-確認完成
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Done(string bookingId)
        {
            return await GetDataResult(async () => await _mmService.Done(bookingId));
        }

        /// <summary>
        /// 預約-取消
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Cancel(string bookingId)
        {
            return await GetDataResult(async () => await _mmService.Cancel(bookingId));
        }

        /// <summary>
        /// 預約-接單前退款
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Refund(string bookingId)
        {
            return await GetDataResult(async () => await _mmService.Refund(bookingId));
        }

        /// <summary>
        /// 我的預約-詳細
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MyBookingDetail(string bookingId)
        {
            return await GetDataResult(async () => await _mmService.MyBookingDetail(bookingId));
        }

        /// <summary>
        /// 刪除預約
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Delete(string bookingId)
        {
            return await GetDataResult(async () => await _mmService.Delete(bookingId));
        }

        /// <summary>
        /// 申請退費
        /// </summary>
        /// <param name="model">申請資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ApplyRefund([FromBody] ApplyRefundDataForClient model)
        {
            return await GetDataResult(async () => await _mmService.ApplyRefund(model));
        }

        #endregion 預約相關

        #region My

        /// <summary>
        /// 個人中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Center()
        {
            return await GetDataResult(async () => await _mmService.Center());
        }

        /// <summary>
        /// 用户是否申请过觅老板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUserIdIsApplyBoss()
        {
            return await GetDataResult(async () => await _mmService.GetUserIdIsApplyBoss());
        }

        /// <summary>
        /// 總覽
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Overview()
        {
            return await GetDataResult(async () => await _mmService.Overview());
        }

        /// <summary>
        /// 發帖管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ManagePost([FromBody] MyPostQueryParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.ManagePost(param));
        }

        /// <summary>
        /// 获取收藏帖子
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetMyFavoritePost([FromBody] MyFavoritePostQueryParam param)
        {
            return await GetDataResult(async () => await _mmService.GetMyFavoritePost(param));
        }

        /// <summary>
        /// 获取收藏店铺
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetMyFavoriteBossShop([FromBody] MyFavoriteBossShopQueryParam param)
        {
            return await GetDataResult(async () => await _mmService.GetMyFavoriteBossShop(param));
        }

        /// <summary>
        /// 發帖管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> OfficialBossManagePost([FromBody] MyOfficialPostQueryParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.OfficialBossManagePost(param));
        }

        /// <summary>
        /// 發帖管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UnlockPostList([FromBody] MyUnlockQueryParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.UnlockPostList(param));
        }

        /// <summary>
        /// 商店營業開關
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ShopOpen()
        {
            return await GetDataResult(async () => await _mmService.ShopOpen());
        }

        /// <summary>
        /// 刪除官方帖
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteOfficialPost([FromBody] OfficialPostDeleteForClient param)
        {
            return await GetDataResult(async () => await _mmService.DeleteOfficialPost(param));
        }

        [HttpPost]
        public async Task<ActionResult> SetShelfOfficialPost([FromBody] OfficialPostDeleteForClient param)
        {
            return await GetDataResult(async () => await _mmService.SetShelfOfficialPost(param));
        }

        [HttpPost]
        public async Task<ActionResult> EditShopDoBusinessTime([FromBody] EditDoBusinessTimeParamter param)
        {
            return await GetDataResult(async () => await _mmService.EditShopDoBusinessTime(param));
        }

        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> MyMessage([FromBody] MyMessageQueryParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.MyMessage(param));
        }

        /// <summary>
        /// 用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UserToMessageOperation([FromBody] MessageOperationParamForClient param)
        {
            return await GetDataResult(async () => await _mmService.UserToMessageOperation(param));
        }

        /// <summary>
        /// 根据ID获取公告详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAnnouncementById(string Id)
        {
            return await GetDataResult(async () => await _mmService.GetAnnouncementById(Id));
        }

        /// <summary>
        /// 根据ID获取投诉详情
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetReportDetailById(string reportId)
        {
            return await GetDataResult(async () => await _mmService.GetReportDetailById(reportId));
        }

        [HttpGet]
        public async Task<ActionResult> CanCelFavorite(string favoriteId)
        {
            return await GetDataResult(async () => await _mmService.CanCelFavorite(favoriteId));
        }

        #endregion My

        #region Vip

        /// <summary>
        /// 購買會員卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BuyVip([FromBody] BuyVipData param)
        {
            return await GetDataResult(async () => await _mmService.BuyVip(param));
        }

        /// <summary>
        /// 會員卡列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetVips()
        {
            return await GetDataResult(async () => await _mmService.GetVips());
        }

        /// <summary>
        /// 購買紀錄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUserVipTransLogs()
        {
            return await GetDataResult(async () => await _mmService.GetUserVipTransLogs());
        }

        #endregion Vip

        #region Wallet

        /// <summary>
        /// 覓錢包
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetWalletInfo(TimeSpan? timeout = null)
        {
            return await GetDataResult(async () => await _mmService.GetWalletInfo());
        }

        /// <summary>
        /// 消費明細
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetExpenseInfo([FromQuery] ExpenseInfoData data)
        {
            return await GetDataResult(async () => await _mmService.GetExpenseInfo(data));
        }

        /// <summary>
        /// 收益明細
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetIncomeInfo([FromQuery] IncomeInfoData data, TimeSpan? timeout = null)
        {
            return await GetDataResult(async () => await _mmService.GetIncomeInfo(data));
        }

        #endregion Wallet

        #region 身份申請

        /// <summary>
        /// 取得認證資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CertificationInfo()
        {
            return await GetDataResult(async () => await _mmService.CertificationInfo());
        }

        /// <summary>
        /// 覓經紀申請
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AgentIdentityApply([FromBody] AgentContactInfoForClient param)
        {
            return await GetDataResult(async () => await _mmService.AgentIdentityApply(param));
        }

        /// <summary>
        /// 覓老闆申請
        /// </summary>
        /// <param name="model">申請資料</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BossIdentityApply([FromBody] BossIdentityApplyDataForClient param)
        {
            return await GetDataResult(async () => await _mmService.BossIdentityApply(param));
        }

        /// <summary>
        /// 觅老板申请或者更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BossIdentityApplyOrUpdate([FromBody] OfficialShopDetailForclient param)
        {
            return await GetDataResult(async () => await _mmService.BossIdentityApplyOrUpdate(param));
        }

        /// <summary>
        /// 覓女郎申請
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GirlIdentityApply()
        {
            return await GetDataResult(async () => await _mmService.GirlIdentityApply());
        }

        #endregion 身份申請

        private async Task<bool> IsLogin()
        {
            try
            {
                string token = await GetToken();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Value.LogError(ex, "LoginMMService fail");
            }

            return false;
        }

        private async Task<string> GetTokenByLogin(VisitType visitType)
        {
            var userDataRaw = string.Empty;
            try
            {
                var userData = await GetUserInfoWithoutAvailable(GetUserId());
                userDataRaw = userData != null ? JsonConvert.SerializeObject(userData) : string.Empty;
                var loginRes = _mmService.SignIn(new SignInData()
                {
                    Nickname = userData.UserName,
                    UserId = userData.UserId,
                    Type = visitType
                }).ConfigureAwait(false).GetAwaiter().GetResult();

                if (!loginRes.IsSuccess)
                {
                    _logger.Value.LogError($"GetTokenByLogin fail userData:{userDataRaw}");
                    throw new Exception(loginRes.Code);
                }

                return loginRes.Data.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.Value.LogError(ex, $"GetTokenByLogin fail userData:{userDataRaw}");
                throw;
            }
        }

        private async Task<ApiResponse<T>> GetDataResultRaw<T>(Func<Task<ApiResponse<T>>> dataFunc)
        {
            _mmService.Token = await GetToken();
            var resultData = new ApiResponse<T>();

            try
            {
                resultData = await dataFunc();
            }
            catch (Exception ex)
            {
                var isUnauthorized = false;
                var mmException = ex as ClientException;
                if (mmException != null)
                {
                    try
                    {
                        var exceptionMsg = mmException.Message.Deserialize<ClientErrorMessage>();
                        isUnauthorized = exceptionMsg != null && exceptionMsg.StatusCode == HttpStatusCode.Unauthorized;
                        if (isUnauthorized)
                        {
                            CacheService.Del(CacheKey);

                            if (await IsLogin())
                            {
                                _mmService.Token = await GetToken();
                                resultData = await dataFunc();
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.Value.LogError(innerEx, $"Deserialize mmException message fail:{mmException.Message}");
                    }
                }

                if (!isUnauthorized)
                {
                    _logger.Value.LogError(ex, "Unauthorized");
                }
                else
                {
                    _logger.Value.LogError(ex, "Error");
                }
            }

            return resultData;
        }

        private async Task<ActionResult> GetDataResult(Func<Task<ApiResponse>> dataFunc)
        {
            return await GetDataResult(dataFunc, async (result) =>
            {
                return await Task.FromResult(new
                {
                    isSuccess = result.IsSuccess,
                    errorMessage = result.Message,
                    code = result.Code,
                });
            });
        }

        private async Task<ActionResult> GetDataResult(Func<Task<ApiResponse>> dataFunc,
            Func<ApiResponse, Task<object>> returnFunc)
        {
            ActionResult result;

            _mmService.Token = await GetToken();
            ApiResponse resultData = null;

            try
            {
                resultData = await dataFunc();
            }
            catch (Exception ex)
            {
                var isUnauthorized = false;
                var mmException = ex as ClientException;
                if (mmException != null)
                {
                    try
                    {
                        var exceptionMsg = mmException.Message.Deserialize<ClientErrorMessage>();
                        isUnauthorized = exceptionMsg != null && exceptionMsg.StatusCode == HttpStatusCode.Unauthorized;
                        if (isUnauthorized)
                        {
                            CacheService.Del(CacheKey);

                            if (await IsLogin())
                            {
                                _mmService.Token = await GetToken();
                                resultData = await dataFunc();
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.Value.LogError(innerEx, $"Deserialize mmException message fail:{mmException.Message}");
                    }
                }

                if (!isUnauthorized)
                {
                    _logger.Value.LogError(ex, "Unauthorized");
                }
                else
                {
                    _logger.Value.LogError(ex, "Error");
                }
            }

            if (resultData != null)
            {
                if (_byteArrayApiService.Value.IsEncodingRequired(_httpContextAccessor.Value.HttpContext.Request))
                {
                    return new JsonResult(await returnFunc(resultData));
                }

                result = Content(JsonConvert.SerializeObject(await returnFunc(resultData), new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }), "application/json");
            }
            else
            {
                var errorResult = new
                {
                    isSuccess = false,
                    errorMessage = "数据采集失败"
                };

                if (_byteArrayApiService.Value.IsEncodingRequired(_httpContextAccessor.Value.HttpContext.Request))
                {
                    return new JsonResult(errorResult);
                }

                result = Content(JsonConvert.SerializeObject(errorResult, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }), "application/json");
            }

            return result;
        }

        private async Task<ActionResult> GetDataResult<T>(Func<Task<ApiResponse<T>>> dataFunc) where T : class
        {
            return await GetDataResult(async () => { return await dataFunc(); }, async (result) =>
            {
                var finalResult = result as ApiResponse<T>;
                return await Task.FromResult(new
                {
                    data = finalResult.Data,
                    isSuccess = finalResult.IsSuccess,
                    errorMessage = finalResult.Message,
                    code = result.Code,
                });
            });
        }

        /// <summary>
        /// 取得access token
        /// </summary>
        /// <param name="visitType">訪問類型。只有在進到畫面第一次為登入行為，其餘為更換token的心跳行為</param>
        /// <returns></returns>
        private async Task<string> GetToken(VisitType visitType = VisitType.Heartbeat)
        {
            var token = MemoryCache.Get(CacheKey) as string;
            if (string.IsNullOrWhiteSpace(token))
            {
                token = CacheService.GetOrSetByRedis(CacheKey, () => { return GetTokenByLogin(visitType).ConfigureAwait(false).GetAwaiter().GetResult(); },
                    DateTime.Now.AddMinutes(25));
                MemoryCache.Set(CacheKey, token, DateTime.Now.AddMinutes(10));
            }
            else if (visitType == VisitType.Login)
            {
                if (MemoryCache.Get(SaveLoginLogCacheKey) == null)
                {
                    GetTokenByLogin(visitType);
                    MemoryCache.Set(SaveLoginLogCacheKey, token, DateTime.Now.AddSeconds(60));
                }
            }

            return token;
        }
    }
}
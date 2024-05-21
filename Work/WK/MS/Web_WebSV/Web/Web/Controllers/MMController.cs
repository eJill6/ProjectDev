using JxBackendService.Common.Util;
using JxBackendService.Model.Paging;
using Microsoft.Extensions.Logging;
using MS.Core.MMClient.Models;
using MS.Core.MMClient.Services;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Vip;
using MS.Core.MMModel.Models.Wallet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Helpers;
using Web.Helpers.Security;
using Web.Models.Base;
using Web.Services;
using JxBackendService.Model.Enums.User;

namespace Web.Controllers
{
    public class MMController : BaseController
    {
        private readonly ILogger<MMController> _logger = null;

        private readonly IMMService _service = null;

        private readonly string OriginProtocol = "seal://common/webview?type=";
        private readonly string OriginRecordProtocol = "seal://user/";

        private static string CacheKey => string.Format(CacheKeyHelper.MMUserToken, AuthenticationUtil.GetUserKey());

        public MMController(
            IUserService userService,
            ICacheService cacheService,
            IMMService service,
            ILogger<MMController> logger
            ) : base(cacheService, userService)
        {
            _logger = logger;
            _service = service;
            _service.BaseUrl = ConfigurationManager.AppSettings["MSServiceUrl"];
        }

        /// <summary>
        /// 銀像投注頁
        /// </summary>
        /// <returns>投注頁</returns>
        public ActionResult Index(string pageParamInfo = "")
        {
            TicketUserData ticket = AuthenticationUtil.GetLoginUserFromCache();

            if (ticket.UserId == 0)
            {
                return HttpNotFound();
            }

            ViewBag.logonMode = ticket.LogonMode;
            ViewBag.pageParamInfo = pageParamInfo;

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

            if (ticket.LogonMode == LogonMode.Lite.Value)
            {
                var prefixUrl = $"{ticket.DepositUrl}/#/msh5/";

                // 提現
                ViewBag.withdrawUrl = withdrawUrl.Replace(OriginProtocol, prefixUrl);
                // 兌換
                ViewBag.exchangeUrl = exchangeUrl.Replace(OriginProtocol, prefixUrl);
                ViewBag.depositUrl = depositUrl.Replace(OriginProtocol, prefixUrl);
                // 兌換紀錄
                ViewBag.exchangerecordUrl = exchangerecordUrl.Replace(OriginRecordProtocol, prefixUrl);

                // 充提紀錄
                ViewBag.dwReportUrl = dwReportUrl.Replace(OriginProtocol, prefixUrl);
                // 綁定手機
                ViewBag.bindPhoneUrl = bindPhoneUrl.Replace(OriginRecordProtocol, prefixUrl);
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
            }

            return View();
        }

        #region 媒體相關操作

        /// <summary>
        /// 取得Banner
        /// </summary>
        /// <returns>Banner</returns>
        [HttpGet]
        public async Task<ActionResult> GetBanner()
        {
            return await GetDataResult(async () => await _service.GetBanner());
        }

        /// <summary>
        /// 上傳媒體檔
        /// </summary>
        /// <returns>媒體檔資料</returns>
        [HttpPost]
        public async Task<ActionResult> CreateMedia(SaveMediaToOssParamForClient param)
        {
            return await GetDataResult(async () => await _service.CreateMedia(param, TimeSpan.FromSeconds(120)));
        }

        #endregion 媒體相關操作

        /// <summary>
        /// 取得首頁公告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetHomeAnnouncement()
        {
            return await GetDataResult(async () => await _service.GetHomeAnnouncement(TimeSpan.FromSeconds(120)));
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
            return await GetDataResult(async () => await _service.OptionsByPostType(postType));
        }

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PriceOptions(int postType)
        {
            return await GetDataResult(async () => await _service.PriceOptions(postType));
        }

        /// <summary>
        /// 取得標籤設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LabelOptions(int postType)
        {
            return await GetDataResult(async () => await _service.LabelOptions(postType));
        }

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MessageTypeOptions(int postType)
        {
            return await GetDataResult(async () => await _service.MessageTypeOptions(postType));
        }

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ServiceOptions(int postType)
        {
            return await GetDataResult(async () => await _service.ServiceOptions(postType));
        }

        /// <summary>
        /// 取得年齡項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AgeOptions()
        {
            return await GetDataResult(async () => await _service.AgeOptions());
        }

        /// <summary>
        /// 取得身高項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> BodyHeightOptions()
        {
            return await GetDataResult(async () => await _service.BodyHeightOptions());
        }

        /// <summary>
        /// 取得cup項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CupOptions()
        {
            return await GetDataResult(async () => await _service.CupOptions());
        }

        /// <summary>
        /// 取得管理員聯繫方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AdminContact()
        {
            return await GetDataResult(async () => await _service.GetAdminContact());
        }

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="postType">發帖類型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PostFilterOptions(int? postType)
        {
            return await GetDataResult(async () => await _service.GetPostFilterOptions(postType));
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
            return await GetDataResult(async () => await _service.WhatIs(postType, contentType));
        }

        /// <summary>
        /// 新增發佈帖
        /// </summary>
        /// <param name="param">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddPost(PostDataForClient param)
        {
            return await GetDataResult(async () => await _service.AddPost(param));
        }

        /// <summary>
        /// 取得編輯帖子資料
        /// </summary>
        /// <param name="postId">帖 Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult> GetPostEditData(string postId)
        {
            return await GetDataResult(async () => await _service.GetPostEditData(postId));
        }

        /// <summary>
        /// 編輯發佈帖子
        /// </summary>
        /// <param name="postId">帖 Id</param>
        /// <param name="param">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        public async Task<ActionResult> EditPost(string postId, PostDataForClient param)
        {
            return await GetDataResult(async () => await _service.EditPost(postId, param));
        }

        /// <summary>
        /// 舉報/投訴
        /// </summary>
        /// <param name="param">投訴資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Report(ReportDataForClient param)
        {
            return await GetDataResult(async () => await _service.Report(param));
        }

        /// <summary>
        /// 評論帖子
        /// </summary>
        /// <param name="param">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentDataForClient param)
        {
            return await GetDataResult(async () => await _service.AddComment(param));
        }

        /// <summary>
        /// 取得編輯評論資訊
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{commentId}")]
        public async Task<ActionResult> GetCommentEditData(string commentId)
        {
            return await GetDataResult(async () => await _service.GetCommentEditData(commentId));
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="commentId">評論Id</param>
        /// <param name="param">評論資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{commentId}")]
        public async Task<ActionResult> EditComment(string commentId, CommentDataForClient param)
        {
            return await GetDataResult(async () => await _service.EditComment(commentId, param));
        }

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">評論的帖子id</param>
        /// <param name="page">目前頁數</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult> CommentList(string postId, int? page)
        {
            return await GetDataResult(async () => await _service.CommentList(postId, page));
        }

        /// <summary>
        /// 查找帖子。適用首頁、廣場
        /// </summary>
        /// <param name="param">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostSearch(PostSearchParamForClient param)
        {
            return await GetDataResult(async () => await _service.PostSearch(param));
        }

        /// <summary>
        /// 查找帖子。適用首頁、廣場
        /// </summary>
        /// <param name="postId">帖子Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult> PostDetail(string postId)
        {
            return await GetDataResult(async () => await _service.PostDetail(postId));
        }

        /// <summary>
        /// 解鎖帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UnlockPost(UnlockPostDataForClient param)
        {
            return await GetDataResult(async () => await _service.UnlockPost(param));
        }

        #endregion 跟帖子相關接口

        #region My

        /// <summary>
        /// 個人中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Center()
        {
            return await GetDataResult(async () => await _service.Center());
        }

        /// <summary>
        /// 總覽
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Overview()
        {
            return await GetDataResult(async () => await _service.Overview());
        }

        /// <summary>
        /// 發帖管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ManagePost(MyPostQueryParamForClient param)
        {
            return await GetDataResult(async () => await _service.ManagePost(param));
        }

        /// <summary>
        /// 發帖管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UnlockPostList(MyUnlockQueryParamForClient param)
        {
            return await GetDataResult(async () => await _service.UnlockPostList(param));
        }

        #endregion My

        #region Vip

        /// <summary>
        /// 購買會員卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BuyVip(BuyVipData param)
        {
            return await GetDataResult(async () => await _service.BuyVip(param));
        }

        /// <summary>
        /// 會員卡列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetVips()
        {
            return await GetDataResult(async () => await _service.GetVips());
        }

        /// <summary>
        /// 購買紀錄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUserVipTransLogs()
        {
            return await GetDataResult(async () => await _service.GetUserVipTransLogs());
        }

        #endregion Vip

        #region Wallet

        /// <summary>
        /// 覓錢包
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetWalletInfo(TimeSpan? timeout = null)
        {
            return await GetDataResult(async () => await _service.GetWalletInfo());
        }

        /// <summary>
        /// 消費明細
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetExpenseInfo(ExpenseInfoData data)
        {
            return await GetDataResult(async () => await _service.GetExpenseInfo(data));
        }

        /// <summary>
        /// 收益明細
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetIncomeInfo(IncomeInfoData data, TimeSpan? timeout = null)
        {
            return await GetDataResult(async () => await _service.GetIncomeInfo(data));
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
            return await GetDataResult(async () => await _service.CertificationInfo());
        }

        /// <summary>
        /// 覓經紀申請
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AgentIdentityApply()
        {
            return await GetDataResult(async () => await _service.AgentIdentityApply());
        }

        #endregion 身份申請

        private bool IsLogin()
        {
            try
            {
                var token = _cacheService.GetOrSetByRedis(CacheKey, GetTokenByLogin, DateTime.Now.AddMinutes(25));

                if (!string.IsNullOrWhiteSpace(token))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoginMMService fail");
            }
            return false;
        }

        private string GetTokenByLogin()
        {
            var userDataRaw = string.Empty;
            try
            {
                var userData = GetUserInfo(false);
                userDataRaw = userData != null ? JsonConvert.SerializeObject(userData) : string.Empty;
                var loginRes = _service.SignIn(new SignInData()
                {
                    Nickname = userData.UserName,
                    UserId = userData.UserId
                }).ConfigureAwait(false).GetAwaiter().GetResult();

                if (!loginRes.IsSuccess)
                {
                    throw new Exception(loginRes.Code);
                }
                return loginRes.Data.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetTokenByLogin fail userData:{userDataRaw}");
                throw;
            }
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

            _service.Token = GetToken();
            ApiResponse resultData = null;

            try
            {
                resultData = await dataFunc();
            }
            catch (Exception ex)
            {
                var isUnauthorized = false;
                var sportException = ex as ClientException;
                if (sportException != null)
                {
                    var exceptionMsg = sportException.Message.Deserialize<ClientErrorMessage>();
                    isUnauthorized = exceptionMsg != null && exceptionMsg.StatusCode == HttpStatusCode.Unauthorized;
                    if (isUnauthorized)
                    {
                        _cacheService.Del(CacheKey);

                        if (IsLogin())
                        {
                            _service.Token = GetToken();
                            resultData = await dataFunc();
                        }
                    }
                }

                if (!isUnauthorized)
                {
                    _logger.LogError(ex, "Unauthorized");
                }
                else
                {
                    _logger.LogError(ex, "Error");
                }
            }

            if (resultData != null)
            {
                result = Content(JsonConvert.SerializeObject(await returnFunc(resultData), new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }), "application/json");
            }
            else
            {
                result = Content(JsonConvert.SerializeObject(new
                {
                    isSuccess = false,
                    errorMessage = "数据采集失败"
                }, new JsonSerializerSettings
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
            return await GetDataResult(async () =>
            {
                return await dataFunc();
            }, async (result) =>
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

        private string GetToken() => _cacheService.GetOrSetByRedis(CacheKey, GetTokenByLogin, DateTime.Now.AddMinutes(25));
    }
}
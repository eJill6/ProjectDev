using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Infrastructure.Jobs;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.Base;
using ControllerShareLib.Models.LongResult;
using ControllerShareLib.Models.LotteryPlan;
using ControllerShareLib.Models.SpaTrend;
using ControllerShareLib.Services;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxLottery.Services.Adapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Model;
using System.Data;
using System.Text.Json;
using Web.Core.Infrastructure;
using Web.Core.Infrastructure.Filters;
using Web.Helpers;

namespace Web.Controllers
{
    [LotterySpaExceptionFilter]
    public class LotterySpaController : BaseController
    {

        private readonly Lazy<ICache> _localInstance;

        private readonly Lazy<ILotteryService> _lotteryService;

        private readonly Lazy<ILotterySpaService> _spaService;

        private readonly Lazy<IRouteUtilService> _routeUtilService;

        /// <summary>
        /// User service.
        /// </summary>

        protected readonly Lazy<IEnumerable<IBonusAdapter>> _adapters;

        protected readonly Lazy<IEnumerable<ISpaTrendHelper>> _helpers;

        protected readonly Lazy<ILogger<LotterySpaService>> _logger;

        private readonly int[] _specialRuleIds;

        public LotterySpaController()
        {
            _routeUtilService = ResolveService<IRouteUtilService>();
            _lotteryService = ResolveService<ILotteryService>();
            _spaService = ResolveService<ILotterySpaService>();
            _adapters = ResolveService<IEnumerable<IBonusAdapter>>();
            _helpers = ResolveService<IEnumerable<ISpaTrendHelper>>();
            _logger = ResolveService<ILogger<LotterySpaService>>();
            _specialRuleIds = _spaService.Value.GetSpecialRuleIds();
            _localInstance = ResolveService<ICache>();
        }

        /// <summary>
        /// 銀像投注頁
        /// </summary>
        /// <returns>投注頁</returns>
        public async Task<IActionResult> Index(string orderNo = null, string pageParamInfo = null)
        {
            TicketUserData ticket = AuthenticationUtil.GetLoginUserFromCache();

            if (ticket.UserId == 0)
            {
                return await Task.FromResult(NotFound());
            }

            var lotteryId = (JxLottery.Adapters.Models.Lottery.LotteryId)Enum
                .Parse(typeof(JxLottery.Adapters.Models.Lottery.LotteryId), ticket.GameID);

            var gameId = _specialRuleIds.Contains((int)lotteryId) ? lotteryId.ToString() : "";

            if (string.IsNullOrEmpty(gameId))
            {
                return View("NetworkError");
            }

            ViewBag.isFullScreen = false;
            if (!string.IsNullOrWhiteSpace(pageParamInfo))
            {
                ViewBag.isFullScreen = true;
            }

            var model = GetBetViewModel(gameId, out int lotteryID, orderNo, ViewBag.isFullScreen);

            if (model == null)
            {
                return await Task.FromResult(NotFound());
            }

            ViewBag.UserId = GetUserId();
            TicketUserData userInfo = AuthenticationUtil.GetLoginUserFromCache();

            ViewBag.lotteryID = lotteryID.ToString();
            ViewBag.originUrl = userInfo.DepositUrl;

            ViewBag.Token = _routeUtilService.Value.GetMiseWebTokenName();
            return await Task.FromResult(View(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetLotteryInfo(string lotteryId)
        {
            TicketUserData ticketUserData = GetUserToken();
            var lotteryUserInfo = ticketUserData.CastByJson<LotteryUserData>();

            UserInfo userInfoBase = await GetUserInfoWithoutAvailable(ticketUserData.UserId);
            lotteryUserInfo.RebatePro = userInfoBase.RebatePro;

            return await Task.FromResult(Json(_spaService.Value.GetLotteryInfo(lotteryId, lotteryUserInfo)));
        }

        /// <summary>
        /// 取得彩種資訊
        /// </summary>
        /// <param name="id">彩種編號</param>
        /// <param name="orderNo">跟單編號</param>
        /// <returns>彩種資訊</returns>
        private object GetBetViewModel(string id, out int lotteryID, string orderNo = null, bool isFullscreen = false)
        {
            var lottery = _spaService.Value.GetLotteryInfoMethod(id);

            if (lottery == null)
            {
                lotteryID = 0;
                return null;
            }
            var adapter = this._adapters.Value.FirstOrDefault(x => x.LotteryId == lottery.LotteryID);

            if (adapter == null)
            {
                lotteryID = 0;
                return null;
            }

            var lotteryInfo = new
            {
                adapter.GameTypeId,
                GameTypeName = ((JxLottery.Models.Lottery.GameTypeId)adapter.GameTypeId).ToString(),
                LotteryId = lottery.LotteryID,
                LotteryTypeName = lottery.LotteryType,
                OfficialUrl = lottery.OfficialLotteryUrl,
                TrendUrl = Url.Action(nameof(Trend), new { id }),
                lottery.MaxBonusMoney,
                LotteryCode = lottery.TypeURL,
                LogoUrl = WebResourceHelper.Content($"~/2015/images/0{lottery.LotteryID}.png"),
                MaxAfterPeriods = After.GetChaseNumberCount(lottery.LotteryID, int.MaxValue)
            };

            var playConfigs = _spaService.Value.GetPlayConfig(adapter).ConfigureAwait(false).GetAwaiter().GetResult();

            var sysSetting = this.UserService.GetSysSettings();

            var settings = sysSetting == null ? new object() : new
            {
                sysSetting.MaxBetCount,
            };

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

                var fullcdnUrl = cdnUrl + "CTS/ClientApp/dist/yinsiang-lottery/img/";
                if (isFullscreen)
                {
                    fullcdnUrl = cdnUrl + "CTS/ClientApp/dist/yinsiang-lottery-fullscreen/img/";
                }
                else if (lottery.LotteryID >= 70)
                {
                    fullcdnUrl = cdnUrl + "CTS/ClientApp/dist/yinsiang-lottery-new/img/";
                }

                settings = sysSetting == null ? new object() : new
                {
                    sysSetting.MaxBetCount,
                    cdnUrl = fullcdnUrl
                };
            }

            TicketUserData userInfo = AuthenticationUtil.GetLoginUserFromCache();

            var msSetting = new
            {
                DepositUrl = userInfo.DepositUrl,
                RoomId = userInfo.RoomNo,
                OrderNo = orderNo,
                LogonMode = userInfo.LogonMode,
            };
            lotteryID = lottery.LotteryID;
            return new { lotteryInfo, playConfigs, settings, msSetting };
        }

        /// <summary>
        /// 設定ViewBag
        /// </summary>
        private void SetViewBag()
        {
            // layout.cshtml for rabbit mq
            ViewBag.UserId = GetUserId();
            TicketUserData userInfo = AuthenticationUtil.GetLoginUserFromCache();
            ViewBag.originUrl = userInfo.DepositUrl;
        }

        /// <summary>
        /// 走勢圖
        /// </summary>
        /// <param name="id">LotteryCode(路由名稱叫Id)</param>
        /// <returns>走勢圖</returns>
        public async Task<IActionResult> Trend(string id, int? sort = null, int searchType = 1, string tab = "comprehensive")
        {
            var lottery = _lotteryService.Value.GetAllLotteryInfo()
                .FirstOrDefault(x => string.Equals(x.TypeURL, id, StringComparison.InvariantCultureIgnoreCase));

            if (lottery == null)
            {
                return await Task.FromResult(NotFound());
            }

            var gameTypeId = (JxLottery.Models.Lottery.GameTypeId)_adapters.Value.FirstOrDefault(x => x.LotteryId == lottery.LotteryID)?.GameTypeId;

            var model = GetTrend(lottery, sort, searchType, tab);
            return await Task.FromResult(View($"{gameTypeId}Trend", model));
        }

        /// <summary>
        /// 取得返點
        /// </summary>
        /// <returns>結果</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRebatePro(int lotteryId, int playTypeId, int playTypeRadioId, bool? isSingleRebatePro)
        {
            var userId = GetUserId();
            var cacheKey = CacheKeyHelper.ApiRebatePro(userId, lotteryId, playTypeId, playTypeRadioId);
            var now = DateTime.Now;
            var cacheExpired = DateTime.Now.AddMinutes(3);
            if (now.Day != cacheExpired.Day)
            {
                cacheExpired = cacheExpired.Date;
            }
            var result =
                await _localInstance.Value.GetOrAddAsync(nameof(GetRebatePros), cacheKey, async () =>
                {
                    var userInfo = await GetUserInfoWithoutAvailable(userId);
                    return _spaService.Value.GetRebatePro(
                        userInfo,
                        lotteryId,
                        playTypeId,
                        playTypeRadioId,
                        isSingleRebatePro);
                }, cacheExpired);

            return await Ok(result);
        }

        /// <summary>
        /// 取得賠率資料
        /// </summary>
        /// <returns>結果</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRebatePros()
        {
            var userInfo = await GetUserInfoWithoutAvailable(GetUserId());
            var userId = GetUserId();
            var cacheKey = string.Format(CacheKeyHelper.ApiRebatePros, userId);
            var now = DateTime.Now;
            var cacheExpired = DateTime.Now.AddMinutes(3);
            if (now.Day != cacheExpired.Day)
            {
                cacheExpired = cacheExpired.Date;
            }
            var result = await _localInstance.Value.GetOrAddAsync(nameof(GetRebatePros), cacheKey, async () =>
            {
                var userInfo = await GetUserInfoWithoutAvailable(userId);

                return await _spaService.Value.GetRebatePros(userInfo);
            }, cacheExpired);
            return await Ok(result);
        }

        /// <summary>
        /// 取得該期額外資訊
        /// </summary>
        /// <param name="id">彩種代碼</param>
        /// <param name="issueNo">期號</param>
        /// <returns>該期的額外資訊</returns>
        [HttpGet]
        public async Task<IActionResult> GetExtIssueData(string id, string issueNo)
        {
            var model = _spaService.Value.GetExtIssueData(id, issueNo, out var resultMsg);
            if (model == null)
            {
                return await BadRequest(resultMsg);
            }

            return await Ok(model);
        }

        /// <summary>
        /// 取得彩種設定
        /// </summary>
        /// <param name="id">彩種編號</param>
        /// <returns>彩種設定</returns>
        [HttpGet]
        public async Task<IActionResult> GetViewModel(string id, bool isFullscreen = false)
        {
            var model = GetBetViewModel(id, out int lotteryID, orderNo: null, isFullscreen: isFullscreen);
            return await Ok(model);
        }

        /// <summary>
        /// Get lottery order details.
        /// </summary>
        /// <param name="orderId">Lottery order id.</param>
        /// <returns>Lottery order details</returns>

        public async Task<IActionResult> OrderDetails(string orderId)
        {
            var details = _spaService.Value.OrderDetails(orderId, out var userId);
            if (details == null || userId != GetUserId())
            {
                return await BadRequest("订单号无效");
            }

            return await Ok(details);
        }

        /// <summary>
        /// 取得遊戲玩法列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllLotteryInfo()
        {
            var list = _spaService.Value.GetAllLotteryInfo();
            return await Ok(list);
        }

        private IEnumerable<dynamic> GetTrend(LotteryInfo info, int? sort, int searchType, string tab)
        {
            var adapter = _adapters.Value.FirstOrDefault(x => x.LotteryId == info.LotteryID);
            var helper = _helpers.Value.FirstOrDefault(x => x.GameTypeId == adapter.GameTypeId);
            var queryInfo = helper.GetQueryInfo(searchType);
            queryInfo.LotteryId = info.LotteryID;
            var list = _lotteryService.Value.QueryCurrentLotteryInfo(queryInfo);
            List<CurrentLotteryInfo> sortList = null;

            if (sort == 1)
            {
                sortList = list.OrderByDescending(x => x.IssueNo).ToList();
            }
            else
            {
                sortList = list.OrderBy(x => x.IssueNo).ToList();
            }
            ViewBag.LotteryTypeName = info.LotteryType;
            ViewBag.LotteryTypeUrl = info.TypeURL;
            var controller = RouteData.Values["controller"].ToString();
            var action = RouteData.Values["action"].ToString();
            ViewBag.Controller = controller;
            ViewBag.Action = action;
            ViewBag.SearchType = searchType;
            ViewBag.Sort = sort;
            ViewBag.TotalCount = sortList.Count;
            ViewBag.lotteryList = _lotteryService.Value.GetAllLotteryInfo();
            ViewBag.Tab = tab;
            return helper.PrepareTrend(sortList);
        }

        #region AJAX Actions

        /// <summary>
        /// 下期期號
        /// </summary>
        /// <param name="lotteryId">LotteryId</param>
        /// <returns>下期期號</returns>
        public async Task<IActionResult> GetNextIssueNo(int lotteryId)
        {
            var cacheKey = LotteryService.GetNextIssueNoCacheKey(lotteryId);

            var localObj = await _localInstance.Value.GetAsync<CachedObj<CurrentIssueNoViewModel>>(cacheKey);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return await Ok(localObj.Value);
                }
            }

            return await Ok(localObj?.Value);
        }

        /// <summary>
        /// 下期期號
        /// </summary>
        /// <returns>下期期號</returns>
        public async Task<IActionResult> GetNextIssueNos()
        {
            var cacheKey = LotteryService.GetAllIssueNoCacheKey;

            var localObj = await _localInstance.Value.GetAsync<CachedObj<CurrentIssueNoViewModel[]>>(cacheKey);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return await Ok(localObj.Value);
                }
            }

            return await Ok(localObj?.Value);
        }

        /// <summary>
        /// 排行榜清單(HTML)
        /// </summary>
        /// <param name="days">天數</param>
        /// <returns>排行榜清單</returns>
        public async Task<IActionResult> GetRankList(int days)
        {
            return await Ok(_spaService.Value.GetRankList(days));
        }

        /// <summary>
        /// 排行榜清單(JSON)
        /// </summary>
        /// <param name="days">天數</param>
        /// <returns>排行榜清單</returns>
        public async Task<IActionResult> GetRankListItems(int days)
        {
            return await Ok(_spaService.Value.GetRankListItems(days));
        }

        /// <summary>
        /// 今日清單
        /// </summary>
        /// <returns>今日清單</returns>
        public async Task<IActionResult> GetTodaySummary(int lotteryId)
        {
            return await Ok(_spaService.Value.GetTodaySummary(this.GetUserId(), lotteryId));
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlayInfoPostModel model)
        {
            try
            {
                if (Array.Exists(_specialRuleIds, x => x == model.LotteryID))
                {
                    decimal price = model.Price;
                    if (price < 2 || price > 200000)
                        return await BadRequest("数字范围为2~200000");
                    else if (Math.Ceiling(price) != Math.Floor(price))
                        return await BadRequest("不得有小数点");
                }

                var userInfo = await GetUserInfoWithoutAvailable(GetUserId());
                var playInfo = _spaService.Value.PlaceOrder(model, userInfo);

                if (playInfo == null)
                {
                    return await BadRequest("彩种已关闭。");
                }

                if (!string.IsNullOrWhiteSpace(playInfo.PalyID) && playInfo.UserName == CommonHelper.SuccessFlag)
                {
                    try
                    {
                        await _spaService.Value.AddUnawardedSummaryData(userInfo, model);
                    }
                    catch (Exception ex)
                    {
                        _logger.Value.LogError(ex,$"AddUnawardedSummaryData Error" +
                                            $",userInfo:{JsonSerializer.Serialize(userInfo)}" +
                                            $",model:{JsonSerializer.Serialize(model)}");
                    }

                    var response = new
                    {
                        playID = playInfo.PalyID,
                        NoteTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    return await Ok(response);
                }
                else
                {
                    return await BadRequest(playInfo.UserName);
                }
            }
            catch (Exception ex)
            {
                return await BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 期號開獎歷史
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="count"></param>
        /// <param name="nextCursor"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IssueHistory(int lotteryId, int count, string nextCursor)
        {
            return await Ok(_spaService.Value.IssueHistory(lotteryId, count, nextCursor));
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecifyOrderList(int? lotteryId, string status, DateTime? searchDate, string cursor, int pageSize, string roomId = null)
        {
            var userId = this.GetUserId();
            var palyInfoList = await _spaService.Value.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize, roomId);
            if (palyInfoList == null)
            {
                return await BadRequest("订单无效");
            }

            return await Ok(palyInfoList);
        }

        //秘色跟单资讯
        [HttpGet]
        public async Task<IActionResult> GetFollowBet(string palyId, int lotteryId = 0)
        {
            if (!_specialRuleIds.Contains(lotteryId))
            {
                lotteryId = 65;
            }

            var palyInfoList = _spaService.Value.GetFollowBet(palyId, lotteryId);
            if (palyInfoList == null)
            {
                return await BadRequest("订单无效");
            }
            return await Ok(palyInfoList);
        }

        [HttpGet]
        public async Task<IActionResult> GetLotteryCountdownTime(int lotteryId)
        {
            return await Ok(_spaService.Value.GetLotteryCountdownTime(lotteryId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLongData(int lotteryId)
        {
            var cacheKey = LotterySpaService.LongResultCacheName(lotteryId);

            var localObj = await _localInstance.Value.GetAsync<CachedObj<LongData>>(cacheKey);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return await Ok(localObj.Value);
                }
            }

            return await Ok(new LongData());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLongDatas()
        {
            var cacheKey = LotterySpaService.LongResultAllInOne;

            var localObj = await _localInstance.Value.GetAsync<CachedObj<LongData[]>>(cacheKey);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return await Ok(localObj.Value);
                }
            }

            return await Ok(new LongData[0]);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLotteryPlanData(int lotteryId, int planType)
        {
            var cacheKey = LotterySpaService.LotteryPlanCacheName(lotteryId, planType);

            var localObj = await _localInstance.Value.GetAsync<CachedObj<LotteryPlanData>>(cacheKey);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return await Ok(localObj.Value);
                }
            }

            return await Ok(new LotteryPlanData());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLotteryPlanDatas()
        {
            var cacheKey = LotterySpaService.LotteryPlanAllInOne;

            var localObj = await _localInstance.Value.GetAsync<CachedObj<LotteryPlanData[]>>(cacheKey);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return await Ok(localObj.Value);
                }
            }

            return await Ok(new LotteryPlanData[0]);
        }
        
        /// <summary>
        /// 取得該彩種當期期號未開獎注單摘要
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="currentIssueNo"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUnawardedSummary(
            int lotteryId, string currentIssueNo, string? roomId = null)
        {
            TicketUserData ticketUserData = GetUserToken();
            var userInfo = await GetUserInfoWithoutAvailable(ticketUserData.UserId);
    
            return await Ok(await _spaService.Value.GetUnawardedSummary(userInfo, lotteryId, roomId, currentIssueNo));
        }

        protected async Task<IActionResult> Ok(object data)
        {
            return await Task.FromResult(LotterySpaUtil.CreateJsonResult(isSuccess: true, errorMessage: null, data));
        }

        protected async Task<IActionResult> BadRequest(string errorMessage)
        {
            return await Task.FromResult(LotterySpaUtil.CreateJsonResult(errorMessage));
        }

        #endregion AJAX Actions
    }
}
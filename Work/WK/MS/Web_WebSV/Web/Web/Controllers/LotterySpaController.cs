using JxBackendService.Common.Util;
using JxLottery.Services.Adapter;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Web.Extensions;
using Web.Helpers;
using Web.Helpers.Security;
using Web.Models.Base;
using Web.Models.Results;
using Web.Models.SpaTrend;
using Web.Services;

namespace Web.Controllers
{
    public class LotterySpaController : BaseController
    {
        protected readonly ILotteryService _lotteryService;

        private readonly ILotterySpaService _spaService = null;

        /// <summary>
        /// User service.
        /// </summary>

        protected readonly IEnumerable<IBonusAdapter> _adapters;

        protected readonly IEnumerable<ISpaTrendHelper> _helpers;

        protected readonly ILogger<LotterySpaController> _logger;

        protected readonly int[] specialRuleIds = { 65, 66, 67, 68 };

        public LotterySpaController(
            ILotteryService lotteryService,
            ILotterySpaService spaService,
            IUserService userService,
            IEnumerable<IBonusAdapter> adapters,
            IEnumerable<ISpaTrendHelper> helpers,
            ICacheService cacheService,
            ILogger<LotterySpaController> logger
            ) : base(cacheService, userService)
        {
            _lotteryService = lotteryService;
            _spaService = spaService;
            _adapters = adapters;
            _helpers = helpers;
            _logger = logger;
        }

        /// <summary>
        /// 投注頁
        /// </summary>
        /// <param name="id">LotteryCode(路由名稱叫Id)</param>
        /// <returns>投注頁</returns>
        public ActionResult Bet(string id)
        {
            var model = GetBetViewModel(id);
            SetViewBag();

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        /// <summary>
        /// 銀像投注頁
        /// </summary>
        /// <returns>投注頁</returns>
        public ActionResult Index(string orderNo = null)
        {
            TicketUserData ticket = AuthenticationUtil.GetLoginUserFromCache();

            if (ticket.UserId == 0)
            {
                return HttpNotFound();
            }

            var lotteryId = (JxLottery.Adapters.Models.Lottery.LotteryId)Enum.Parse(typeof(JxLottery.Adapters.Models.Lottery.LotteryId), ticket.GameID);

            var gameId = specialRuleIds.Contains((int)lotteryId) ? lotteryId.ToString() : "OMKS";

            var model = GetBetViewModel(gameId, orderNo);
            SetViewBag();

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetLotteryInfo(string lotteryId)
        {
            TicketUserData ticketUserData = GetUserToken();
            var lotteryUserInfo = ticketUserData.CastByJson<LotteryUserData>();

            UserInfo userInfoBase = GetUserInfo(true);
            lotteryUserInfo.RebatePro = userInfoBase.RebatePro;

            return new JsonCamelCaseResult(_spaService.GetLotteryInfo(lotteryId, lotteryUserInfo));
        }

        /// <summary>
        /// 取得彩種資訊
        /// </summary>
        /// <param name="id">彩種編號</param>
        /// <param name="orderNo">跟單編號</param>
        /// <returns>彩種資訊</returns>
        private object GetBetViewModel(string id, string orderNo = null)
        {
            var lottery = _spaService.GetLotteryInfoMethod(id);

            if (lottery == null)
            {
                return null;
            }
            var adapter = this._adapters.FirstOrDefault(x => x.LotteryId == lottery.LotteryID);

            if (adapter == null)
            {
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
                LogoUrl = WebResourceHelper.Content($"~/Content/2015/images/0{lottery.LotteryID}.png"),
                MaxAfterPeriods = After.GetChaseNumberCount(lottery.LotteryID, int.MaxValue)
            };

            var playConfigs = _spaService.GetPlayConfig(adapter, this.GetUserId());

            var sysSetting = this._userService.GetSysSettings();

            var settings = sysSetting == null ? new object() : new
            {
                sysSetting.MaxBetCount,
            };

            if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseCDN"]))
            {
                var cdnUrl = System.Configuration.ConfigurationManager.AppSettings["CDNSite"];
                if (cdnUrl != null)
                {
                    if (cdnUrl[cdnUrl.Length - 1] != '/')
                    {
                        cdnUrl = cdnUrl + "/";
                    }
                }
                settings = sysSetting == null ? new object() : new
                {
                    sysSetting.MaxBetCount,
                    cdnUrl = cdnUrl + "Content/CTS/ClientApp/dist/yinsiang-lottery/img/"
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

            return new { lotteryInfo, playConfigs, settings, msSetting };
        }

        /// <summary>
        /// 設定ViewBag
        /// </summary>
        private void SetViewBag()
        {
            // layout.cshtml for rabbit mq
            ViewBag.UserId = this.GetUserIdStr();
            ViewBag.UserName = this.GetUserName();
            ViewBag.IPAddress = this.GetClientIPAddress();
            ViewBag.StompServiceUrl = GlobalCacheHelper.StompServiceUrl;
            var cookies = Request.Cookies;
            if (cookies[CookieKeyHelper.UniqueID] != null)
            {
                ViewBag.UniqueID = cookies[CookieKeyHelper.UniqueID].Value;
            }
        }

        /// <summary>
        /// 走勢圖
        /// </summary>
        /// <param name="id">LotteryCode(路由名稱叫Id)</param>
        /// <returns>走勢圖</returns>
        public ActionResult Trend(string id, int? sort = null, int searchType = 1, string tab = "comprehensive")
        {
            var lottery = _lotteryService.GetLotteryType()
                .FirstOrDefault(x => string.Equals(x.TypeURL, id, StringComparison.InvariantCultureIgnoreCase));

            if (lottery == null)
            {
                return HttpNotFound();
            }

            var gameTypeId = (JxLottery.Models.Lottery.GameTypeId)_adapters.FirstOrDefault(x => x.LotteryId == lottery.LotteryID)?.GameTypeId;

            var model = GetTrend(lottery, sort, searchType, tab);
            return View($"{gameTypeId}Trend", model);
        }

        /// <summary>
        /// 取得返點
        /// </summary>
        /// <returns>結果</returns>
        [HttpGet]
        public ActionResult GetRebatePro(int lotteryId, int playTypeId, int playTypeRadioId, bool? isSingleRebatePro)
        {
            var userInfo = GetUserInfo(false);

            return Ok(_spaService.GetRebatePro(userInfo, lotteryId, playTypeId, playTypeRadioId, isSingleRebatePro));
        }

        /// <summary>
        /// Get after details.
        /// </summary>
        /// <param name="search">After details search model.</param>
        /// <returns>After details JSON view.</returns>
        [HttpPost]
        public virtual ActionResult GetAfterDetails(string id, AfterDetailsSearchModel search)
        {
            var model = _spaService.GetAfterDetails(id, search, out var resultMsg);
            if (model == null)
            {
                return BadRequest(resultMsg);
            }

            return Ok(model);
        }

        /// <summary>
        /// 取得該期額外資訊
        /// </summary>
        /// <param name="id">彩種代碼</param>
        /// <param name="issueNo">期號</param>
        /// <returns>該期的額外資訊</returns>
        [HttpGet]
        public ActionResult GetExtIssueData(string id, string issueNo)
        {
            var model = _spaService.GetExtIssueData(id, issueNo, out var resultMsg);
            if (model == null)
            {
                return BadRequest(resultMsg);
            }

            return Ok(model);
        }

        /// <summary>
        /// 取得彩種設定
        /// </summary>
        /// <param name="id">彩種編號</param>
        /// <returns>彩種設定</returns>
        [HttpGet]
        public ActionResult GetViewModel(string id)
        {
            var model = GetBetViewModel(id);
            return Ok(model);
        }

        /// <summary>
        /// Get lottery order details.
        /// </summary>
        /// <param name="orderId">Lottery order id.</param>
        /// <returns>Lottery order details</returns>

        public ActionResult OrderDetails(string orderId)
        {
            var details = _spaService.OrderDetails(orderId, out var userId);
            if (details == null || userId != this.GetUserId())
            {
                return BadRequest("订单号无效");
            }

            return Ok(details);
        }

        /// <summary>
        /// 撤销订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CancerOrder(string orderId)
        {
            string result = _spaService.CancerOrder(this.GetUserId(), orderId);

            if (!result.Equals("01"))
            {
                return BadRequest(result);
            }

            return Ok(null);
        }

        /// <summary>
        /// 取得遊戲玩法列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllLotteryInfo()
        {
            var list = _lotteryService.GetAllLotteryInfo();
            return Ok(list);
        }

        private IEnumerable<dynamic> GetTrend(LotteryInfo info, int? sort, int searchType, string tab)
        {
            var adapter = _adapters.FirstOrDefault(x => x.LotteryId == info.LotteryID);
            var helper = _helpers.FirstOrDefault(x => x.GameTypeId == adapter.GameTypeId);
            var queryInfo = helper.GetQueryInfo(searchType);
            queryInfo.LotteryId = info.LotteryID;
            var list = _lotteryService.QueryCurrentLotteryInfo(queryInfo);
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
            ViewBag.lotteryList = _lotteryService.GetAllLotteryInfo();
            ViewBag.Tab = tab;
            return helper.PrepareTrend(sortList);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.LogError(filterContext.Exception, filterContext.Exception.Message);

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = JsonResponse(false, "应用程序发生错误，请重新登录或稍候重试！", null);
            }
            else
            {
                var result = new ViewResult()
                {
                    ViewName = "Error"
                };

                filterContext.Result = result;
            }

            filterContext.ExceptionHandled = true;
        }

        #region AJAX Actions

        /// <summary>
        /// 下期期號
        /// </summary>
        /// <param name="lotteryId">LotteryId</param>
        /// <returns>下期期號</returns>
        public ActionResult GetNextIssueNo(int lotteryId)
        {
            var userID = this.GetUserId();

            return Ok(_spaService.GetNextIssueNo(userID, lotteryId));
        }

        /// <summary>
        /// 排行榜清單(HTML)
        /// </summary>
        /// <param name="days">天數</param>
        /// <returns>排行榜清單</returns>
        public ActionResult GetRankList(int days)
        {
            return Ok(_spaService.GetRankList(days));
        }

        /// <summary>
        /// 排行榜清單(JSON)
        /// </summary>
        /// <param name="days">天數</param>
        /// <returns>排行榜清單</returns>
        public ActionResult GetRankListItems(int days)
        {
            return Ok(_spaService.GetRankListItems(days));
        }

        /// <summary>
        /// 今日清單
        /// </summary>
        /// <returns>今日清單</returns>
        public ActionResult GetTodaySummary(int lotteryId)
        {
            return Ok(_spaService.GetTodaySummary(this.GetUserId(), lotteryId));
        }

        [HttpPost]
        public ActionResult PlaceOrder(PlayInfoPostModel model)
        {
            try
            {
                if (Array.Exists(specialRuleIds, x => x == model.LotteryID))
                {
                    decimal price = model.Price;
                    if (price < 2 || price > 200000)
                        return BadRequest("数字范围为2~200000");
                    else if (Math.Ceiling(price) != Math.Floor(price))
                        return BadRequest("不得有小数点");
                }

                var userInfo = GetUserInfo(true);
                var playInfo = _spaService.PlaceOrder(model, userInfo);

                if (playInfo == null)
                {
                    return BadRequest("彩种已关闭。");
                }

                if (!string.IsNullOrWhiteSpace(playInfo.PalyID) && playInfo.UserName == CommonHelper.SuccessFlag)
                {
                    var response = new
                    {
                        playID = playInfo.PalyID,
                        NoteTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(playInfo.UserName);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        public ActionResult IssueHistory(int lotteryId, int count, string nextCursor)
        {
            return Ok(_spaService.IssueHistory(lotteryId, count, nextCursor));
        }

        [HttpGet]
        public ActionResult GetSpecifyOrderList(int lotteryId, string status, DateTime? searchDate, string cursor, int pageSize)
        {
            var userId = this.GetUserId();
            var palyInfoList = _spaService.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize);
            if (palyInfoList == null)
            {
                return BadRequest("订单无效");
            }

            return Ok(palyInfoList);
        }

        //秘色跟单资讯
        [HttpGet]
        public ActionResult GetFollowBet(string palyId, int lotteryId = 0)
        {
            if (!specialRuleIds.Contains(lotteryId))
            {
                lotteryId = 65;
            }

            var palyInfoList = _spaService.GetFollowBet(palyId, lotteryId);
            if (palyInfoList == null)
            {
                return BadRequest("订单无效");
            }
            return Ok(palyInfoList);
        }

        [HttpGet]
        public ActionResult GetLotteryCountdownTime(int lotteryId)
        {
            var userID = this.GetUserId();
            return Ok(_spaService.GetLotteryCountdownTime(userID, lotteryId));
        }

        [HttpGet]
        public ActionResult GetLongData(int lotteryId)
        {
            return Ok(_spaService.GetLongData(lotteryId));
        }

        [HttpGet]
        public ActionResult GetLotteryPlanData(int lotteryId, int planType)
        {
            return Ok(_spaService.GetLotteryPlanData(lotteryId, planType));
        }

        protected ActionResult JsonResponse(bool isSuccess, string errorMessage, object data)
        {
            var response = new
            {
                isSuccess,
                errorMessage,
                data
            };

            var jsonSerializerSetting = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(response, jsonSerializerSetting),
                ContentEncoding = Encoding.UTF8
            };
        }

        protected ActionResult Ok(object data)
        {
            return JsonResponse(true, string.Empty, data);
        }

        protected ActionResult BadRequest(string errorMessage)
        {
            return JsonResponse(false, errorMessage, null);
        }

        #endregion AJAX Actions
    }
}
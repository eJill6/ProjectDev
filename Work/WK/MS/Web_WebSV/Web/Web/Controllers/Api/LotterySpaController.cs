using JxBackendService.Common.Util;
using Microsoft.Extensions.Logging;
using SLPolyGame.Web.Model;
using System;
using System.Web.Http;
using Web.Helpers;
using Web.Models.Base;
using Web.Services;

namespace Web.Controllers.Api
{
    [Obsolete("這個class目前還沒開始使用所以有些功能尚未開發完全，如果真正開始使用請跟/LotterySpaController同步一下實作")]
    public class LotterySpaController : BaseApiController
    {
        private readonly ILotterySpaService _spaService = null;

        protected readonly ILogger<LotterySpaController> _logger;

        protected readonly int[] specialRuleIds = { 65 };

        public LotterySpaController(
            IUserService userService,
            ICacheService cacheService,
            ILotterySpaService spaService,
            ILogger<LotterySpaController> logger
            ) : base(cacheService, userService)
        {
            _logger = logger;
            _spaService = spaService;
        }

        [HttpGet]
        public IHttpActionResult GetLotteryInfo(string lotteryId)
        {
            TicketUserData ticketUserData = GetUserToken();
            var lotteryUserInfo = ticketUserData.CastByJson<LotteryUserData>();

            UserInfo userInfoBase = GetUserInfo(true);
            lotteryUserInfo.RebatePro = userInfoBase.RebatePro;

            return Ok(_spaService.GetLotteryInfo(lotteryId, lotteryUserInfo));
        }

        /// <summary>
        /// 取得返點
        /// </summary>
        /// <returns>結果</returns>
        [HttpGet]
        public IHttpActionResult GetRebatePro(int lotteryId, int playTypeId, int playTypeRadioId, bool? isSingleRebatePro)
        {
            var userInfo = GetAndStoreUserInfo();

            return Ok(_spaService.GetRebatePro(userInfo, lotteryId, playTypeId, playTypeRadioId, isSingleRebatePro));
        }

        /// <summary>
        /// Get after details.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="search">After details search model.</param>
        /// <returns>After details JSON view.</returns>
        [HttpPost]
        public virtual IHttpActionResult GetAfterDetails(string id, AfterDetailsSearchModel search)
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
        public IHttpActionResult GetExtIssueData(string id, string issueNo)
        {
            var model = _spaService.GetExtIssueData(id, issueNo, out var resultMsg);
            if (model == null)
            {
                return BadRequest(resultMsg);
            }

            return Ok(model);
        }

        /// <summary>
        /// Get lottery order details.
        /// </summary>
        /// <param name="orderId">Lottery order id.</param>
        /// <returns>Lottery order details</returns>

        public IHttpActionResult OrderDetails(string orderId)
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
        public IHttpActionResult CancerOrder(string orderId)
        {
            string result = _spaService.CancerOrder(this.GetUserId(), orderId);

            if (!result.Equals("01"))
            {
                return BadRequest(result);
            }

            return Ok(null);
        }

        /// <summary>
        /// 下期期號
        /// </summary>
        /// <param name="lotteryId">LotteryId</param>
        /// <returns>下期期號</returns>
        public IHttpActionResult GetNextIssueNo(int lotteryId)
        {
            var userID = this.GetUserId();

            return Ok(_spaService.GetNextIssueNo(userID, lotteryId));
        }

        /// <summary>
        /// 排行榜清單(HTML)
        /// </summary>
        /// <param name="days">天數</param>
        /// <returns>排行榜清單</returns>
        public IHttpActionResult GetRankList(int days)
        {
            return Ok(_spaService.GetRankList(days));
        }

        /// <summary>
        /// 排行榜清單(JSON)
        /// </summary>
        /// <param name="days">天數</param>
        /// <returns>排行榜清單</returns>
        public IHttpActionResult GetRankListItems(int days)
        {
            return Ok(_spaService.GetRankListItems(days));
        }

        /// <summary>
        /// 今日清單
        /// </summary>
        /// <returns>今日清單</returns>
        public IHttpActionResult GetTodaySummary(int lotteryId)
        {
            return Ok(_spaService.GetTodaySummary(GetUserId(), lotteryId));
        }

        [HttpPost]
        public IHttpActionResult PlaceOrder(PlayInfoPostModel model)
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

                var userInfo = GetAndStoreUserInfo();
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
        public IHttpActionResult IssueHistory(int lotteryId, int count, string nextCursor)
        {
            return Ok(_spaService.IssueHistory(lotteryId, count, nextCursor));
        }

        [HttpGet]
        public IHttpActionResult GetSpecifyOrderList(int lotteryId, string status, DateTime? searchDate, string cursor, int pageSize)
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
        public IHttpActionResult GetFollowBet(string palyId)
        {
            //暫時寫死目前只供秘色一分快三使用
            int lotteryId = 65;
            var palyInfoList = _spaService.GetFollowBet(palyId, lotteryId);
            if (palyInfoList == null)
            {
                return BadRequest("订单无效");
            }
            return Ok(palyInfoList);
        }

        [HttpGet]
        public IHttpActionResult GetLotteryCountdownTime(int lotteryId)
        {
            var userID = this.GetUserId();
            return Ok(_spaService.GetLotteryCountdownTime(userID, lotteryId));
        }

        /// <summary>
        /// 取得及暫存用戶資訊
        /// </summary>
        /// <returns></returns>
        private UserInfo GetAndStoreUserInfo()
        {
            string userId = this.GetUserId().ToString();

            string key = string.Format(CacheKeyHelper.UserInfo, userId);

            //用户信息缓存5分钟
            UserInfo userInfo = _cacheService.Get(key, DateTime.Now.AddMinutes(5),
                () => _userService.GetUserInfo());

            return userInfo;
        }
    }
}
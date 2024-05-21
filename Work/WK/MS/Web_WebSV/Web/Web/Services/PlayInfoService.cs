using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Helpers;
using Web.Helpers.Security;
using Web.Models.Base;

namespace Web.Services
{
    public class PlayInfoService : IPlayInfoService
    {
        /// <summary>
        /// CacheService
        /// </summary>
        private readonly ICacheService _cacheService = null;

        /// <summary>
        /// SLPolyGameService client.
        /// </summary>
        private readonly ISLPolyGameWebSVService _slPolyGameWebSVService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayInfoService" /> class.
        /// </summary>
        public PlayInfoService(ISLPolyGameWebSVService slPolyGameWebSVService,
            ICacheService cacheService)
        {
            _slPolyGameWebSVService = slPolyGameWebSVService;
            _cacheService = cacheService;
        }

        public string CancerOrder(int userId, string payId)
        {
            return this._slPolyGameWebSVService.CancelOrder(new PalyInfo()
            {
                UserID = userId,
                PalyID = payId
            });
        }

        public SLPolyGame.Web.Model.CursorPagination<PalyInfo> GetFollowBet(string palyId, int lottertId)
        {
            return _slPolyGameWebSVService.GetFollowBet(palyId, lottertId);
        }

        public List<string> GetLatestWinningList()
        {
            return GetLatestWinningList(CacheKeyHelper.LatestWinningsKey, "day");
        }

        public List<WinningListItem> GetLatestWinningListItems(string period)
        {
            string cacheKey = "WEB#LATEST_WINNING_LIST_ITEMS_" + period;
            var cachedData = _cacheService.Get<List<WinningListItem>>(cacheKey);

            if (cachedData == null)
            {
                List<WinningListItem> list = _slPolyGameWebSVService.GetLatestWinningListItems(period);

                if (list.AnyAndNotNull())
                {
                    _cacheService.Set(cacheKey, list, DateTime.Now.AddMinutes(10));
                }
                return list;
            }
            else
            {
                return cachedData;
            }
        }

        public List<string> GetLatestWinningMonthList()
        {
            return GetLatestWinningList(CacheKeyHelper.LatestWinningsMonthKey, "month");
        }

        public List<string> GetLatestWinningWeekList()
        {
            return GetLatestWinningList(CacheKeyHelper.LatestWinningsWeekKey, "week");
        }

        private List<string> GetLatestWinningList(string cacheKey, string period)
        {
            List<string> list = _cacheService.Get<List<string>>(cacheKey);

            if (list == null)
            {
                list = _slPolyGameWebSVService.GetLatestWinningList(period);

                if (list.AnyAndNotNull())
                {
                    _cacheService.Set(cacheKey, list, DateTime.Now.AddMinutes(10));
                }
            }

            return list;
        }

        public PalyInfo GetPlayBet(string playId)
        {
            return _slPolyGameWebSVService.GetPalyIDPalyBet(playId);
        }

        public DateTime GetServerCurrentTime()
        {
            return _slPolyGameWebSVService.GetServerCurrentTime();
        }

        public SLPolyGame.Web.Model.CursorPagination<PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, DateTime searchDate, string cursor, int pageSize)
        {
            return _slPolyGameWebSVService.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize);
        }

        public PalyInfo InsertPlayInfo(PalyInfo playInfo)
        {
            playInfo.ClientIP = IP.GetDoWorkIP();
            TicketUserData userInfo = AuthenticationUtil.GetLoginUserFromCache();
            playInfo.RoomId = userInfo.RoomNo;
            var ret = _slPolyGameWebSVService.InsertPlayInfo(playInfo);

            return ret;
        }

        public List<PalyInfo> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            return _slPolyGameWebSVService.GetPlayBetsByAnonymous(startTime, endTime, gameId).ToList();
        }

        public PalyInfo GetPlayBetByAnonymous(string playId)
        {
            return _slPolyGameWebSVService.GetPlayBetByAnonymous(playId);
        }
    }
}
using ControllerShareLib.Helpers;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Services
{
    public class PlayInfoService : IPlayInfoService
    {
        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        private readonly Lazy<ICacheService> _cacheService;

        private readonly Lazy<IIpUtilService> _ipUtilService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayInfoService" /> class.
        /// </summary>
        public PlayInfoService()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
            _cacheService = DependencyUtil.ResolveService<ICacheService>(); ;
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>(); ;
        }

        public string CancerOrder(int userId, string payId)
        {
            return _slPolyGameWebSVService.Value.CancelOrder(new PalyInfo()
            {
                UserID = userId,
                PalyID = payId
            }).GetAwaiterAndResult();
        }

        public CursorPagination<PalyInfo> GetFollowBet(string palyId, int lottertId)
        {
            return _slPolyGameWebSVService.Value.GetFollowBet(palyId, lottertId).GetAwaiterAndResult();
        }

        public List<string> GetLatestWinningList()
        {
            return GetLatestWinningList(CacheKeyHelper.LatestWinningsKey, "day");
        }

        public List<WinningListItem> GetLatestWinningListItems(string period)
        {
            string cacheKey = "WEB#LATEST_WINNING_LIST_ITEMS_" + period;
            var cachedData = _cacheService.Value.Get<List<WinningListItem>>(cacheKey);

            if (cachedData == null)
            {
                List<WinningListItem> list = _slPolyGameWebSVService.Value.GetLatestWinningListItems(period).GetAwaiterAndResult();

                if (list.AnyAndNotNull())
                {
                    _cacheService.Value.Set(cacheKey, list, DateTime.Now.AddMinutes(10));
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
            List<string> list = _cacheService.Value.Get<List<string>>(cacheKey);

            if (list == null)
            {
                list = _slPolyGameWebSVService.Value.GetLatestWinningList(period).GetAwaiterAndResult();

                if (list.AnyAndNotNull())
                {
                    _cacheService.Value.Set(cacheKey, list, DateTime.Now.AddMinutes(10));
                }
            }

            return list;
        }

        public PalyInfo GetPlayBet(string playId)
        {
            return _slPolyGameWebSVService.Value.GetPalyIDPalyBet(playId).GetAwaiterAndResult();
        }

        public DateTime GetServerCurrentTime()
        {
            return _slPolyGameWebSVService.Value.GetServerCurrentTime().GetAwaiterAndResult();
        }

        public async Task<CursorPaginationTotalData<PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime searchDate, string cursor, int pageSize, string roomId)
        {
            return await _slPolyGameWebSVService.Value.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize, roomId);
        }

        public PalyInfo InsertPlayInfo(PalyInfo playInfo)
        {
            playInfo.ClientIP = _ipUtilService.Value.GetIPAddress();

            var ret = _slPolyGameWebSVService.Value.InsertPlayInfo(playInfo).GetAwaiterAndResult();

            return ret;
        }

        public List<PalyInfo> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            return _slPolyGameWebSVService.Value.GetPlayBetsByAnonymous(startTime, endTime, gameId).GetAwaiterAndResult().ToList();
        }

        public PalyInfo GetPlayBetByAnonymous(string playId)
        {
            return _slPolyGameWebSVService.Value.GetPlayBetByAnonymous(playId).GetAwaiterAndResult();
        }
    }
}
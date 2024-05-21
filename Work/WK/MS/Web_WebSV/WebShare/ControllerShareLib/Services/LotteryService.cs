using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Infrastructure.Jobs;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        private readonly Lazy<ISerTabWebSVService> _serTabWebSVService;

        private readonly Lazy<ICacheService> _cacheService;

        private readonly Lazy<ICache> _localInstance;

        public static string GetRawNextIssueNoCacheKey(int lotteryId) => $"GetRawLotteryInfos#{lotteryId}";

        public static string GetNextIssueNoCacheKey(int lotteryId) => $"GetLotteryInfos#{lotteryId}";

        public static string GetAllIssueNoCacheKey => "GetAllLotteryInfos";

        public LotteryService()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
            _serTabWebSVService = DependencyUtil.ResolveService<ISerTabWebSVService>();
            _cacheService = DependencyUtil.ResolveService<ICacheService>();
            _localInstance = DependencyUtil.ResolveService<ICache>();
        }

        public CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
        {
            return _slPolyGameWebSVService.Value.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor).GetAwaiterAndResult();
        }

        public async Task<CurrentLotteryInfo> GetRawLotteryInfos(int lotteryId)
        {
            var result = await _cacheService.Value.GetLocalCacheAsync<CurrentLotteryInfo>(GetRawNextIssueNoCacheKey(lotteryId));

            if (result != null)
            {
                result.CurrentTime = DateTime.Now;
            }

            return result;
        }

        public async Task<CurrentIssueNoViewModel> GetLotteryInfos(int lotteryId)
        {
            return await _cacheService.Value.GetLocalCacheAsync<CurrentIssueNoViewModel>(
                GetNextIssueNoCacheKey(lotteryId)
                );
        }

        public async Task<CurrentIssueNoViewModel[]> GetNextIssueNos(int[] lotteryIds)
        {
            return await _cacheService.Value.GetLocalCacheAsync<CurrentIssueNoViewModel[]>(
                GetAllIssueNoCacheKey);
        }

        public List<LotteryInfo> GetAllLotteryInfo()
        {
            return _cacheService.Value.GetLocalCache<List<LotteryInfo>>(
                CacheKeyHelper.LotteryTypeKey,
                () =>
                {
                    List<LotteryInfo> lotteryInfos = _serTabWebSVService.Value.GetLotteryType().GetAwaiterAndResult();
                    List<MenuInnerInfo> menuInnerInfos = _slPolyGameWebSVService.Value.GetMenuInnerInfos().GetAwaiterAndResult();

                    foreach (var lotteryInfo in lotteryInfos)
                    {
                        var menuInnerInfo = menuInnerInfos.SingleOrDefault(m => m.RemoteCode == lotteryInfo.LotteryID.ToString());

                        if (menuInnerInfo is not null)
                        {
                            lotteryInfo.IsMaintaining = menuInnerInfo.IsMaintaining;
                        }
                    }

                    return lotteryInfos;
                },
                (data) => DateTime.Now.AddMinutes(10));
        }

        public bool IsFrontsideMenuActive(int lotteryID)
        {
            ThirdPartySubGameCodes? thirdPartySubGameCode = ThirdPartySubGameCodes.GetAll()
                .SingleOrDefault(w =>
                    w.PlatformProduct == PlatformProduct.Lottery &&
                    w.RemoteGameCode == lotteryID.ToString());

            if (thirdPartySubGameCode == null)
            {
                return false;
            }

            var frontSideMainMenu = new FrontSideMainMenu()
            {
                ProductCode = thirdPartySubGameCode.PlatformProduct.Value,
                GameCode = thirdPartySubGameCode.Value
            };

            return _slPolyGameWebSVService.Value.IsFrontsideMenuActive(frontSideMainMenu).GetAwaiterAndResult();
        }

        public async Task<List<PlayTypeInfo>> GetPlayTypeInfo()
        {
            var result = new List<PlayTypeInfo>();
            result = await _localInstance.Value.GetOrAddAsync<List<PlayTypeInfo>>(nameof(GetPlayTypeInfo), CacheKeyHelper.PlayTypeInfoKey, async () =>
            {
                return await _serTabWebSVService.Value.GetPlayTypeInfo();
            }, DateTime.Now.AddMinutes(10));
            return result;
        }

        public async Task<List<PlayTypeRadio>> GetPlayTypeRadio()
        {
            var result = new List<PlayTypeRadio>();
            result = await _localInstance.Value.GetOrAddAsync<List<PlayTypeRadio>>(nameof(GetPlayTypeRadio), CacheKeyHelper.PlayTypeRadioKey, async () =>
            {
                return await _serTabWebSVService.Value.GetPlayTypeRadio();
            }, DateTime.Now.AddMinutes(10));
            return result;
        }

        public TodaySummaryInfo GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryId, int count)
        {
            return _serTabWebSVService.Value.GetTodaySummaryInfo(start, end, lotteryId, count).GetAwaiterAndResult();
        }

        /// <summary>
        /// GetTodaySummaryInfo
        /// </summary>
        /// <param name="lotteryID"></param>
        /// <param name="count"></param>
        /// <param name="LoadType">true，从主库读取，false，从备库读取</param>
        /// <returns></returns>
        public TodaySummaryInfo GetTodaySummaryInfo(int lotteryID, int count, bool? LoadType)
        {
            bool isansy = true;

            if (LoadType == true)
            {
                isansy = false;
            }

            return _serTabWebSVService.Value.GetPlayInfoSummaryInfo(lotteryID, count, isansy).GetAwaiterAndResult();
        }

        public List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            return _serTabWebSVService.Value.QueryCurrentLotteryInfo(query).GetAwaiterAndResult();
        }

        public IEnumerable<LiveGameManage> GetLiveGameManageInfos()
        {
            return _cacheService.Value.GetLocalCache<IEnumerable<LiveGameManage>>(
               CacheKeyHelper.LiveGameManageInfos,
               () =>
               {
                   IOrderedEnumerable<LiveGameManage> liveGameManages = _serTabWebSVService.Value.GetLiveGameManageInfos().GetAwaiterAndResult()
                       .Where(w => w.IsActive)
                       .OrderBy(o => o.Sort);

                   return liveGameManages;
               },
               (data) => DateTime.Now.AddSeconds(5));
        }
    }
}
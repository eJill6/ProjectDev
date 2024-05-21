using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.LongResult;
using ControllerShareLib.Models.LotteryPlan;
using ControllerShareLib.Services;
using JxBackendService.Interface.Service.Config;
using JxLottery.Services.Adapter;
using Microsoft.Extensions.Logging;
using Quartz;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class MemoryCacheLongAndPlanJob : IJob
    {
        private readonly IConfigUtilService _configUtilService;

        private readonly ICacheService _cacheService = null;

        private readonly ILogger<MemoryCacheLongAndPlanJob> _logger = null;

        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private readonly int[] _allOverviewLotteryId;

        protected readonly IEnumerable<IBonusAdapter> _adapters;

        private readonly Dictionary<int, int[]> _allOverviewLongPlan;

        private readonly Dictionary<string, int> _allOverviewPlanMapping;

        public MemoryCacheLongAndPlanJob(ICacheService cacheService,
            IConfigUtilService configUtilService,
            IEnumerable<IBonusAdapter> adapters,
            ILogger<MemoryCacheLongAndPlanJob> logger)
        {
            _adapters = adapters;
            _cacheService = cacheService;
            _configUtilService = configUtilService;
            _allOverviewLotteryId = _configUtilService.Get<int[]>("Default:AllOverviewLotteryIds");
            _allOverviewLongPlan = _configUtilService.Get<Dictionary<int, int[]>>("Default:AllOverviewLongPlans");
            _allOverviewPlanMapping = _allOverviewLongPlan.SelectMany(x => x.Value, (x1, x2) =>
            {
                return new KeyValuePair<string, int>(LotterySpaService.LotteryPlanRedisCacheName(x1.Key, x2), x2);
            }).ToArray().GroupBy(x => x.Key, x => x.Value).ToDictionary(x => x.Key, x => x.FirstOrDefault());
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _lock.WaitAsync();

                await SetupLongData();
                await SetupLotteryPlan();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoryCacheLongAndPlanJob fails");
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task SetupLotteryPlan()
        {
            var keys = _allOverviewPlanMapping.Select(x => x.Key).ToArray();
            var results = await _cacheService.GetByRedisAsync<LotteryPlanData>(keys);
            foreach (var item in results)
            {
                await _cacheService.SetLocalCacheAsync<LotteryPlanData>(LotterySpaService.LotteryPlanCacheName(item.Value.LotteryID, _allOverviewPlanMapping[item.Key]), item.Value, DateTime.Now.AddMinutes(10));
            }
            await _cacheService.SetLocalCacheAsync<LotteryPlanData[]>(LotterySpaService.LotteryPlanAllInOne, results.Select(x => x.Value).ToArray(), DateTime.Now.AddMinutes(10));
        }

        private async Task SetupLongData()
        {
            var keys = _allOverviewLotteryId.Select(x => LotterySpaService.LongResultRedisCacheName(x)).ToArray();
            var result = _cacheService.GetByRedis<LongData>(keys);
            var nextIssues = await Task.WhenAll(_allOverviewLotteryId.Select(x => _cacheService.GetLocalCacheAsync<CurrentLotteryInfo>(LotteryService.GetRawNextIssueNoCacheKey(x))).ToArray());
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    // 規格書說超過兩期才顯示
                    item.Value.LongInfo = item.Value.LongInfo.Where(x => x.Count > 1).ToArray();
                    await _cacheService.SetLocalCacheAsync<LongData>(LotterySpaService.LongResultCacheName(item.Value.LotteryID), item.Value, DateTime.Now.AddMinutes(10));
                    var adapter = _adapters.FirstOrDefault(x => x.LotteryId == item.Value.LotteryID);
                    var nextIssueInfo = nextIssues.FirstOrDefault(x => x.LotteryID == item.Value.LotteryID);
                    item.Value.CurrentIssueNo = nextIssueInfo?.LotteryNo;
                    item.Value.LotteryTime = nextIssueInfo.EndTime;
                    item.Value.LotteryTypeName = nextIssueInfo.LotteryType;
                    item.Value.GameTypeId = adapter.GameTypeId;
                    item.Value.GameTypeName = ((JxLottery.Models.Lottery.GameTypeId)adapter.GameTypeId).ToString();
                }
                await _cacheService.SetLocalCacheAsync<LongData[]>(LotterySpaService.LongResultAllInOne, result.Select(x => x.Value).ToArray(), DateTime.Now.AddMinutes(10));
            }
        }
    }
}
using ControllerShareLib.Infrastructure.Jobs;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.LongResult;
using ControllerShareLib.Models.LotteryPlan;
using ControllerShareLib.Services;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums.Queue;
using JxLottery.Services.Adapter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Infrastructure.HostingService
{
    public class MessageContent
    {
        public string IssueNo { get; set; }

        public string LotteryType { get; set; }

        public int LotteryID { get; set; }

        public string CurrentLotteryNum { get; set; }

        public string CurrentIssueNo { get; set; }

        public string EndTime { get; set; }

        public string CurrentTime { get; set; }
    }

    /// <summary>
    /// Mq訊息的model
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// Mq訊息類別
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        public MessageContent SendContent { get; set; }

        /// <summary>
        /// exchange name
        /// </summary>
        public string SendExchange { get; set; }

        /// <summary>
        /// route key內容
        /// </summary>
        public string SendRoutKey { get; set; }

        /// <summary>
        /// 發送時間
        /// </summary>
        public string SendTime { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string SendType { get; set; }
    }

    public class KJEventService : IHostedService
    {
        private readonly IMessageQueueService _messageQueueService;

        private readonly IEnvironmentService _environmentService;

        private static bool _disposed = false;

        private static bool _isInited = false;

        private static DateTime _lastUpdateTime = DateTime.Now;

        private readonly IConfigUtilService _configUtilService;

        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private readonly ICacheService _cacheService = null;

        private readonly ILogger<KJEventService> _logger = null;

        private readonly int[] _allOverviewLotteryId;

        protected readonly IEnumerable<IBonusAdapter> _adapters;

        private readonly Dictionary<int, int[]> _allOverviewLongPlan;

        private readonly Dictionary<string, int> _allOverviewPlanMapping;

        public KJEventService(ICacheService cacheService,
            IConfigUtilService configUtilService,
            IEnumerable<IBonusAdapter> adapters,
            ILogger<KJEventService> logger,
            IEnvironmentService environmentService)
        {
            _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>().Value;
            _environmentService = environmentService;
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (!_isInited)
                {
                    _logger.LogError($"Init KJEventService...");
                    var coreServiceUrl = _configUtilService.Get("CoreServiceUrl");
                    await MemoryCacheNextIssueJob.SetupNextIssueNo(coreServiceUrl, _cacheService, _allOverviewLotteryId);
                    await SetupLotteryPlan();
                    await SetupLongData();

                    TaskQueueName taskQueueName = TaskQueueName.RefreshLotteryFanout(_environmentService.Application);
                    string firstClientProvidedName = _messageQueueService.GetClientProvidedNames().First();
                    
                    _messageQueueService.StartNewDequeueJob(firstClientProvidedName, taskQueueName, (DoDequeueJobAfterReceivedParam doDequeueJobAfterReceivedParam) =>
                    {
                        if (!_disposed)
                        {
                            try
                            {
                                var entity = JsonConvert.DeserializeObject<MessageEntity>(doDequeueJobAfterReceivedParam.Message);
                                Task.Run(async () =>
                                {
                                    SetupData();
                                    Task.Delay(500).ContinueWith(t =>
                                    {
                                        SetupData();
                                    });
                                    Task.Delay(1000).ContinueWith(t =>
                                    {
                                        SetupData();
                                    });
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "StartNewDequeueJob Fails");
                            }
                            return true;
                        }
                        return false;
                    });
                    _isInited = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartAsync fails");
            }
        }

        private async Task SetupData()
        {
            try
            {
                _lock.Wait();
                if ((DateTime.Now - _lastUpdateTime).TotalSeconds >= 0.5)
                {
                    _lastUpdateTime = DateTime.Now;
                    await SetupLotteryPlan();
                    await SetupLongData();
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _disposed = true;
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
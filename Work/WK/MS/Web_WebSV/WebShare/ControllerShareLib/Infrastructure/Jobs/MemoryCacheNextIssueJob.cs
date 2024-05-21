using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Services;
using JxBackendService.Interface.Service.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Infrastructure.Jobs
{
    public class MemoryCacheNextIssueJob : BaseControllerJob
    {
        private readonly int[] _allOverviewLotteryId;

        private readonly ILogger<MemoryCacheNextIssueJob> _logger;

        private readonly ICacheService _cacheService;

        private readonly IConfigUtilService _configUtilService;

        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public MemoryCacheNextIssueJob(IConfigUtilService configUtilService,
            ICacheService cacheService,
            ILogger<MemoryCacheNextIssueJob> logger)
        {
            _allOverviewLotteryId = ConfigUtilService.Get<int[]>("Default:AllOverviewLotteryIds");
            _configUtilService = configUtilService;
            _cacheService = cacheService;
            _logger = logger;
        }

        protected override async Task DoExecute()
        {
            try
            {
                await _lock.WaitAsync();
                var coreServiceUrl = _configUtilService.Get("CoreServiceUrl");
                await SetupNextIssueNo(coreServiceUrl, _cacheService, _allOverviewLotteryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoryCacheNextIssueJob fails");

                throw;
            }
            finally
            {
                _lock.Release();
            }
        }

        public static async Task SetupNextIssueNo(string coreServiceUrl, ICacheService cacheService, int[] allowLotteryId)
        {

            var url = $"{coreServiceUrl}/SerTabService/GetNextIssueNos?lotteryIds={JsonConvert.SerializeObject(allowLotteryId)}";
            var client = new RestSharp.RestClient();
            var request = new RestSharp.RestRequest(url);
            var query = await client.ExecuteAsync(request);

            if (query != null && query.IsSuccessful)
            {
                var results = JsonConvert.DeserializeObject<CurrentLotteryInfo[]>(query.Content);
                var list = new List<CurrentIssueNoViewModel>();

                foreach (var item in results)
                {
                    var isLottery = false;

                    if (!string.IsNullOrEmpty(item.Lottery_result))
                    {
                        isLottery = true;
                    }

                    var val = new CurrentIssueNoViewModel(
                        item.LotteryNo,
                        item.CurrentTime.ToString("MM/dd/yyyy HH:mm:ss"),
                        item.EndTime.ToString("MM/dd/yyyy HH:mm:ss"),
                        item.PreLotteryNo,
                        isLottery,
                        item.Lottery_result,
                        item.LotteryID
                    );

                    await cacheService.SetLocalCacheAsync<CurrentIssueNoViewModel>(LotteryService.GetNextIssueNoCacheKey(item.LotteryID.Value),
                        val,
                        DateTime.Now.AddMinutes(10));

                    await cacheService.SetLocalCacheAsync(LotteryService.GetRawNextIssueNoCacheKey(item.LotteryID.Value),
                        item,
                        DateTime.Now.AddMinutes(10));

                    list.Add(val);
                }

                await cacheService.SetLocalCacheAsync(LotteryService.GetAllIssueNoCacheKey,
                        list.ToArray(),
                        DateTime.Now.AddMinutes(10));
            }
        }
    }

    public class CurrentIssueNoViewModel
    {
        public string CurrentIssueNo { get; }
        public string CurrentTime { get; }
        public string EndTime { get; }
        public string LastIssueNo { get; }
        public bool LastIssueNoIsLottery { get; }
        public string LastDrawNumber { get; }
        public int? LotteryId { get; }

        public CurrentIssueNoViewModel(string currentIssueNo, string currentTime, string endTime, string lastIssueNo, bool lastIssueNoIsLottery, string lastDrawNumber, int? lotteryId)
        {
            CurrentIssueNo = currentIssueNo;
            CurrentTime = currentTime;
            EndTime = endTime;
            LastIssueNo = lastIssueNo;
            LastIssueNoIsLottery = lastIssueNoIsLottery;
            LastDrawNumber = lastDrawNumber;
            LotteryId = lotteryId;
        }

        public override bool Equals(object? obj)
        {
            return obj is CurrentIssueNoViewModel other &&
                   CurrentIssueNo == other.CurrentIssueNo &&
                   CurrentTime == other.CurrentTime &&
                   EndTime == other.EndTime &&
                   LastIssueNo == other.LastIssueNo &&
                   LastIssueNoIsLottery == other.LastIssueNoIsLottery &&
                   LastDrawNumber == other.LastDrawNumber &&
                   LotteryId == other.LotteryId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CurrentIssueNo, CurrentTime, EndTime, LastIssueNo, LastIssueNoIsLottery, LastDrawNumber, LotteryId);
        }
    }
}
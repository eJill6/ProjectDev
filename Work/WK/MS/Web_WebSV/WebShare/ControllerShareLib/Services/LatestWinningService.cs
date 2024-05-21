using ControllerShareLib.Helpers;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using Microsoft.Extensions.Logging;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Services
{
    /// <summary>
    /// 假資料用
    /// </summary>
    public class FakeWinningListItem
    {
        /// <summary>
        /// 使用者
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 彩種名
        /// </summary>
        public string LotteryName { get; set; }

        /// <summary>
        /// 中獎金額
        /// </summary>
        public string AmountText { get; set; }

        /// <summary>
        /// 下注時間
        /// </summary>
        public DateTime NoteTime { get; set; }

        /// <summary>
        /// 轉換成WinningListItem格式
        /// </summary>
        /// <returns>WinningListItem格式</returns>
        public WinningListItem Convert()
        {
            return new WinningListItem
            {
                AmountText = AmountText,
                LotteryName = LotteryName,
                UserName = UserName
            };
        }
    }

    /// <summary>
    /// 彩票大獎設定
    /// </summary>
    public class WinningLotterySetting
    {
        /// <summary>
        /// 彩種名稱
        /// </summary>
        public string LotteryType { get; set; }

        /// <summary>
        /// 彩種賠率
        /// </summary>
        public decimal Odds { get; set; }

        /// <summary>
        /// 彩種出現比率
        /// </summary>
        public int Rate { get; set; }
    }

    public class LatestWinningSetting
    {
        /// <summary>
        /// 彩種設定
        /// </summary>
        public WinningLotterySetting[] LotteryInfoSettings { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string[] UserNames { get; set; }

        /// <summary>
        /// 注金的區間
        /// </summary>
        public Tuple<int, int> BetRange { get; set; } = null;

        /// <summary>
        /// 使用者數量
        /// </summary>
        public Tuple<int, int>[] UserCounts { set; get; } = null;

        /// <summary>
        /// 使用者數量的最大值
        /// </summary>
        public int UserCountMax { set; get; }

        /// <summary>
        /// 彩種設定的最大值
        /// </summary>
        public int LotteryInfoMax { set; get; }

        /// <summary>
        /// 假資料最多多少筆
        /// </summary>
        public int MaxCount { set; get; }
    }

    /// <summary>
    /// 大獎的服務
    /// </summary>
    public class LatestWinningService
    {
        private readonly Lazy<ILogger<LatestWinningService>> _logger;
        private readonly Lazy<ICacheService> _cacheManager;
        private readonly Lazy<ISLPolyGameWebSVService> _polyGameServiceClient;

        public LatestWinningService()
        {
            _logger = DependencyUtil.ResolveService<ILogger<LatestWinningService>>();
            _cacheManager = DependencyUtil.ResolveService<ICacheService>();
            _polyGameServiceClient = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
        }


        /// <summary>
        /// 大獎公告的字樣
        /// </summary>
        private string LatestWinnningTextFormat { set; get; } = "【{0}】在{1}中喜中<span class=\"red\">{2}元</span>";

        /// <summary>
        /// 大獎相關設定
        /// </summary>
        private LatestWinningSetting Settings { set; get; } = null;

        /// <summary>
        /// 刷新的毫秒
        /// </summary>
        private int RefreshMilliseconds { get; set; } = 10 * 60 * 1000;

        /// <summary>
        /// 計時器
        /// </summary>
        private Timer TimerForSetCache { set; get; }

        /// <summary>
        /// 亂數產生器
        /// </summary>
        private Random _rand { set; get; } = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            TimerForSetCache = new Timer(new TimerCallback(p => RefreshLatestWinnings()), null, Timeout.Infinite, Timeout.Infinite);
            TimerForSetCache.Change(RefreshMilliseconds, Timeout.Infinite);
            InitConfigure();
        }

        private void InitConfigure()
        {
            try
            {
                var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;

                Settings = null;
                string? lottery = configUtilService.Get("FakeWinningLotterySetting");
                string? user = configUtilService.Get("FakeWinningUsernameSetting");
                string? range = configUtilService.Get("FakeWinningBetRangeSetting");
                string? userCounts = configUtilService.Get("FakeWinningCountSetting");
                string? maxCount = configUtilService.Get("FakeWinningMaxCountSetting");

                if (!string.IsNullOrEmpty(lottery) &&
                    !string.IsNullOrEmpty(user) &&
                    !string.IsNullOrEmpty(range) &&
                    !string.IsNullOrEmpty(userCounts) &&
                    !string.IsNullOrEmpty(maxCount))
                {
                    lottery = lottery.Trim();
                    user = user.Trim();
                    range = range.Trim();
                    userCounts = userCounts.Trim();
                    var userCountMax = 0;
                    var lotteryInfoMax = 0;
                    var settings = new LatestWinningSetting()
                    {
                        LotteryInfoSettings = lottery.Split(',').Select(x =>
                        {
                            var split = x.Trim().Split(':');
                            lotteryInfoMax += Convert.ToInt32(split[1]);
                            return new WinningLotterySetting()
                            {
                                LotteryType = split[0],
                                Odds = Convert.ToDecimal(split[2]),
                                Rate = lotteryInfoMax,
                            };
                        }).ToArray(),
                        UserNames = user.Split(',').Select(x => x.Trim()).ToArray(),
                        BetRange = new Tuple<int, int>(Convert.ToInt32(range.Split(',')[0]),
                            Convert.ToInt32(range.Split(',')[1])),
                        UserCounts = userCounts.Split(',').Select(x =>
                        {
                            var split = x.Trim().Split(':');
                            userCountMax += Convert.ToInt32(split[1]);
                            return new Tuple<int, int>(
                                Convert.ToInt32(split[0]),
                                userCountMax);
                        }).ToArray(),
                        UserCountMax = userCountMax,
                        LotteryInfoMax = lotteryInfoMax,
                        MaxCount = Convert.ToInt32(maxCount)
                    };

                    Settings = settings;
                }
            }
            catch (Exception ex)
            {
                _logger.Value.LogError(ex, "排行榜初始化失败，详细信息：" + ex.Message + "，堆：" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 結束
        /// </summary>
        public void End()
        {
            TimerForSetCache.Dispose();
            TimerForSetCache = null;
        }

        /// <summary>
        /// 刷新大獎資訊
        /// </summary>
        private void RefreshLatestWinnings()
        {
            ICacheService cacheManager = _cacheManager.Value;
            ILogger<LatestWinningService> logger = _logger.Value;
            ISLPolyGameWebSVService polyGameServiceClient = _polyGameServiceClient.Value;

            try
            {
                // 只允許一個站台進來做事
                var token = cacheManager.Get<string>(CacheKeyHelper.LatestWinningsFakeTokenKey);
                
                if (string.IsNullOrEmpty(token) &&
                    cacheManager.Set(CacheKeyHelper.LatestWinningsFakeTokenKey,
                        CacheKeyHelper.LatestWinningsFakeTokenKey,
                        DateTime.Now.AddMilliseconds(RefreshMilliseconds - 1000)))
                {
                    // 拉取資料
                    logger.LogInformation("自动获取日排行榜");
                    var listOrigin = polyGameServiceClient.GetLatestWinningListItems("day").ConfigureAwait(false).GetAwaiter().GetResult().ToList();

                    logger.LogInformation("自动获取周排行榜");
                    var weekListOrigin = polyGameServiceClient.GetLatestWinningListItems("week").ConfigureAwait(false).GetAwaiter().GetResult().ToList();

                    var list = listOrigin;
                    var weekList = weekListOrigin;

                    // 如果設定檔有資料 就處理假資料
                    if (Settings != null)
                    {
                        try
                        {
                            var todayEnd = DateTime.Today.AddDays(1);
                            var todayStart = DateTime.Today;
                            var weekStart = DateTime.Today.AddDays(-7);
                            logger.LogInformation("處理假資料");

                            var fake = cacheManager.Get<List<FakeWinningListItem>>(CacheKeyHelper.LatestWinningsFakeWeekKey, true) ?? new List<FakeWinningListItem>();
                            var count = fake.Count(x => x.NoteTime >= todayStart && x.NoteTime < todayEnd);
                            if (count < Settings.MaxCount)
                            {
                                fake.AddRange(CreateFake(Settings.MaxCount - count));
                                fake = fake.Where(x => x.NoteTime >= weekStart).ToList();
                                cacheManager.Set(CacheKeyHelper.LatestWinningsFakeWeekKey, fake);
                            }

                            if (fake.Count > 0)
                            {
                                list.AddRange(fake.Where(x => x.NoteTime >= todayStart).Select(x => x.Convert()));
                                weekList.AddRange(fake.Select(x => x.Convert()));

                                list = list.OrderByDescending(x => Convert.ToDecimal(x.AmountText)).Take(50).ToList();
                                weekList = weekList.OrderByDescending(x => Convert.ToDecimal(x.AmountText)).Take(50).ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "获取排行榜假資料失败，详细信息：" + ex.Message + "，堆：" + ex.StackTrace);
                        }
                    }

                    if (list != null && list.Count > 0)
                    {
                        cacheManager.Set(CacheKeyHelper.LatestWinningsKey, list.Select(x => string.Format(LatestWinnningTextFormat, x.UserName, x.LotteryName, x.AmountText)).ToList());
                        cacheManager.Set(CacheKeyHelper.LatestWinningsItemKey, list);
                    }
                    else
                    {
                        cacheManager.Set(CacheKeyHelper.LatestWinningsKey, new List<string>());
                        cacheManager.Set(CacheKeyHelper.LatestWinningsItemKey, new List<WinningListItem>());
                    }

                    if (weekList != null && weekList.Count > 0)
                    {
                        cacheManager.Set(CacheKeyHelper.LatestWinningsWeekKey, weekList.Select(x => string.Format(LatestWinnningTextFormat, x.UserName, x.LotteryName, x.AmountText)).ToList());
                        cacheManager.Set(CacheKeyHelper.LatestWinningsItemWeekKey, weekList);
                    }
                    else
                    {
                        cacheManager.Set(CacheKeyHelper.LatestWinningsWeekKey, new List<string>());
                        cacheManager.Set(CacheKeyHelper.LatestWinningsItemWeekKey, new List<WinningListItem>());
                    }
                }
                else
                {
                    logger.LogError("本次計算排行榜未取得令符");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取排行榜失败，详细信息：" + ex.Message + "，堆：" + ex.StackTrace);
            }

            if (TimerForSetCache != null)
            {
                TimerForSetCache.Change(RefreshMilliseconds, Timeout.Infinite);
            }
        }

        /// <summary>
        /// 產生假資料
        /// </summary>
        /// <param name="needCount">需要的數量</param>
        /// <returns>假資料的內容</returns>
        private IEnumerable<FakeWinningListItem> CreateFake(int needCount)
        {
            // 假資料邏輯
            var userCount = Settings.UserCounts.FirstOrDefault(x => x.Item2 > _rand.Next(0, Settings.UserCountMax));
            
            return Enumerable.Range(0,
                userCount.Item1 < needCount ? userCount.Item1 : needCount).Select(index =>
            {
                var lotteryInfo = Settings.LotteryInfoSettings.FirstOrDefault(x => x.Rate > _rand.Next(0, Settings.LotteryInfoMax));
                var userNameIndex = _rand.Next(0, Settings.UserNames.Length);
                var betAmount = _rand.Next(Settings.BetRange.Item1, Settings.BetRange.Item2);
                
                return new FakeWinningListItem()
                {
                    UserName = ConvertName(Settings.UserNames[userNameIndex]),
                    LotteryName = lotteryInfo.LotteryType,
                    AmountText = (betAmount * lotteryInfo.Odds).ToString("f2"),
                    NoteTime = DateTime.Now.AddSeconds(index)
                };
            });
        }

        /// <summary>
        /// 混淆使用者名稱
        /// </summary>
        /// <param name="userName">輸入的使用者名稱</param>
        /// <returns>混淆後的使用者名稱</returns>
        private string ConvertName(string userName)
        {
            if (userName.Length > 4)
            {
                userName = userName.Substring(0, userName.Length - 3) + "***";
            }
            else if (userName.Length > 3)
            {
                userName = userName.Substring(0, userName.Length - 2) + "***";
            }
            else if (userName.Length > 2)
            {
                userName = userName.Substring(0, userName.Length - 1) + "***";
            }
            else
            {
                userName = userName + "***";
            }
            return userName;
        }
    }
}
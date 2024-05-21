using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Infrastructure.Jobs;
using ControllerShareLib.Interfaces.Model;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models;
using ControllerShareLib.Models.Api;
using ControllerShareLib.Models.Base;
using ControllerShareLib.Models.LongResult;
using ControllerShareLib.Models.LotteryPlan;
using ControllerShareLib.Models.SpaPlayConfig;
using ControllerShareLib.Models.SpaTrend;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxLottery.Models.Lottery;
using JxLottery.Models.Lottery.Bet;
using JxLottery.Services.Adapter;
using JxLottery.Services.BonusService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SLPolyGame.Web.Model;
using LotteryInfo = SLPolyGame.Web.Model.LotteryInfo;
using PlayTypeInfo = SLPolyGame.Web.Model.PlayTypeInfo;

namespace ControllerShareLib.Services
{
    /// <summary>
    /// 彩票相關服務抽共用
    /// </summary>
    public class LotterySpaService : ILotterySpaService
    {
        private static readonly int[] _allOverviewLotteryId;

        private static readonly Dictionary<int, int[]> _allOverviewLongPlan;

        private readonly Lazy<ILotteryService> _lotteryService;

        private readonly Lazy<ICacheService> _cacheService;

        private readonly Lazy<IPlayInfoService> _playInfoService;

        private readonly Lazy<IEnumerable<IBonusAdapter>> _adapters;

        private readonly Lazy<IEnumerable<ISpaPlayConfig>> _configs;

        private readonly Lazy<IEnumerable<ISpaTrendHelper>> _helpers;

        private readonly Lazy<IEnumerable<IBonusService>> _bonuses;

        private readonly Lazy<ILogger<LotterySpaService>> _logger;

        /// <summary>
        /// 長龍Redis快取的名稱
        /// </summary>
        public static string LongResultRedisCacheName(int lotteryId) => $"Lottery:LongResult_{lotteryId}";

        /// <summary>
        /// 長龍快取的名稱
        /// </summary>
        public static string LongResultCacheName(int lotteryId) => $"Lottery:LongResult_{lotteryId}";

        /// <summary>
        /// 長龍本地快取Key
        /// </summary>
        public static string LongResultAllInOne => $"LongResultAllInOne";

        /// <summary>
        /// 跟單計畫本地快取Key
        /// </summary>
        public static string LotteryPlanAllInOne => $"LotteryPlanAllInOne";

        /// <summary>
        /// 跟單計畫Redis快取的名稱
        /// </summary>
        public static string LotteryPlanRedisCacheName(int lotteryId, int planType) => $"Lottery:LotteryPlan_{lotteryId}_{planType}";

        /// <summary>
        /// 跟單計畫快取的名稱
        /// </summary>
        public static string LotteryPlanCacheName(int lotteryId, int planType) => $"Lottery:LotteryPlan_{lotteryId}_{planType}";

        /// <summary>
        /// 未結算注單統計資訊Redis快取的名稱
        /// </summary>
        private static string LotteryUnawardedSummaryRedisCacheName(
            int lotteryId, string currentIssueNo, int userId, bool isFullScreen) 
            => $"Lottery:LotteryUnawardedSummary:{userId}:{lotteryId}_{currentIssueNo}_{isFullScreen}";
        
        /// <summary>
        /// 未結算注單統計資訊原始資料Redis快取的名稱
        /// </summary>
        private static string LotteryUnawardedSummaryRedisSAddCacheName(
            int lotteryId, string currentIssueNo, int userId, bool isFullScreen) 
            => $"Lottery:LotteryUnawardedSummary:{userId}:SAdd_{lotteryId}_{currentIssueNo}_{isFullScreen}";
        
        static LotterySpaService()
        {
            var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
            _allOverviewLotteryId = configUtilService.Get<int[]>("Default:AllOverviewLotteryIds");
            _allOverviewLongPlan = configUtilService.Get<Dictionary<int, int[]>>("Default:AllOverviewLongPlans");
        }

        public LotterySpaService()
        {
            _lotteryService = DependencyUtil.ResolveService<ILotteryService>();
            _cacheService = DependencyUtil.ResolveService<ICacheService>();
            _playInfoService = DependencyUtil.ResolveService<IPlayInfoService>();
            _adapters = DependencyUtil.ResolveService<IEnumerable<IBonusAdapter>>();
            _configs = DependencyUtil.ResolveService<IEnumerable<ISpaPlayConfig>>();
            _helpers = DependencyUtil.ResolveService<IEnumerable<ISpaTrendHelper>>();
            _bonuses = DependencyUtil.ResolveService<IEnumerable<IBonusService>>();
            _logger = DependencyUtil.ResolveService<ILogger<LotterySpaService>>();
        }

        public Models.LotteryPlayTypeInfo GetLotteryInfo(string lotteryId, ILotteryUserData lotteryUserData)
        {
            var result = new Models.LotteryPlayTypeInfo();
            var id = (JxLottery.Adapters.Models.Lottery.LotteryId)Enum.Parse(typeof(JxLottery.Adapters.Models.Lottery.LotteryId), lotteryId);
            var lottery = GetLotteryInfoMethod(id.ToString());

            if (lottery != null)
            {
                var adapter = _adapters.Value.FirstOrDefault(x => x.LotteryId == lottery.LotteryID);
                var bonus = _bonuses.Value.FirstOrDefault(x => x.LotteryId == lottery.LotteryID);

                if (adapter != null)
                {
                    var playConfigs = GetPlayConfig(adapter).ConfigureAwait(false).GetAwaiter().GetResult();

                    result.Data = new Models.PlayConfigData(lotteryUserData.DepositUrl, playConfigs, bonus, lotteryUserData);
                    result.IsSuccess = true;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public Models.LotteryPlayTypeInfo[] GetLotteryInfo(ILotteryUserData userData)
        {
            var result = new List<Models.LotteryPlayTypeInfo>();
            foreach (var item in _allOverviewLotteryId)
            {
                result.Add(GetLotteryInfo(item.ToString(), userData));
            }
            return result.ToArray();
        }

        public List<RebateSelectItem> GetRebatePro(UserInfo userInfo, int lotteryId, int playTypeId, int playTypeRadioId, bool? isSingleRebatePro)
        {
            var result = new List<RebateSelectItem>();
            var currentIssueResult = _lotteryService.Value.GetRawLotteryInfos(lotteryId).GetAwaiterAndResult();
            var bonus = _bonuses.Value.FirstOrDefault(x => x.LotteryId == lotteryId);
            bonus.CloseDateTime = currentIssueResult.EndTime;
            var rebates = new List<decimal>() { 0.0M };
            var useSingleRebatePro = isSingleRebatePro.HasValue && isSingleRebatePro.Value;

            if (userInfo != null && !useSingleRebatePro)
            {
                rebates.Add((decimal)((float)userInfo.RebatePro + (float)userInfo.AddedRebatePro));
            }
            foreach (var rebate in rebates)
            {
                var playInfo = new PlayInfo()
                {
                    LotteryId = lotteryId,
                    PlayTypeId = playTypeId,
                    PlayTypeRadioId = playTypeRadioId,
                    UserId = userInfo.UserId,
                    Rebate = rebate,
                    UserRebate = (userInfo?.RebatePro ?? 0.0M) + (userInfo?.AddedRebatePro) ?? 0.0M
                };
                var odds = GetRebateOdds(bonus, playInfo);
                var numberOdds = bonus.GetNumberOdds(playInfo);

                if (odds != 0)
                {
                    var item = new RebateSelectItem()
                    {
                        Value = new RebatePro()
                        {
                            Odds = odds,
                            Rebate = rebate
                        },
                        NumberOdds = numberOdds
                    };
                    item.Text = useSingleRebatePro ? odds.ToString() : $"{odds * 2}-{item.RebateText}";
                    result.Add(item);
                }
                else
                {
                    var multi = GetMultiOdds(bonus, playInfo);
                    if (multi != null || multi.Count() > 0)
                    {
                        var item = new RebateSelectItem()
                        {
                            Value = new RebatePro()
                            {
                                Odds = multi.Max(),
                                Rebate = rebate
                            },
                            NumberOdds = numberOdds
                        };
                        item.Text = useSingleRebatePro ? $"{multi.Min()}~{multi.Max()}" : $"{multi.Min() * 2}~{multi.Max() * 2}-{item.RebateText}";
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public string CancerOrder(int userId, string orderId)
        {
            return _playInfoService.Value.CancerOrder(userId, orderId);
        }

        public List<string> GetRankList(int days)
        {
            Dictionary<int, string> cacheKeyMappings = new Dictionary<int, string>()
            {
                {1, CacheKeyHelper.LatestWinningsKey},
                {7, CacheKeyHelper.LatestWinningsWeekKey}
            };

            if (!cacheKeyMappings.ContainsKey(days))
            {
                days = cacheKeyMappings.Keys.First();
            }

            var cacheKey = cacheKeyMappings[days];

            List<string> result = _cacheService.Value.Get<List<string>>(cacheKey);

            if (result == null)
            {
                result = new List<string>();
            }

            return result;
        }

        public List<WinningListItem> GetRankListItems(int days)
        {
            Dictionary<int, string> periodMappings = new Dictionary<int, string>()
            {
                {1, CacheKeyHelper.LatestWinningsItemKey},
                {7, CacheKeyHelper.LatestWinningsItemWeekKey}
            };

            if (!periodMappings.TryGetValue(days, out string period))
            {
                period = periodMappings.Values.First();
            }

            var result = _cacheService.Value.Get<List<WinningListItem>>(period, slideExpire: true);

            if (result == null)
            {
                result = new List<WinningListItem>();
            }

            return result;
        }

        public object GetTodaySummary(int userId, int lotteryId)
        {
            string cacheKey = string.Format("Web#{0}#TodaySummary#{1}", userId, lotteryId);

            TodaySummaryInfo summaryInfo = _cacheService.Value.Get(cacheKey, DateTime.Now.AddSeconds(5),
                () => _lotteryService.Value.GetTodaySummaryInfo(lotteryId, 120, null));

            var model = new TodaySummaryModel();

            if (summaryInfo != null)
            {
                //X日内订单
                List<OrderDetailsModel> list = new List<OrderDetailsModel>();
                if (summaryInfo.Orders != null)
                {
                    foreach (var p in summaryInfo.Orders)
                    {
                        var order = new OrderDetailsModel
                        {
                            LotteryType = p.LotteryType,
                            PalyCurrentNum = p.PalyCurrentNum,
                            CurrentLotteryTime = p.CurrentLotteryTime,
                            NoteMoney = p.NoteMoney,
                            PlayID = p.PalyID,
                            WinMoney = p.WinMoney,
                            NoteTime = p.NoteTime,
                            IsFactionAward = p.IsFactionAward
                        };
                        if (p.IsFactionAward == 1)
                        {
                            if (p.WinMoney > 0)
                            {
                                if (p.WinMoney >= p.NoteMoney)
                                {
                                    order.PrizeMoney = p.WinMoney + p.NoteMoney;
                                }
                                else
                                {
                                    order.PrizeMoney = p.NoteMoney - p.WinMoney;
                                }
                            }
                            else
                            {
                                order.PrizeMoney = p.NoteMoney + p.WinMoney;
                            }
                        }
                        list.Add(order);
                    }
                }

                //今日投注
                model.OrderList = list.Where(p => p.NoteTime >= DateTime.Now.Date && p.NoteTime <= DateTime.Now.AddDays(1).AddSeconds(-1)).ToList();

                //今日开奖
                var previousFourLotteryInfo = summaryInfo.LotteryResults;
                if (previousFourLotteryInfo != null)
                {
                    foreach (var lotteryInfo in previousFourLotteryInfo)
                    {
                        var m = new LotteryModel()
                        {
                            CurrentLotteryNum = lotteryInfo.CurrentLotteryNum,
                            CurrentLotteryTime = lotteryInfo.UpdateTime.GetValueOrDefault(),
                            IssueNo = lotteryInfo.IssueNo,
                            IsLottery = true,
                        };
                        model.LotteryList.Add(m);
                    }
                }

                //盈亏总览

                var s1 = new PlaySummaryModel
                {
                    Scope = "一天内输赢",
                    TotalBonus = Math.Round(summaryInfo.PlaySummaryInfoes[0].TotalBonus ?? 0, 2),
                    TotalNoteMoney = Math.Round(summaryInfo.PlaySummaryInfoes[0].TotalNoteMoney, 2),
                    TotalWinMoney = Math.Round(summaryInfo.PlaySummaryInfoes[0].TotalWinMoney ?? 0, 2)
                };

                var s2 = new PlaySummaryModel
                {
                    Scope = "七天内输赢",
                    TotalBonus = Math.Round(summaryInfo.PlaySummaryInfoes[1].TotalBonus ?? 0, 2),
                    TotalNoteMoney = Math.Round(summaryInfo.PlaySummaryInfoes[1].TotalNoteMoney, 2),
                    TotalWinMoney = Math.Round(summaryInfo.PlaySummaryInfoes[1].TotalWinMoney ?? 0, 2)
                };

                model.SummaryList.Add(s1);
                model.SummaryList.Add(s2);
            }

            // 避免字太長，用...取代
            int maxLotteryTypeLength = 7;
            model.OrderList.Where(x => x.LotteryType.Length > maxLotteryTypeLength).ToList().ForEach(x =>
            {
                string ellipsis = "...";
                x.LotteryType = string.Concat(x.LotteryType.Substring(0, maxLotteryTypeLength), ellipsis);
            });

            var todayBetList = model.OrderList.Select(x => new
            {
                x.PlayID,
                x.LotteryType,
                formattedNoteMoney = x.NoteMoney.HasValue ? x.NoteMoney.Value.ToString() : string.Empty,
                x.IsFactionAward,
                formattedWinMoney = x.WinMoney.HasValue ? x.WinMoney.Value.ToString("f4") : "---",
                x.PalyCurrentNum,
                formattedNoteTime = x.NoteTime.HasValue ? x.NoteTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty
            }).ToList();

            var todayDrawList = model.LotteryList.Select(x => new
            {
                x.IssueNo,
                x.CurrentLotteryNum
            }).ToList();

            var profitLossList = model.SummaryList.Select(x => new
            {
                x.Scope,
                formattedTotalNoteMoney = x.TotalNoteMoney.ToString(),
                formattedTotalBonus = x.TotalBonus.HasValue ? x.TotalBonus.Value.ToString() : string.Empty,
                formattedTotalWinMoney = x.TotalWinMoney.HasValue ? x.TotalWinMoney.Value.ToString() : string.Empty
            }).ToList();

            var response = new { todayBetList, todayDrawList, profitLossList };

            return response;
        }

        public object GetExtIssueData(string id, string issueNo, out string resultMsg)
        {
            resultMsg = string.Empty;
            var lottery = GetLotteryInfoMethod(id);

            if (lottery == null)
            {
                resultMsg = "彩种代码错误";
                return null;
            }

            dynamic model = null;

            var adapter = _adapters.Value.FirstOrDefault(x => x.LotteryId == lottery.LotteryID);
            var helper = _helpers.Value.FirstOrDefault(x => x.GameTypeId == adapter.GameTypeId);
            var currentLotteryInfo = _lotteryService.Value.QueryCurrentLotteryInfo(new CurrentLotteryQueryInfo()
            {
                Count = 1,
                LotteryId = lottery.LotteryID,
                StartIssueNo = issueNo,
                EndIssueNo = issueNo,
            }).FirstOrDefault(x => x.IssueNo == issueNo);
            if (currentLotteryInfo == null)
            {
                resultMsg = "期号错误";
                return null;
            }

            if (helper is IHelperExtIssueData dataHelper)
            {
                model = dataHelper.GetExtIssueData(currentLotteryInfo);
            }

            return model;
        }

        public AfterDetailsModel GetAfterDetails(string id, AfterDetailsSearchModel search, out string resultMsg)
        {
            resultMsg = string.Empty;
            var lottery = GetLotteryInfoMethod(id);
            var lotteryId = lottery.LotteryID;
            var maxPeriods = After.GetChaseNumberCount(lotteryId, int.MaxValue);

            if (search.Periods > maxPeriods)
            {
                resultMsg = string.Format("最大允许追号{0}期！", maxPeriods);
                return null;
            }

            decimal afterMoney = Convert.ToDecimal(Convert.ToDecimal(search.SingleMoney).ToString("#0.####"));
            // 20180731 Yark 依需求修改驗證單注金額不得多於小數第4位
            if ((search.BetMoney % 1).ToString().Length - 2 > 3 || (afterMoney % 1).ToString().Length - 2 > 3)
            {
                resultMsg = "投注金额最大为三位小数";
                return null;
            }

            var kind = After.GetKind(lotteryId);
            var now = _playInfoService.Value.GetServerCurrentTime();
            var periods = After.CreatePeriods(search.Periods, kind, search.CurrentIssueNo, now, lotteryId);
            List<ZhStrct> afterDetails = null;
            string msg = "";

            switch (search.AfterType)
            {
                case AfterType.Profit:
                    search.PrizeMoney = double.Parse((search.PrizeMoney / 2.0).ToString("f3"));
                    afterDetails = After.ProfitRate(search.Periods, search.ProfitRate, search.BetMoney, search.SingleMoney, search.PrizeMoney, search.Multiple);
                    if (search.BetMoney * search.PrizeMoney - search.SingleMoney < 0)
                    {
                        msg = "您的追号方案会导致亏损，确认追号吗？";
                    }
                    break;

                case AfterType.SameMultilple:
                    afterDetails = After.ProfitRate(search.Periods, search.SingleMoney, search.Multiple);
                    if (search.BetMoney * search.PrizeMoney - search.SingleMoney < 0)
                    {
                        msg = "您的追号方案会导致亏损，确认追号吗？";
                    }
                    break;

                case AfterType.DoubleMultilple:
                    afterDetails = After.ProfitRate(search.Periods, search.Interval, search.SingleMoney, search.Multiple);
                    break;
            }

            if (afterDetails == null)
            {
                resultMsg = "您的投注方案会导致亏损，请重新选择！";
                return null;
            }

            if (periods.Count != afterDetails.Count)
            {
                resultMsg = "追号异常，请重新追号！";
                return null;
            }

            var model = new AfterDetailsModel();
            model.msg = msg;
            for (int i = 0; i < periods.Count; i++)
            {
                afterDetails[i].Multiple = afterDetails[i].Multiple == 0 ? 0 : afterDetails[i].Multiple;
                var detailModel = new AfterDetailModel()
                {
                    Num = afterDetails[i].Num,
                    IssueNo = periods[i],
                    Money = afterDetails[i].Money,
                    Multiple = afterDetails[i].Multiple
                };
                model.AfterDetails.Add(detailModel);
            }

            return model;
        }

        public object OrderDetails(string orderId, out int userId)
        {
            userId = 0;
            var details = _playInfoService.Value.GetPlayBet(orderId);
            if (details == null)
            {
                return null;
            }
            userId = details.UserID.Value;
            var model = new OrderDetailsModel()
            {
                PlayID = details.PalyID,
                UserID = details.UserID,
                CurrentLotteryNum = details.CurrentLotteryNum,
                LotteryType = details.LotteryType,
                NoteMoney = details.NoteMoney,
                NoteNum = details.NoteNum,
                NoteTime = details.NoteTime,
                PalyCurrentNum = details.PalyCurrentNum,
                PalyNum = details.PalyNum,
                PlayTypeName = details.PlayTypeName ?? string.Empty,
                PlayTypeRadioName = details.PlayTypeRadioName,
                RebatePro = details.RebatePro,
                RebateProMoney = details.RebateProMoney,
                SingleMoney = details.SingleMoney,
                StFactionAward = details.StFactionAward,
                WinMoney = details.WinMoney,
                WinNum = details.WinNum,
                WinPossMoney = details.WinPossMoney,
                CurrentLotteryTime = details.CurrentLotteryTime,
                Multiple = details.Multiple,
                CurrencyUnit = details.CurrencyUnit,
                Ratio = details.Ratio
            };
            if (model.Multiple > 0)
            {
                model.SingleMoney = model.SingleMoney / model.Multiple;
            }
            model.PalyNum = model.PalyNum.Replace("L", "龙").Replace("H", "虎");
            if (model.PlayTypeName.Contains("大小单双"))
            {
                model.PalyNum = model.PalyNum.Replace("1", "大").Replace("2", "小").Replace("3", "单").Replace("4", "双");
            }
            else if (string.IsNullOrEmpty(details.SourceType) || details.SourceType == "oldweb" || details.SourceType == "flash")
            {
                if (model.PlayTypeName.Contains("大小"))
                {
                    model.PalyNum = model.PalyNum.Replace("1", "大").Replace("0", "小");
                }
                if (model.PlayTypeName.Contains("单双"))
                {
                    model.PalyNum = model.PalyNum.Replace("1", "单").Replace("0", "双");
                }
            }

            // 單注金額文字
            string singleBetMoneyText = string.Empty;

            if (model.Multiple > 0)
            {
                singleBetMoneyText = string.Format("{0}x{1} 追号{2}倍", model.SingleMoney.GetValueOrDefault().ToString("N3"), model.Multiple, model.Multiple);
            }
            else
            {
                singleBetMoneyText = model.SingleMoney.GetValueOrDefault().ToString("N3");
            }

            // 倍數文字
            string multipleText = string.Empty;

            if (model.CurrencyUnit.HasValue)
            {
                var basicRate = 1;
                var unitName = string.Empty;
                int tempRate;
                while (basicRate < 10000)
                {
                    if (int.TryParse((model.CurrencyUnit.Value * basicRate).ToString(), System.Globalization.NumberStyles.Number, null, out tempRate))
                    {
                        switch (basicRate)
                        {
                            case 1:
                                unitName = "元";
                                break;

                            case 10:
                                unitName = "角";
                                break;

                            case 100:
                                unitName = "分";
                                break;

                            case 1000:
                                unitName = "厘";
                                break;
                        }
                        break;
                    }
                    basicRate *= 10;
                }

                multipleText = string.Format("1{0} X {1}倍", unitName, model.Ratio.Value);
            }

            // 賠率文字
            string oddsText = details.Odds.ToString();

            // 投注金額文字
            string noteMoneyText = model.NoteMoney.GetValueOrDefault().ToString("N4");

            // 中奖金额文字
            string winPossMoneyText = model.WinPossMoney.ToString("N4");

            // 交易時間文字
            string noteTimeText = model.NoteTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss");

            // 返點文字
            string rebateProText = model.RebatePro.GetValueOrDefault().ToString("N4");

            // 盈虧文字
            string winMoneyText = model.WinMoney.GetValueOrDefault().ToString("N4");

            // 訂單狀態
            bool notFactionAward = model.StFactionAward == "未开奖";
            bool canCancel = notFactionAward && model.CurrentLotteryTime != null;

            var response = new
            {
                OrderId = model.PlayID,
                model.PalyNum,
                model.NoteNum,
                singleBetMoneyText,
                multipleText,
                model.PalyCurrentNum,
                noteMoneyText,
                model.LotteryType,
                model.PlayTypeName,
                model.WinNum,
                model.RebateProMoney,
                model.CurrentLotteryNum,
                oddsText,
                winPossMoneyText,
                noteTimeText,
                rebateProText,
                notFactionAward,
                canCancel,
                model.StFactionAward,
                winMoneyText
            };

            return response;
        }

        /// <summary>
        /// 下期期號
        /// </summary>
        /// <returns>下期期號</returns>
        public async Task<CurrentIssueNoViewModel> GetNextIssueNo(int lotteryId)
        {
            return await _lotteryService.Value.GetLotteryInfos(lotteryId);
        }

        /// <summary>
        /// 下期期號
        /// </summary>
        /// <returns>下期期號</returns>
        public async Task<CurrentIssueNoViewModel[]> GetNextIssueNos()
        {
            return await _lotteryService.Value.GetNextIssueNos(_allOverviewLotteryId);
        }

        public PalyInfo PlaceOrder(PlayInfoPostModel model, UserInfo userInfo)
        {
            var playInfo = GeneratePlayInfo(model, userInfo);

            if (playInfo == null)
            {
                return playInfo;
            }

            var habitRebatePro = 0M;
            if (!float.IsNaN(model.HabitRebatePro))
            {
                habitRebatePro = (decimal)model.HabitRebatePro;
            }
            // Sets RebatePro.
            playInfo.RebatePro = habitRebatePro;

            playInfo.BateIsChange = model.HabitRebatePro != (model.CustomerBonusPct ?? -1);

            // Post order to server.
            return _playInfoService.Value.InsertPlayInfo(playInfo);
        }

        /// <summary>
        /// Generate play info instance of interface.
        /// </summary>
        /// <param name="model">Post play info model.</param>
        /// <param name="userInfo">userInfo</param>
        /// <returns>Play info instance</returns>
        private PalyInfo GeneratePlayInfo(PlayInfoPostModel model, UserInfo userInfo)
        {
            var lottery = _lotteryService.Value.GetAllLotteryInfo().FirstOrDefault(x => x.LotteryID == model.LotteryID);

            if (lottery == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(model.RoomId))
            {
                var ticketUserData = AuthenticationUtil.GetLoginUserFromCache();
                model.RoomId = ticketUserData.RoomNo;
            }

            if (!CheckLotteryIsActive(model, lottery)) return null;

            var playInfo = new PalyInfo()
            {
                PalyCurrentNum = model.CurrentIssueNo,
                PalyNum = model.SelectedNums,
                LotteryID = model.LotteryID,
                LotteryType = lottery.LotteryType,
                PlayTypeID = model.PlayType,
                PlayTypeRadioID = model.PlayTypeRadio,
                UserID = userInfo.UserId,
                UserName = userInfo.UserName,
                NoteNum = model.Amount,
                SingleMoney = model.Price,
                NoteMoney = model.Amount * model.Price,
                NoteTime = DateTime.Now,
                IsFactionAward = 0,
                CurrencyUnit = model.CurrencyUnit,
                Ratio = model.Ratio,
                RoomId = model.RoomId,
            };

            return playInfo;
        }

        private bool CheckLotteryIsActive(PlayInfoPostModel model, LotteryInfo lottery)
        {
            var isFullScreen = model.RoomId.IsNullOrEmpty()
                               || !int.TryParse(model.RoomId, out var roomId)
                               || roomId <= 0;

            switch (isFullScreen)
            {
                case true when !_lotteryService.Value.IsFrontsideMenuActive(model.LotteryID):
                case false when !_lotteryService.Value.GetLiveGameManageInfos().Any(x =>
                    x.LotteryId == model.LotteryID && x.IsActive):
                    return false;

                default:
                    return true;
            }
        }

        public object IssueHistory(int lotteryId, int count, string nextCursor)
        {
            if (count <= 0)
            {
                count = 10;
            }

            // 2022-06 固定抓5日
            DateTime end = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            DateTime start = end.AddDays(-5);

            CursorPagination<CurrentLotteryInfo> result = new CursorPagination<CurrentLotteryInfo>();

            //第一頁進來的資料做短暫Cache
            if (string.IsNullOrEmpty(nextCursor))
            {
                var cacheKey = string.Format(CacheKeyHelper.IssueHistoryFirstPage, lotteryId, count);

                result = _cacheService.Value.GetLocalCache(
                    cacheKey,
                    () => _lotteryService.Value.GetCursorPaginationDrawResult(lotteryId, start, end, count, nextCursor),
                    (data) =>
                    {
                        if (data != null && data.Data != null && data.Data.Any())
                        {
                            var maxCurrentLotteryTime = data.Data.Max(x => x.CurrentLotteryTime);

                            if (maxCurrentLotteryTime > DateTime.Now)
                            {
                                return DateTime.Now.AddSeconds(1);
                            }
                        }
                        return DateTime.Now.AddSeconds(5);
                    });
            }
            else
            {
                result = _lotteryService.Value.GetCursorPaginationDrawResult(lotteryId, start, end, count, nextCursor);
            }

            var response = new
            {
                result.NextCursor,
                list = result.Data.Select(x => new
                {
                    x.IssueNo,
                    x.CurrentLotteryNum
                }).ToList()
            };

            return response;
        }

        public async Task<FollowBetResponse> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime? searchDate, string cursor, int pageSize, string roomId)
        {
            if (!searchDate.HasValue)
            {
                searchDate = DateTime.Now;
            }

            CursorPaginationTotalData<PalyInfo> palyInfoList = await _playInfoService.Value.GetSpecifyOrderList(userId, lotteryId, status, searchDate.Value, cursor, pageSize, roomId);

            if (palyInfoList?.Data is null)
            {
                return null;
            }

            FollowBetResponse response = new FollowBetResponse();
            List<PalyInfoViewModel> palyInfoViewModels = new List<PalyInfoViewModel>();

            var data = palyInfoList.Data;
            //往下搜尋用到的id
            string nextCursor = palyInfoList.NextCursor;

            //判斷柱單輸贏
            if (status == Models.Enums.AwardStatus.Won.ToString())
            {
                data = data.Where(p => p.IsWin == true).ToList();
            }
            else if (status == Models.Enums.AwardStatus.Lost.ToString())
            {
                data = data.Where(p => p.IsWin == false).ToList();
            }

            string moneyFormat = "0.##"; // 有小數顯示2位，沒有就整數

            //改成前端要的格式
            foreach (var l in data)
            {
                var playInfoViewModel = new PalyInfoViewModel();
                playInfoViewModel.LotteryId = l.LotteryID;
                playInfoViewModel.LotteryType = l.LotteryType;
                playInfoViewModel.PlayTypeName = l.PlayTypeName;
                playInfoViewModel.PlayTypeRadioName = l.PlayTypeRadioName;
                playInfoViewModel.Odds = l.Odds;
                playInfoViewModel.NoteTime = l.NoteTime.HasValue ? l.NoteTime.Value.ToFormatDateTimeString() : string.Empty;

                if (l.NoteMoney != 0 || l.NoteMoney != null)
                {
                    playInfoViewModel.NoteMoneyText = "-" + l.NoteMoney.GetValueOrDefault().ToString(moneyFormat);
                }
                else
                {
                    playInfoViewModel.NoteMoneyText = decimal.Zero.ToString(moneyFormat);
                }

                playInfoViewModel.PrizeMoney = Math.Floor(l.WinPossMoney * 100) / 100;
                playInfoViewModel.PrizeMoneyText = playInfoViewModel.PrizeMoney >= 0 ? "+" + playInfoViewModel.PrizeMoney.ToString(moneyFormat) : playInfoViewModel.PrizeMoney.ToString(moneyFormat);
                playInfoViewModel.Status = l.Status;
                playInfoViewModel.StatusText = l.StFactionAward;
                playInfoViewModel.PalyNum = l.PalyNum;
                playInfoViewModel.IssueNo = l.PalyCurrentNum;
                playInfoViewModel.NoteNum = l.NoteNum;
                playInfoViewModel.WinMoney = l.WinMoney;
                playInfoViewModel.WinMoneyText = playInfoViewModel.WinMoney.GetValueOrDefault().ToString(moneyFormat);
                playInfoViewModel.PlayModeId = l.UserType;
                playInfoViewModel.PlayTypeId = l.PlayTypeID.GetValueOrDefault();
                playInfoViewModel.PlayTypeRadioId = l.PlayTypeRadioID.GetValueOrDefault();

                palyInfoViewModels.Add(playInfoViewModel);
            }

            response.DataDetail = palyInfoViewModels;
            response.NextCursor = nextCursor;
            response.TotalBetCount = palyInfoList.TotalBetCount;
            response.TotalPrizeMoney = palyInfoList.TotalPrizeMoney;
            response.TotalWinMoney = palyInfoList.TotalWinMoney;
            return response;
        }

        //秘色跟单资讯
        public FollowBetResponse GetFollowBet(string palyId, int lotteryId)
        {
            var palyInfoList = _playInfoService.Value.GetFollowBet(palyId, lotteryId);

            if (palyInfoList == null)
            {
                return null;
            }

            var response = new FollowBetResponse();
            var data = palyInfoList.Data;
            string moneyFormat = "0.##"; // 有小數顯示2位，沒有就整數

            //改成前端要的格式
            var list = data.Select(item => new PalyInfoViewModel
            {
                LotteryId = item.LotteryID,
                LotteryType = item.LotteryType,
                PlayTypeName = item.PlayTypeName,
                PlayTypeRadioName = item.PlayTypeRadioName,
                Odds = item.Odds,
                NoteTime = item.NoteTime.HasValue ? item.NoteTime.Value.ToFormatDateTimeString() : string.Empty,
                Status = item.Status,
                StatusText = item.StFactionAward,
                PalyNum = item.PalyNum,
                IssueNo = item.PalyCurrentNum,
                NoteNum = item.NoteNum,
                PlayModeId = item.UserType,
                PlayTypeId = item.PlayTypeID.GetValueOrDefault(),
                PlayTypeRadioId = item.PlayTypeRadioID.GetValueOrDefault(),
                NoteMoneyText = item.NoteMoney != 0 || item.NoteMoney != null ? item.NoteMoney.GetValueOrDefault().ToString(moneyFormat) : decimal.Zero.ToString(moneyFormat)
            }).ToList();

            response.DataDetail = list;

            return response;
        }

        public string GetLotteryCountdownTime(int lotteryId)
        {
            CurrentLotteryInfo currentLotteryInfo = _lotteryService.Value.GetRawLotteryInfos(lotteryId).GetAwaiterAndResult();
            //毫秒轉秒
            string response = ((int)(currentLotteryInfo.RemainTime / 1000)).ToString();

            return response;
        }

        public SLPolyGame.Web.Model.LotteryInfo GetLotteryInfoMethod(string lotteryCode)
        {
            return _lotteryService.Value.GetAllLotteryInfo()
                .FirstOrDefault(x => string.Equals(x.TypeURL, lotteryCode, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 舊彩種取的多重賠率的方法
        /// </summary>
        /// <param name="service">獎金服務</param>
        /// <param name="playInfo">注單資訊</param>
        /// <returns>多重賠率</returns>
        private List<decimal> GetMultiOdds(IBonusService service, PlayInfo playInfo)
        {
            return service.GetMultiOdds(playInfo);
        }

        /// <summary>
        /// 舊彩種取得單一賠率的方法
        /// </summary>
        /// <param name="service">獎金服務</param>
        /// <param name="playInfo">注單資訊</param>
        /// <returns>賠率</returns>
        private decimal GetRebateOdds(IBonusService service, PlayInfo playInfo)
        {
            return service.GetRebateOdds(playInfo);
        }

        public async Task<Dictionary<int, LotteryRebateInfo[]>> GetRebatePros(UserInfo userInfo)
        {
            var result = new Dictionary<int, LotteryRebateInfo[]>();
            var playTypeInfos = await _lotteryService.Value.GetPlayTypeInfo();
            var playTypeRadioInfos = await _lotteryService.Value.GetPlayTypeRadio();
            foreach (var lotteryId in _allOverviewLotteryId)
            {
                if (!result.ContainsKey(lotteryId))
                {
                    result.Add(lotteryId, GetRebatePro(userInfo,lotteryId, playTypeInfos, playTypeRadioInfos).ToArray());
                }
            }
            
            return result;
        }
        
        public async Task<bool> AddUnawardedSummaryData(UserInfo userInfo, PlayInfoPostModel model)
        {
            var isFullScreen = model.RoomId == "0";
            var cacheKey = LotteryUnawardedSummaryRedisSAddCacheName(
                model.LotteryID, model.CurrentIssueNo, userInfo.UserId, isFullScreen);

            var dateTimeNow = DateTime.Now;
            
            var values = Array.Empty<UnawardedSummaryDataRowModel>();
            
            foreach (var playNumber in model.SelectedNums.Split("|"))
            {
                var playNumberInfo = playNumber.Split(" ");
                var playType = playNumberInfo[0];
                var number = playNumberInfo[1];

                values = values.Append(new UnawardedSummaryDataRowModel
                {
                    PlayType = playType,
                    Number = number,
                    Price = model.Price,
                    Count = 1,
                    CreateTime = dateTimeNow,
                }).ToArray();
            }

            if (!values.Any())
            {
                return false;
            }
            
            return await _cacheService.Value.SAddAsync(cacheKey, values, DateTime.Now.AddMinutes(3));
        }
        
        public async Task<List<UnawardedSummaryApiResponse>> GetUnawardedSummary(UserInfo userInfo, 
            int lotteryId, string roomId, string currentIssueNo)
        {
            var isFullScreen = roomId == "0";

            return _cacheService.Value.GetOrSetByRedis(
                LotteryUnawardedSummaryRedisCacheName(lotteryId, currentIssueNo, 
                    userInfo.UserId, isFullScreen),
                () =>
                {
                    var playTypeInfos = 
                            _lotteryService.Value.GetPlayTypeInfo().ConfigureAwait(false).GetAwaiter().GetResult();
                    var playTypeRadioInfos = 
                        _lotteryService.Value.GetPlayTypeRadio().ConfigureAwait(false).GetAwaiter().GetResult();
        
                    var rebatePros = GetRebatePro(userInfo, lotteryId, playTypeInfos, playTypeRadioInfos);

                    var cacheKey = LotteryUnawardedSummaryRedisSAddCacheName
                        (lotteryId, currentIssueNo, userInfo.UserId, isFullScreen);

                    var dataRows = 
                        _cacheService.Value.SMembersAsync<UnawardedSummaryDataRowModel>(cacheKey).ConfigureAwait(false).GetAwaiter().GetResult();
                    
                    var unawardedSummaryInfos = new List<UnawardedSummaryApiResponse>();
        
                    foreach (var rebateSelectItem in rebatePros)
                    {
                        var numberOddsConverted = new List<UnawardedSummaryNumberOddsModel>();
        
                        foreach (var numberOdd in rebateSelectItem.NumberOdds)
                        {
                            var categoryAndValue = numberOdd.Key.Split(' ');
                            var category = categoryAndValue[0];
                            var value = categoryAndValue.Length > 1 ? categoryAndValue[1] : "";

                            var count = 0;
                            var totalAmountString = "0";
                            
                            if (dataRows != null && dataRows.Any())
                            {
                                var unawardedSummaryDatas = dataRows.Where(d => d.PlayType == category);
                                count = unawardedSummaryDatas.Count();
                                totalAmountString = unawardedSummaryDatas
                                    .Where(d => d.Number == value)
                                    .Sum(d => d.Price)
                                    .ToString();
                            }
                            
                            var existingCategory = numberOddsConverted.Find(c => c.Category == category);
        
                            if (existingCategory == null)
                            {
                                existingCategory = new UnawardedSummaryNumberOddsModel { Category = category, Count = count, Values = new List<UnawardedSummaryOddsModel>() };
                                numberOddsConverted.Add(existingCategory);
                            }
                            
                            existingCategory.Values.Add(new UnawardedSummaryOddsModel { Value = value, TotalAmount = totalAmountString});
                        }
        
                        unawardedSummaryInfos.Add(new UnawardedSummaryApiResponse(numberOddsConverted));
                    }
        
                    return unawardedSummaryInfos;
                },
                    DateTime.Now.AddSeconds(3)
                );
        }

        private IEnumerable<LotteryRebateInfo> GetRebatePro(UserInfo userInfo, int lotteryId, 
            List<PlayTypeInfo> playTypeInfos, List<PlayTypeRadio> playTypeRadioInfos)
        {
            var adapter = _adapters.Value.FirstOrDefault(x => x.LotteryId == lotteryId);
            if (adapter == null) yield break;
            {
                IEnumerable<SLPolyGame.Web.Model.PlayTypeInfo> types = playTypeInfos.Where(x => x.LotteryID == lotteryId);
                foreach (var type in types)
                {
                    IEnumerable<PlayTypeRadio> radios = playTypeRadioInfos.Where(x => x.PlayTypeID == type.PlayTypeID);
                    foreach (var radio in radios)
                    {
                        yield return new LotteryRebateInfo()
                        {
                            GameTypeId = adapter.GameTypeId,
                            LotteryId = adapter.LotteryId,
                            NumberOdds = GetRebatePro(userInfo, lotteryId, type.PlayTypeID, radio.PlayTypeRadioID, true).FirstOrDefault().NumberOdds,
                            PlayTypeId = radio.PlayTypeID.Value,
                            PlayTypeRadioId = radio.PlayTypeRadioID,
                        };
                    }
                }
            }
        }

        public async Task<IList<PlayMode<PlayTypeInfoApiResult>>> GetPlayConfig(IBonusAdapter adapter)
        {
            int lotteryId = adapter.LotteryId;
            ILotteryService lotteryService = _lotteryService.Value;

            CurrentLotteryInfo result = lotteryService.GetRawLotteryInfos(lotteryId).GetAwaiterAndResult();

            var playModes = JsonConvert.DeserializeObject<IList<PlayMode<Models.SpaPlayConfig.PlayTypeInfo>>>(JsonConvert.SerializeObject(_configs.Value.FirstOrDefault(x => x.GameTypeId == adapter.GameTypeId).
                Get(result.EndTime == default(DateTime) ? DateTime.Now : result.EndTime)));

            List<SLPolyGame.Web.Model.PlayTypeInfo> playTypes = (await lotteryService.GetPlayTypeInfo()).Where(x => x.LotteryID == lotteryId).ToList();

            List<PlayTypeRadio> playTypeRadios = (await lotteryService.GetPlayTypeRadio())
                .Where(x => playTypes.Any(info => info.PlayTypeID == x.PlayTypeID)).ToList();

            playModes = playModes.Select(m =>
            {
                m.PlayTypeInfos = m.PlayTypeInfos.Select(i =>
                {
                    i.PlayTypeRadioInfos = i.PlayTypeRadioInfos.Select(r =>
                    {
                        r.Info = playTypeRadios.FirstOrDefault(o =>
                            m.PlayModeId == o.UserType &&
                            adapter.GetPlayTypeId(lotteryId, o.PlayTypeID.Value) == i.BasePlayTypeId &&
                            adapter.GetPlayTypeRadioId(lotteryId, o.PlayTypeRadioID) == r.BasePlayTypeRadioId);

                        return r;
                    }).Where(r => r.Info != null)
                        .ToList();

                    i.Info = playTypes.FirstOrDefault(t =>
                        m.PlayModeId == t.UserType && adapter.GetPlayTypeId(lotteryId, t.PlayTypeID) == i.BasePlayTypeId);

                    return i;
                }).Where(i => i.Info != null && i.PlayTypeRadioInfos.Count > 0)
                .ToList();

                return m;
            }).Where(m => m.PlayTypeInfos.Count > 0)
            .ToList();

            return playModes.Select(x =>
            {
                return new PlayMode<PlayTypeInfoApiResult>
                {
                    PlayModeId = x.PlayModeId,
                    PlayModeName = x.PlayModeName,
                    PlayTypeInfos = x.PlayTypeInfos.Select(p =>
                    {
                        return new PlayTypeInfoApiResult()
                        {
                            Info = p.Info,
                            BasePlayTypeId = p.BasePlayTypeId,
                            PlayTypeEnum = p.PlayTypeEnum,
                            PlayTypeRadioInfos = p.PlayTypeRadioInfos.GroupBy(r => r.Info.TypeModel)
                                .ToDictionary(d => d.Key ?? string.Empty,
                                    d => d.ToList()),
                        };
                    }).ToList()
                };
            }).ToList();
        }

        public LongData GetLongData(int lotteryId)
        {
            var result = _cacheService.Value.GetByRedis<LongData>(LongResultRedisCacheName(lotteryId));

            if (result != null)
            {
                // 規格書說超過兩期才顯示
                result.LongInfo = result.LongInfo.Where(x => x.Count > 1).ToArray();
            }
            else
            {
                result = new LongData();
            }

            return result;
        }

        /// <inheritdoc/>
        public LongData[] GetLongDatas()
        {
            var result = _cacheService.Value.GetByRedis<LongData>(_allOverviewLotteryId.Select(x => LongResultRedisCacheName(x)).ToArray());

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var adapter = _adapters.Value.FirstOrDefault(x => x.LotteryId == item.Value.LotteryID);

                    var nextIssueInfo = _lotteryService.Value.GetRawLotteryInfos(item.Value.LotteryID).GetAwaiterAndResult();

                    item.Value.CurrentIssueNo = nextIssueInfo.LotteryNo;
                    item.Value.LotteryTime = nextIssueInfo.EndTime;
                    item.Value.LotteryTypeName = nextIssueInfo.LotteryType;
                    item.Value.GameTypeId = adapter.GameTypeId;
                    item.Value.GameTypeName = ((JxLottery.Models.Lottery.GameTypeId)adapter.GameTypeId).ToString();

                    // 規格書說超過兩期才顯示
                    item.Value.LongInfo = item.Value.LongInfo.Where(x => x.Count > 1).ToArray();
                }
            }

            return result.Select(x => x.Value).ToArray();
        }

        /// <inheritdoc/>
        public LotteryPlanData GetLotteryPlanData(int lotteryId, int planType)
        {
            return _cacheService.Value.GetByRedis<LotteryPlanData>(LotteryPlanRedisCacheName(lotteryId, planType));
        }

        public LotteryPlanData[] GetLotteryPlanData()
        {
            var keys = _allOverviewLongPlan.SelectMany(x => x.Value, (x1, x2) => LotteryPlanRedisCacheName(x1.Key, x2)).ToArray();
            var result = _cacheService.Value.GetByRedis<LotteryPlanData>(keys);

            return result.Select(x => x.Value).ToArray();
        }

        public SLPolyGame.Web.Model.LotteryInfo[] GetAllLotteryInfo()
        {
            return _lotteryService.Value.GetAllLotteryInfo().Where(x => _allOverviewLotteryId.Contains(x.LotteryID)).ToArray();
        }

        public virtual int[] GetSpecialRuleIds() => new int[] { 65, 66, 67, 68, 69, 70, 71, 72, 73, 74 };
    }
}

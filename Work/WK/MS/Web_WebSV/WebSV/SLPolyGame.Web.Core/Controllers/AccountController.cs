using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Game;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.BLL;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.MSSeal.Models;
using SLPolyGame.Web.MSSeal.Models.Interface;
using System.Collections.Concurrent;

namespace SLPolyGame.Web.Core.Controllers
{
    /// <summary>
    /// 帳號功能控制器
    /// </summary>
    public class AccountController : BaseApiController
    {
        private readonly Lazy<PalyInfo> _playInfoService = null;

        private readonly Lazy<CurrentLotteryInfo> _currentLotteryService = null;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private readonly Lazy<ITPGameAccountReadService> _tpGameAccountReadService;

        private readonly int _searchTpGameOrdersLimitHourRange = 24;

        /// <summary>ctor</summary>
        public AccountController()
        {
            _playInfoService = DependencyUtil.ResolveService<PalyInfo>();
            _currentLotteryService = DependencyUtil.ResolveService<CurrentLotteryInfo>();
            _platformProductService = DependencyUtil.ResolveService<IPlatformProductService>();
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        /// <summary>
        /// 取得訂單
        /// </summary>
        /// <param name="serialNumber">訂單編號</param>
        /// <returns>訂單資訊</returns>
        [HttpGet]
        public ResultModel<MSLotteryBettingResult> GetOrder(string serialNumber)
        {
            var detail = _playInfoService.Value.GetPlayBetByAnonymous(serialNumber);
            var result = new ResultModel<MSLotteryBettingResult>();

            if (detail != null)
            {
                detail.PalyNum = detail.PalyNum.Replace("L", "龙").Replace("H", "虎");
                if (detail.PlayTypeName.Contains("大小单双"))
                {
                    detail.PalyNum = detail.PalyNum.Replace("1", "大").Replace("2", "小").Replace("3", "单").Replace("4", "双");
                }
                else if (string.IsNullOrEmpty(detail.SourceType) || detail.SourceType == "oldweb" || detail.SourceType == "flash")
                {
                    if (detail.PlayTypeName.Contains("大小"))
                    {
                        detail.PalyNum = detail.PalyNum.Replace("1", "大").Replace("0", "小");
                    }
                    if (detail.PlayTypeName.Contains("单双"))
                    {
                        detail.PalyNum = detail.PalyNum.Replace("1", "单").Replace("0", "双");
                    }
                }

                var currentLotteryInfo = _currentLotteryService.Value.QueryCurrentLotteryInfo(new Model.CurrentLotteryQueryInfo()
                {
                    Count = 1,
                    LotteryId = detail.LotteryID.Value,
                    Start = detail.NoteTime,
                    End = detail.CurrentLotteryTime,
                    StartIssueNo = detail.PalyCurrentNum,
                    EndIssueNo = detail.PalyCurrentNum,
                }).FirstOrDefault();

                if (currentLotteryInfo != null)
                {
                    detail.CurrentLotteryNum = currentLotteryInfo.CurrentLotteryNum;
                }

                result.Data = new MSLotteryBettingResult(detail);
                result.Success = true;
            }

            return result;
        }

        /// <summary>
        /// 取得自營彩票訂單
        /// </summary>
        /// <param name="startTime">起始時間</param>
        /// <param name="endTime">結束時間</param>
        /// <param name="gameId">彩票玩法編號</param>
        /// <returns>訂單資訊</returns>
        [HttpGet]
        public ResultModel<MSLotteryBettingResult[]> GetOrders(string startTime, string endTime, string? gameId = null) //給預設值讓swagger判別為非必填
        {
            DateTime startDateTime;
            DateTime endDateTime;

            if (!DateTime.TryParse(startTime, out startDateTime) ||
                !DateTime.TryParse(endTime, out endDateTime))
            {
                return new ResultModel<MSLotteryBettingResult[]>()
                {
                    Success = false,
                    Error = $"請輸入正確時間格式, gameId={gameId}"
                };
            }

            TimeSpan timeDiff = endDateTime - startDateTime;

            if (timeDiff.TotalHours > _searchTpGameOrdersLimitHourRange)
            {
                return new ResultModel<MSLotteryBettingResult[]>()
                {
                    Success = false,
                    Error = $"搜索時間區間不能超過{_searchTpGameOrdersLimitHourRange}小時, gameId={gameId}"
                };
            }

            PlatformProduct platformProduct = PlatformProduct.Lottery;

            if (!gameId.IsNullOrEmpty())
            {
                MiseOrderGameId miseOrderGameId = MiseOrderGameId.GetSingle(gameId);

                if (miseOrderGameId == null || miseOrderGameId.Product != PlatformProduct.Lottery)
                {
                    return new ResultModel<MSLotteryBettingResult[]>()
                    {
                        Success = false,
                        Error = $"找不到對應的gameId, gameId={gameId}"
                    };
                }
            }

            //彩票查詢
            ResultModel<MSLotteryBettingResult[]> bettingResult = GetLotteryBettingOrders(startTime, endTime, gameId);

            return bettingResult;
        }

        /// <summary>
        /// 取得第三方遊戲訂單
        /// </summary>
        /// <param name="startTime">起始時間</param>
        /// <param name="endTime">結束時間</param>
        /// <param name="gameId">第三方產品代碼</param>
        /// <returns>訂單資訊</returns>
        [HttpGet]
        public ResultModel<MSThirdPartyBettingResult[]> GetTPGameOrders(string startTime, string endTime, string gameId)
        {
            DateTime startDateTime;
            DateTime endDateTime;

            if (!DateTime.TryParse(startTime, out startDateTime) ||
                !DateTime.TryParse(endTime, out endDateTime))
            {
                return new ResultModel<MSThirdPartyBettingResult[]>()
                {
                    Success = false,
                    Error = $"請輸入正確時間格式, gameId={gameId}"
                };
            }

            TimeSpan timeDiff = endDateTime - startDateTime;

            if (timeDiff.TotalHours > _searchTpGameOrdersLimitHourRange)
            {
                return new ResultModel<MSThirdPartyBettingResult[]>()
                {
                    Success = false,
                    Error = $"搜索時間區間不能超過{_searchTpGameOrdersLimitHourRange}小時, gameId={gameId}"
                };
            }

            MiseOrderGameId queryOrderGameId = MiseOrderGameId.GetSingle(gameId);

            if (queryOrderGameId == null || !_platformProductService.Value.GetAll().Any(a => a == queryOrderGameId.Product))
            {
                return new ResultModel<MSThirdPartyBettingResult[]>()
                {
                    Success = false,
                    Error = $"找不到對應的gameId, gameId={gameId}"
                };
            }

            PlatformProduct platformProduct = queryOrderGameId.Product;

            //不走彩票查詢
            if (platformProduct == PlatformProduct.Lottery)
            {
                return new ResultModel<MSThirdPartyBettingResult[]>()
                {
                    Success = false,
                    Error = $"找不到對應的gameId, gameId={gameId}"
                };
            }

            ResultModel<MSThirdPartyBettingResult[]> bettingResult = GetAllGamePlayInfoHistory(startDateTime, endDateTime, queryOrderGameId);

            return bettingResult;
        }

        /// <summary>取得各家第三方餘額</summary>
        [HttpGet]
        public MiseLiveResponse<List<GameBalance>> GetTPGameBalances(int userId)
        {
            BaseReturnDataModel<UserAccountSearchResult> returnDataModel = _tpGameAccountReadService.Value.GetByLocalAccount(userId);

            if (!returnDataModel.IsSuccess)
            {
                return returnDataModel.ToMiseLiveResponse<List<GameBalance>>();
            }

            var allGameBalances = new List<GameBalance>();
            List<TPGameAccountSearchResult> tpGameAccountSearchResults = returnDataModel.DataModel.TPGameAccountSearchResults;

            ConcurrentDictionary<PlatformProduct, HashSet<MiseOrderGameId>> map = MiseOrderGameId.GetProductGameIdMap();

            foreach (TPGameAccountSearchResult tpGameAccountSearchResult in tpGameAccountSearchResults)
            {
                PlatformProduct product = _platformProductService.Value.GetSingle(tpGameAccountSearchResult.TPGameProductCode);

                if (map.TryGetValue(product, out HashSet<MiseOrderGameId> miseOrderGameIds))
                {
                    List<GameBalance> gameBalances = miseOrderGameIds
                        .Select(s =>
                        {
                            var gameBalance = new GameBalance()
                            {
                                TPGameAccount = tpGameAccountSearchResult.TPGameAccount,
                                Balance = tpGameAccountSearchResult.TPGameAvailableScore,
                                FreezeBalance = tpGameAccountSearchResult.TPGameFreezeScore
                            };

                            gameBalance.SetByGameId(s);

                            return gameBalance;
                        })
                        .ToList();

                    allGameBalances.AddRange(gameBalances);
                }
            }

            return new MiseLiveResponse<List<GameBalance>>()
            {
                Success = true,
                Data = allGameBalances
            };
        }

        /// <summary> 彩票投注紀錄 </summary>
        private ResultModel<MSLotteryBettingResult[]> GetLotteryBettingOrders(string startTime, string endTime, string gameId)
        {
            var details = _playInfoService.Value.GetPlayBetsByAnonymous(startTime, endTime, gameId);
            var result = new ResultModel<MSLotteryBettingResult[]>();

            if (details.Length > 0)
            {
                var dicCurrentLotteryInfos = new Dictionary<int, List<Model.CurrentLotteryInfo>>();
                if (string.IsNullOrWhiteSpace(gameId))
                {
                    dicCurrentLotteryInfos = details.GroupBy(x => x.LotteryID.Value).ToDictionary(x => x.Key, y =>
                    {
                        var query = new Model.CurrentLotteryQueryInfo()
                        {
                            Count = 2880,
                            LotteryId = y.Key,
                            Start = y.OrderBy(x => x.NoteTime)
                               .FirstOrDefault().NoteTime,
                            End = y.OrderByDescending(x => x.CurrentLotteryTime)
                               .FirstOrDefault()?.CurrentLotteryTime ?? DateTime.Now,
                        };
                        return _currentLotteryService.Value.QueryCurrentLotteryInfo(query);
                    });
                }
                else
                {
                    var query = new Model.CurrentLotteryQueryInfo()
                    {
                        Count = 2880,
                        LotteryId = Convert.ToInt32(gameId),
                        Start = details.OrderBy(x => x.NoteTime)
                            .FirstOrDefault().NoteTime,
                        End = details.OrderByDescending(x => x.CurrentLotteryTime)
                            .FirstOrDefault()?.CurrentLotteryTime ?? DateTime.Now,
                    };

                    var currentLotteryInfos = _currentLotteryService.Value.QueryCurrentLotteryInfo(query);
                    dicCurrentLotteryInfos.Add(query.LotteryId, currentLotteryInfos);
                }

                result.Data = details.Select(x =>
                {
                    x.PalyNum = x.PalyNum.Replace("L", "龙").Replace("H", "虎");
                    if (x.PlayTypeName.Contains("大小单双"))
                    {
                        x.PalyNum = x.PalyNum.Replace("1", "大").Replace("2", "小").Replace("3", "单").Replace("4", "双");
                    }
                    else if (string.IsNullOrEmpty(x.SourceType) || x.SourceType == "oldweb" || x.SourceType == "flash")
                    {
                        if (x.PlayTypeName.Contains("大小"))
                        {
                            x.PalyNum = x.PalyNum.Replace("1", "大").Replace("0", "小");
                        }
                        if (x.PlayTypeName.Contains("单双"))
                        {
                            x.PalyNum = x.PalyNum.Replace("1", "单").Replace("0", "双");
                        }
                    }
                    if (dicCurrentLotteryInfos.ContainsKey(x.LotteryID.Value))
                    {
                        var currentLotteryInfo = dicCurrentLotteryInfos[x.LotteryID.Value]
                            .FirstOrDefault(issue => issue.IssueNo == x.PalyCurrentNum);
                        if (currentLotteryInfo != null)
                        {
                            x.CurrentLotteryNum = currentLotteryInfo.CurrentLotteryNum;
                        }
                    }
                    return new MSLotteryBettingResult(x);
                }).ToArray();

                result.Success = true;
            }
            else if (details.Length == 0)
            {
                result.Data = new MSLotteryBettingResult[0];
                result.Success = true;
            }

            return result;
        }

        /// <summary> 所有投注紀錄 </summary>
        private ResultModel<MSThirdPartyBettingResult[]> GetAllGamePlayInfoHistory(DateTime startTime, DateTime endTime, MiseOrderGameId miseOrderGameId)
        {
            PlatformProduct platformProduct = miseOrderGameId.Product;

            var param = new TPGamePlayInfoSearchParam
            {
                PageSize = int.MaxValue,
                ProductCode = platformProduct.Value,
                GameType = miseOrderGameId.SubGameCode,
                QueryStartDate = startTime,
                QueryEndDate = endTime
            };

            var platformProducts = new List<PlatformProduct>();

            if (param.ProductCode.IsNullOrEmpty())
            {
                platformProducts.AddRange(_platformProductService.Value.GetAll());
            }
            else
            {
                platformProducts.Add(_platformProductService.Value.GetSingle(param.ProductCode));
            }

            var searchParam = new SearchAllPagedPlayInfoParam()
            {
                PageNo = 1,
                PageSize = param.PageSize,
                GameCode = miseOrderGameId.SubGameCode,
                PlatformProducts = platformProducts,
                QueryStartDate = param.QueryStartDate,
                QueryEndDate = param.QueryEndDate,
            };

            IAllGameStatService allGameStatService = DependencyUtil.ResolveJxBackendService<IAllGameStatService>(EnvLoginUser, DbConnectionTypes.Slave).Value;

            PagedResultWithAdditionalData<AllGamePlayInfoRowModel, TPGamePlayInfoStatModel> pagedResult = allGameStatService
                .GetPagedAllPlayInfo(searchParam);

            ResultModel<MSThirdPartyBettingResult[]> bettingResult = new ResultModel<MSThirdPartyBettingResult[]>
            {
                Success = true,
                Data = pagedResult.ResultList.Select(s =>
                {
                    int miseLiveOrderStatus = BetResultType.GetSingle(s.BetResultType).ToMiseLiveOrderStatus(s.WinMoney);
                    string turnover = "0";

                    if (miseLiveOrderStatus != (int)MiseLiveOrderStatuses.Draw)
                    {
                        turnover = s.WinMoney == 0 ? "0" : s.BetMoneyText;
                    }

                    var msThirdPartyBettingResult = new MSThirdPartyBettingResult
                    {
                        PlayId = string.Empty,
                        SerialNumber = s.PlayID,
                        UserId = s.UserId.ToString(),
                        Nickname = s.UserName,
                        RoomNumber = string.Empty,
                        Amount = s.AllBetMoneyText,
                        CreateTime = s.BetTimeText,
                        SettleTime = s.ProfitLossTimeText,
                        Status = miseLiveOrderStatus.ToString(),
                        ProfitLoss = s.WinMoneyText,
                        GameName = miseOrderGameId.Name,
                        GameDetail = s.Memo,
                        PeriodNumber = string.Empty,
                        GameResult = s.Memo,
                        Turnover = turnover,
                        IsCashOut = s.BetResultType == BetResultType.Cashout,
                    };

                    msThirdPartyBettingResult.SetByGameId(miseOrderGameId);

                    return msThirdPartyBettingResult;
                }).ToArray()
            };

            return bettingResult;
        }
    }
}
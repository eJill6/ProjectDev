using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Infrastructure.Jobs;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.Api;
using ControllerShareLib.Models.Base;
using ControllerShareLib.Models.Enums;
using ControllerShareLib.Models.LongResult;
using ControllerShareLib.Models.LotteryPlan;
using ControllerShareLib.Models.SpaPlayConfig;
using ControllerShareLib.Services;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.ReturnModel;
using JxLottery.Models.Lottery.Bet;
using JxLottery.Services.Adapter;
using M.Core.Controllers.Base;
using M.Core.Interface.Services;
using M.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Model;
using System.Text.Json;

namespace M.Core.Controllers;

public class LotterySpaController : BaseAuthApiController
{
    private readonly Lazy<ILotteryService> _lotteryService;

    private readonly Lazy<ILotterySpaService> _spaService;

    private readonly Lazy<IEnumerable<IBonusAdapter>> _adapters;

    private readonly Lazy<ICache> _localInstance;

    private readonly Lazy<IMenuService> _menuService;

    private readonly Lazy<ILogger<LotterySpaController>> _logger;
    
    public LotterySpaController()
    {
        _lotteryService = ResolveService<ILotteryService>();
        _spaService = ResolveService<ILotterySpaService>();
        _adapters = ResolveService<IEnumerable<IBonusAdapter>>();
        _localInstance = DependencyUtil.ResolveService<ICache>();
        _menuService = DependencyUtil.ResolveService<IMenuService>();
        _logger = ResolveService<ILogger<LotterySpaController>>();
    }

    /// <summary>
    /// 取得返點、賠率資料
    /// </summary>
    /// <returns>結果</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<IEnumerable<RebateProApiResponse>>> GetRebatePro(int lotteryId, int playTypeId,
        int playTypeRadioId, bool? isSingleRebatePro)
    {
        var userId = GetUserId();
        var cacheKey = CacheKeyHelper.ApiRebatePro(userId, lotteryId, playTypeId, playTypeRadioId);
        var now = DateTime.Now;
        var cacheExpired = DateTime.Now.AddMinutes(3);
        if (now.Day != cacheExpired.Day)
        {
            cacheExpired = cacheExpired.Date;
        }
        IEnumerable<RebateProApiResponse> result =
            await _localInstance.Value.GetOrAddAsync(nameof(GetRebatePros), cacheKey, async () =>
            {
                var userInfo = await GetUserInfoWithoutAvailable(userId);
                var rawData = _spaService.Value.GetRebatePro(
                    userInfo,
                    lotteryId,
                    playTypeId,
                    playTypeRadioId,
                    isSingleRebatePro);
                return ConvertToRebateProApiResponse(rawData);
            }, cacheExpired);

        return await Ok(result);
    }

    /// <summary>
    /// 取得賠率資料
    /// </summary>
    /// <returns>結果</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<RebateProsResponse>> GetRebatePros(string? hash)
    {
        var rebatePros = await GetRebatePros();

        var rebateProsHash = GetRebateProsHash(rebatePros);

        if (!string.IsNullOrEmpty(hash) &&
            hash.Equals(rebateProsHash, StringComparison.OrdinalIgnoreCase))
        {
            return await BadRequest<RebateProsResponse>("No Update");
        }

        return await Ok(
            new RebateProsResponse
            {
                Hash = rebateProsHash,
                RebatePros = rebatePros
            });
    }

    private static string GetRebateProsHash(Dictionary<int, IEnumerable<RebateProResponse>> rebatePros)
    {
        return HashExtension.MD5(JsonSerializer.Serialize(rebatePros));
    }

    private async Task<Dictionary<int, IEnumerable<RebateProResponse>>> GetRebatePros()
    {
        var userId = GetUserId();
        var cacheKey = string.Format(CacheKeyHelper.ApiRebatePros, userId);
        var now = DateTime.Now;
        var cacheExpired = DateTime.Now.AddMinutes(3);
        if (now.Day != cacheExpired.Day)
        {
            cacheExpired = cacheExpired.Date;
        }
        var result = await _localInstance.Value.GetOrAddAsync(nameof(GetRebatePros), cacheKey, async () =>
            {
                var userInfo = await GetUserInfoWithoutAvailable(userId);

                var rebateProsOriginal = await _spaService.Value.GetRebatePros(userInfo);

                var rebateProsResponse = new Dictionary<int, IEnumerable<RebateProResponse>>();

                foreach (var rebatePro in rebateProsOriginal)
                {
                    rebateProsResponse.Add(
                        rebatePro.Key,
                        rebatePro.Value.Select(r => new RebateProResponse
                        {
                            LotteryID = r.LotteryId,
                            PlayTypeID = r.PlayTypeId,
                            PlayTypeRadioID = r.PlayTypeRadioId,
                            NumberOdds = ConvertToNumberOddsModel(r.NumberOdds)
                        }));
                }
                return rebateProsResponse;
            }, cacheExpired);
        return result;
    }

    /// <summary>
    /// 取得彩種列表
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<List<LotteryInfo>>> GetAllLotteryInfo()
    {
        List<LotteryInfo> list = _lotteryService.Value.GetAllLotteryInfo();

        return await Ok(list);
    }

    /// <summary>
    /// 取得下期期號
    /// </summary>
    /// <param name="lotteryId">LotteryId</param>
    /// <returns>下期期號</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<CurrentIssueNoViewModel>> GetNextIssueNo(int lotteryId)
    {
        var cacheKey = LotteryService.GetNextIssueNoCacheKey(lotteryId);

        var localObj = await _localInstance.Value.GetAsync<CachedObj<CurrentIssueNoViewModel>>(cacheKey);

        if (localObj != null)
        {
            if (localObj.ExpiredTime >= DateTime.Now)
            {
                return await Ok(localObj.Value);
            }
        }

        return await Ok(localObj?.Value);
    }

    /// <summary>
    /// 下期期號
    /// </summary>
    /// <returns>下期期號</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<CurrentIssueNoViewModel[]>> GetNextIssueNos()
    {
        var cacheKey = LotteryService.GetAllIssueNoCacheKey;

        var localObj = await _localInstance.Value.GetAsync<CachedObj<CurrentIssueNoViewModel[]>>(cacheKey);

        if (localObj != null)
        {
            if (localObj.ExpiredTime >= DateTime.Now)
            {
                return await Ok(localObj.Value);
            }
        }

        return await Ok(localObj?.Value);
    }

    /// <summary>
    /// 排行榜清單(HTML)
    /// </summary>
    /// <param name="days">天數</param>
    /// <returns>排行榜清單</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<List<string>>> GetRankList(int days)
    {
        return await Ok(_spaService.Value.GetRankList(days));
    }

    /// <summary>
    /// 排行榜清單(JSON)
    /// </summary>
    /// <param name="days">天數</param>
    /// <returns>排行榜清單</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<List<WinningListItem>>> GetRankListItems(int days)
    {
        return await Ok(_spaService.Value.GetRankListItems(days));
    }

    /// <summary>
    /// 今日清單
    /// </summary>
    /// <returns>今日清單</returns>
    [HttpGet]
    public async Task<AppResponseModel<object>> GetTodaySummary(int lotteryId)
    {
        return await Ok(_spaService.Value.GetTodaySummary(this.GetUserId(), lotteryId));
    }

    /// <summary>
    /// 下票
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<AppResponseModel<object>> PlaceOrder([FromBody] PlayInfoPostApiModel model)
    {
        try
        {
            int[] specialRuleIds = _spaService.Value.GetSpecialRuleIds();

            if (Array.Exists(specialRuleIds, x => x == model.LotteryID))
            {
                decimal price = model.Price;
                if (price < 2 || price > 200000)
                    return await BadRequest<object>("数字范围为2~200000");
                else if (Math.Ceiling(price) != Math.Floor(price))
                    return await BadRequest<object>("不得有小数点");
            }

            SLPolyGame.Web.Model.UserInfo userInfo = GetUserInfoWithoutAvailable(GetUserId()).ConfigureAwait(false).GetAwaiter().GetResult();
            PalyInfo playInfo = _spaService.Value.PlaceOrder(model, userInfo);

            if (playInfo == null)
            {
                return await BadRequest<object>("彩种已关闭。");
            }

            if (!string.IsNullOrWhiteSpace(playInfo.PalyID) && playInfo.UserName == "ok")
            {
                try
                {
                    await _spaService.Value.AddUnawardedSummaryData(userInfo, model);
                }
                catch (Exception ex)
                {
                    _logger.Value.LogError(ex,$"AddUnawardedSummaryData Error" +
                                              $",userInfo:{JsonSerializer.Serialize(userInfo)}" +
                                              $",model:{JsonSerializer.Serialize(model)}");
                }
                
                var response = new
                {
                    playID = playInfo.PalyID,
                    NoteTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                return await Ok<object>(response);
            }
            else
            {
                return await BadRequest<object>(playInfo.UserName);
            }
        }
        catch (Exception ex)
        {
            return await BadRequest<object>(ex.Message);
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
    [AllowAnonymous]
    public async Task<AppResponseModel<object>> IssueHistory(int lotteryId, int count, string? nextCursor = null)
    {
        return await Ok(_spaService.Value.IssueHistory(lotteryId, count, nextCursor));
    }

    /// <summary>
    /// 取得投注紀錄資料
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <param name="status">全部就不需傳入。
    /// 待開獎：Unawarded、已中獎：Won、未中獎：Lost、和局：SystemCancel、系統撤單：SystemRefund</param>
    /// <param name="searchDate">e.g. 2023-11-10</param>
    /// <param name="cursor"></param>
    /// <param name="pageSize"></param>
    /// <param name="roomId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<AppResponseModel<FollowBetResponse>> GetSpecifyOrderList(int? lotteryId
        , DateTime? searchDate, int pageSize, string? roomId = null, string? status = null
        , string? cursor = null)
    {
        //TODO: 【緊急處理】Articor 確認App 3.0.0版本上線後移除此處理
        if (roomId != null && roomId != "0")
        {
            roomId = null;
        }

        var userId = this.GetUserId();
        var palyInfoList =
            await _spaService.Value.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize, roomId);
        if (palyInfoList == null)
        {
            return await BadRequest<FollowBetResponse>("订单无效");
        }

        foreach (var item in palyInfoList.DataDetail)
        {
            if (string.Equals(item.Status, AwardStatus.SystemCancel.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                item.Status = AwardStatus.Won.ToString();
                item.StatusText = AwardStatus.Won.GetDescription();
            }
        }

        return await Ok(palyInfoList);
    }

    //秘色跟单资讯
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<FollowBetResponse>> GetFollowBet(string palyId, int lotteryId = 0)
    {
        int[] specialRuleIds = _spaService.Value.GetSpecialRuleIds();

        if (!specialRuleIds.Contains(lotteryId))
        {
            lotteryId = MiseOrderGameId.OMKS.Value.ToInt32();
        }

        var palyInfoList = _spaService.Value.GetFollowBet(palyId, lotteryId);
        if (palyInfoList == null)
        {
            return await BadRequest<FollowBetResponse>("订单无效");
        }

        return await Ok(palyInfoList);
    }

    [HttpGet]
    [Obsolete("目前沒找到哪裡使用，等待確認是否有需要，沒使用的話後續安排移除")]
    public async Task<AppResponseModel<string>> GetLotteryCountdownTime(int lotteryId)
    {
        return await Ok(_spaService.Value.GetLotteryCountdownTime(lotteryId));
    }

    /// <summary>
    /// 取得長龍列表(單一彩種)
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LongData>> GetLongData(int lotteryId)
    {
        return await Ok(_spaService.Value.GetLongData(lotteryId));
    }

    /// <summary>
    /// 取得長龍列表(所有彩種)
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LongData[]>> GetLongDatas()
    {
        return await Ok(_spaService.Value.GetLongDatas());
    }

    /// <summary>
    /// 取得跟單計畫列表(單一彩種)
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <param name="planType"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LotteryPlanData>> GetLotteryPlanData(int lotteryId, int planType)
    {
        return await Ok(_spaService.Value.GetLotteryPlanData(lotteryId, planType));
    }

    /// <summary>
    /// 取得跟單計畫列表(所有彩種)
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LotteryPlanData[]>> GetLotteryPlanDatas()
    {
        return await Ok(_spaService.Value.GetLotteryPlanData());
    }

    /// <summary>
    /// 取得長龍列表(單一彩種)
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LongData>> GetLongDataV2(int lotteryId)
    {
        var cacheKey = LotterySpaService.LongResultCacheName(lotteryId);

        var localObj = await _localInstance.Value.GetAsync<CachedObj<LongData>>(cacheKey);

        if (localObj != null)
        {
            if (localObj.ExpiredTime >= DateTime.Now)
            {
                return await Ok(localObj.Value);
            }
        }

        return await Ok(new LongData());
    }

    /// <summary>
    /// 取得長龍列表(所有彩種)
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LongData[]>> GetLongDatasV2()
    {
        var cacheKey = LotterySpaService.LongResultAllInOne;

        var localObj = await _localInstance.Value.GetAsync<CachedObj<LongData[]>>(cacheKey);

        if (localObj != null)
        {
            if (localObj.ExpiredTime >= DateTime.Now)
            {
                return await Ok(localObj.Value);
            }
        }

        return await Ok(new LongData[0]);
    }

    /// <summary>
    /// 取得跟單計畫列表(單一彩種)
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <param name="planType"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LotteryPlanData>> GetLotteryPlanDataV2(int lotteryId, int planType)
    {
        var cacheKey = LotterySpaService.LotteryPlanCacheName(lotteryId, planType);

        var localObj = await _localInstance.Value.GetAsync<CachedObj<LotteryPlanData>>(cacheKey);

        if (localObj != null)
        {
            if (localObj.ExpiredTime >= DateTime.Now)
            {
                return await Ok(localObj.Value);
            }
        }

        return await Ok(new LotteryPlanData());
    }

    /// <summary>
    /// 取得跟單計畫列表(所有彩種)
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<LotteryPlanData[]>> GetLotteryPlanDatasV2()
    {
        var cacheKey = LotterySpaService.LotteryPlanAllInOne;

        var localObj = await _localInstance.Value.GetAsync<CachedObj<LotteryPlanData[]>>(cacheKey);

        if (localObj != null)
        {
            if (localObj.ExpiredTime >= DateTime.Now)
            {
                return await Ok(localObj.Value);
            }
        }

        return await Ok(new LotteryPlanData[0]);
    }

    /// <summary>
    /// 取得彩種玩法資訊
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<AppResponseModel<object>> GetPlayTypes(int lotteryId)
    {
        var adapter = this._adapters.Value.FirstOrDefault(x => x.LotteryId == lotteryId);

        if (adapter == null)
        {
            return await BadRequest<object>("参数无效");
        }

        var playConfig = await _spaService.Value.GetPlayConfig(adapter);

        return await Ok<object>(ConvertToPlayTypesApiResponse(playConfig));
    }

    /// <summary>
    /// 取得彩種選單
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<AppResponseModel<IEnumerable<LotteryInfoResponse>>> GetLotteryMenus()
    {
        string cacheKey = CacheKeyHelper.LotteryMenus;

        return await Ok(
            CacheService.GetLocalCache(
                cacheKey,
                () => _menuService.Value.GetLotteryMenus(),
                (data) => DateTime.Now.AddMinutes(3))
            );
    }
    
    /// <summary>
    /// 取得該彩種當期期號未開獎注單摘要
    /// </summary>
    /// <param name="lotteryId"></param>
    /// <param name="currentIssueNo"></param>
    /// <param name="roomId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<AppResponseModel<List<UnawardedSummaryApiResponse>>> GetUnawardedSummary(
        int lotteryId, string currentIssueNo, string? roomId = null)
    {
        var userInfo = GetUserInfo();

        return await Ok(await _spaService.Value.GetUnawardedSummary(userInfo, lotteryId, roomId, currentIssueNo));
    }

    private static IEnumerable<RebateProApiResponse> ConvertToRebateProApiResponse(
        List<RebateSelectItem> rebateSelectItems)
    {
        foreach (var rebateSelectItem in rebateSelectItems)
        {
            yield return new RebateProApiResponse(ConvertToNumberOddsModel(rebateSelectItem.NumberOdds));
        }
    }

    private static List<NumberOddsModel> ConvertToNumberOddsModel(Dictionary<string, Decimal> numberOdds)
    {
        var numberOddsConverted = new List<NumberOddsModel>();

        foreach (var numberOdd in numberOdds)
        {
            var categoryAndValue = numberOdd.Key.Split(' ');
            var category = categoryAndValue[0];
            var value = categoryAndValue.Length > 1 ? categoryAndValue[1] : "";

            var existingCategory = numberOddsConverted.Find(c => c.Category == category);

            if (existingCategory == null)
            {
                existingCategory = new NumberOddsModel { Category = category, Values = new List<OddsModel>() };
                numberOddsConverted.Add(existingCategory);
            }

            existingCategory.Values.Add(new OddsModel { Value = value, Odds = numberOdd.Value.ToString() });
        }

        return numberOddsConverted;
    }

    private IEnumerable<object> ConvertToPlayTypesApiResponse(IEnumerable<PlayMode<PlayTypeInfoApiResult>> playConfig)
    {
        var userInfo = GetUserInfoWithoutAvailable(GetUserId()).ConfigureAwait(false).GetAwaiter().GetResult();

        foreach (PlayMode<PlayTypeInfoApiResult> playMode in playConfig)
        {
            foreach (var playTypeInfo in playMode.PlayTypeInfos)
            {
                foreach (var playTypeRadioInfo in playTypeInfo.PlayTypeRadioInfos)
                {
                    foreach (var value in playTypeRadioInfo.Value)
                    {
                        var rebateSelectItems = _spaService.Value.GetRebatePro(userInfo, playTypeInfo.Info.LotteryID.Value,
                            playTypeInfo.Info.PlayTypeID, value.Info.PlayTypeRadioID, isSingleRebatePro: true);

                        yield return new
                        {
                            playTypeInfo.Info.LotteryID,
                            playTypeInfo.Info.PlayTypeID,
                            value.Info.PlayTypeRadioID,
                            NumberOdds = ConvertToNumberOddsModel(rebateSelectItems.FirstOrDefault()?.NumberOdds)
                        };
                    }
                }
            }
        }
    }

    private async Task<AppResponseModel<T>> Ok<T>(T data) where T : class
    {
        return await Task.FromResult(new AppResponseModel<T>()
        {
            Success = true,
            Data = data
        });
    }

    private async Task<AppResponseModel<T>> BadRequest<T>(string errorMessage) where T : class
    {
        return await Task.FromResult(new AppResponseModel<T>()
        {
            Success = false,
            Message = errorMessage
        });
    }

    #region Old View Part 先留著

    // /// <summary>
    // /// 取得彩種資訊
    // /// </summary>
    // /// <param name="id">彩種編號</param>
    // /// <param name="orderNo">跟單編號</param>
    // /// <returns>彩種資訊</returns>
    // private object GetBetViewModel(string id, string orderNo = null, bool isFullscreen = false)
    // {
    //     var lottery = _spaService.GetLotteryInfoMethod(id);
    //
    //     if (lottery == null)
    //     {
    //         return null;
    //     }
    //     var adapter = this._adapters.FirstOrDefault(x => x.LotteryId == lottery.LotteryID);
    //
    //     if (adapter == null)
    //     {
    //         return null;
    //     }
    //
    //     var lotteryInfo = new
    //     {
    //         adapter.GameTypeId,
    //         GameTypeName = ((JxLottery.Models.Lottery.GameTypeId)adapter.GameTypeId).ToString(),
    //         LotteryId = lottery.LotteryID,
    //         LotteryTypeName = lottery.LotteryType,
    //         OfficialUrl = lottery.OfficialLotteryUrl,
    //         TrendUrl = Url.Action(nameof(Trend), new { id }),
    //         lottery.MaxBonusMoney,
    //         LotteryCode = lottery.TypeURL,
    //         LogoUrl = WebResourceHelper.Content($"~/2015/images/0{lottery.LotteryID}.png"),
    //         MaxAfterPeriods = After.GetChaseNumberCount(lottery.LotteryID, int.MaxValue)
    //     };
    //
    //     var playConfigs = _spaService.GetPlayConfig(adapter, GetUserId());
    //
    //     var sysSetting = this.UserService.GetSysSettings();
    //
    //     var settings = sysSetting == null ? new object() : new
    //     {
    //         sysSetting.MaxBetCount,
    //     };
    //
    //     if (bool.Parse(ConfigUtilService.Get("UseCDN")))
    //     {
    //         string cdnUrl = ConfigUtilService.Get("CDNSite");
    //
    //         if (cdnUrl != null)
    //         {
    //             if (cdnUrl[cdnUrl.Length - 1] != '/')
    //             {
    //                 cdnUrl = cdnUrl + "/";
    //             }
    //         }
    //
    //         var fullcdnUrl = cdnUrl + "CTS/ClientApp/dist/yinsiang-lottery/img/";
    //         if (isFullscreen)
    //         {
    //             fullcdnUrl = cdnUrl + "CTS/ClientApp/dist/yinsiang-lottery-fullscreen/img/";
    //         }
    //         settings = sysSetting == null ? new object() : new
    //         {
    //             sysSetting.MaxBetCount,
    //             cdnUrl = fullcdnUrl
    //         };
    //     }
    //
    //     TicketUserData userInfo = AuthenticationUtil.GetLoginUserFromCache();
    //
    //     var msSetting = new
    //     {
    //         DepositUrl = userInfo.DepositUrl,
    //         RoomId = userInfo.RoomNo,
    //         OrderNo = orderNo,
    //         LogonMode = userInfo.LogonMode,
    //     };
    //
    //     return new { lotteryInfo, playConfigs, settings, msSetting };
    // }
    //
    // /// <summary>
    // /// 走勢圖
    // /// </summary>
    // /// <param name="id">LotteryCode(路由名稱叫Id)</param>
    // /// <returns>走勢圖</returns>
    // public async Task<IActionResult> Trend(string id, int? sort = null, int searchType = 1, string tab = "comprehensive")
    // {
    //     var lottery = _lotteryService.GetLotteryType()
    //         .FirstOrDefault(x => string.Equals(x.TypeURL, id, StringComparison.InvariantCultureIgnoreCase));
    //
    //     if (lottery == null)
    //     {
    //         return await Task.FromResult(NotFound());
    //     }
    //
    //     var gameTypeId = (JxLottery.Models.Lottery.GameTypeId)_adapters.FirstOrDefault(x => x.LotteryId == lottery.LotteryID)?.GameTypeId;
    //
    //     var model = GetTrend(lottery, sort, searchType, tab);
    //     return await Task.FromResult(View($"{gameTypeId}Trend", model));
    // }
    // /// <summary>
    // /// 取得該期額外資訊
    // /// </summary>
    // /// <param name="id">彩種代碼</param>
    // /// <param name="issueNo">期號</param>
    // /// <returns>該期的額外資訊</returns>
    // [HttpGet]
    // public async Task<IActionResult> GetExtIssueData(string id, string issueNo)
    // {
    //     var model = _spaService.GetExtIssueData(id, issueNo, out var resultMsg);
    //     if (model == null)
    //     {
    //         return await BadRequest(resultMsg);
    //     }
    //
    //     return await Ok(model);
    // }
    //
    // /// <summary>
    // /// 取得彩種設定
    // /// </summary>
    // /// <param name="id">彩種編號</param>
    // /// <returns>彩種設定</returns>
    // [HttpGet]
    // public async Task<IActionResult> GetViewModel(string id)
    // {
    //     var model = GetBetViewModel(id);
    //     return await Ok(model);
    // }
    //
    // /// <summary>
    // /// Get lottery order details.
    // /// </summary>
    // /// <param name="orderId">Lottery order id.</param>
    // /// <returns>Lottery order details</returns>
    //
    // public async Task<IActionResult> OrderDetails(string orderId)
    // {
    //     var details = _spaService.OrderDetails(orderId, out var userId);
    //     if (details == null || userId != GetUserId())
    //     {
    //         return await BadRequest("订单号无效");
    //     }
    //
    //     return await Ok(details);
    // }
    //
    // private IEnumerable<dynamic> GetTrend(LotteryInfo info, int? sort, int searchType, string tab)
    // {
    //     var adapter = _adapters.FirstOrDefault(x => x.LotteryId == info.LotteryID);
    //     var helper = _helpers.FirstOrDefault(x => x.GameTypeId == adapter.GameTypeId);
    //     var queryInfo = helper.GetQueryInfo(searchType);
    //     queryInfo.LotteryId = info.LotteryID;
    //     var list = _lotteryService.QueryCurrentLotteryInfo(queryInfo);
    //     List<CurrentLotteryInfo> sortList = null;
    //
    //     if (sort == 1)
    //     {
    //         sortList = list.OrderByDescending(x => x.IssueNo).ToList();
    //     }
    //     else
    //     {
    //         sortList = list.OrderBy(x => x.IssueNo).ToList();
    //     }
    //     ViewBag.LotteryTypeName = info.LotteryType;
    //     ViewBag.LotteryTypeUrl = info.TypeURL;
    //     var controller = RouteData.Values["controller"].ToString();
    //     var action = RouteData.Values["action"].ToString();
    //     ViewBag.Controller = controller;
    //     ViewBag.Action = action;
    //     ViewBag.SearchType = searchType;
    //     ViewBag.Sort = sort;
    //     ViewBag.TotalCount = sortList.Count;
    //     ViewBag.lotteryList = _lotteryService.GetAllLotteryInfo();
    //     ViewBag.Tab = tab;
    //     return helper.PrepareTrend(sortList);
    // }

    #endregion Old View Part 先留著
}
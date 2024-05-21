using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.BLL;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.MSSeal.Models;
using SLPolyGame.Web.MSSeal.Models.OrderResponse;
using System.Runtime.Caching;

namespace SLPolyGame.Web.Core.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly Lazy<PalyInfo> _playInfoService;

        private readonly Lazy<CurrentLotteryInfo> _currentLotteryService;

        private readonly Lazy<ILogger<OrderController>> _logger;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private static readonly MemoryCache _cache = MemoryCache.Default;

        //65一分快三, 66一分快車, 67一分時時彩, 68一分六合彩, 69百人牛牛, 70百家樂, 71輪盤, 72魚蝦蟹, 73三公, 74龍虎
        protected readonly int[] specialRuleIds = { 65, 66, 67, 68, 69, 70, 71, 72, 73, 74 };

        public OrderController()
        {
            _playInfoService = DependencyUtil.ResolveService<PalyInfo>();
            _currentLotteryService = DependencyUtil.ResolveService<CurrentLotteryInfo>();
            _logger = DependencyUtil.ResolveService<ILogger<OrderController>>();
            _platformProductService = DependencyUtil.ResolveService<IPlatformProductService>();
        }

        [HttpGet]
        public async Task<ResultModel<NearOpenModel>> GetNearOpen(int gameId)
        {
            var result = new ResultModel<NearOpenModel>();
            try
            {
                var cacheKey = string.Format("Web#GetLotteryInfos#{0}", gameId);
                if (specialRuleIds.Contains(gameId))
                {
                    var rawResult = _cache.Get(cacheKey) as Model.CurrentLotteryInfo;
                    if (rawResult == null)
                    {
                        rawResult = _currentLotteryService.Value.GetLotteryInfos(gameId);
                        _cache.Set(cacheKey, rawResult, DateTime.Now.AddSeconds(1));
                    }
                    if (rawResult != null)
                    {
                        result.Data = new NearOpenModel()
                        {
                            GameId = gameId,
                            EndTime = rawResult.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            PeriodNumber = rawResult.LotteryNo
                        };
                        result.Success = true;
                    }
                }
                else
                {
                    result.Error = "GameId not exist";
                }
            }
            catch (Exception ex)
            {
                _logger.Value.LogError(ex, $"GetNearOpen fail");
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// gameId與訂單類型關聯
        /// </summary>
        [HttpGet]
        public async Task<ResultModel<GameIdListResponse>> GetGameIdList()
        {
            HashSet<PlatformProduct> productSet = _platformProductService.Value.GetAll().Where(w => w.Value != PlatformProduct.Lottery).ToHashSet();
            List<MiseOrderGameId> miseOrderGameIds = MiseOrderGameId.GetAll().Where(w => productSet.Contains(w.Product)).ToList();
            List<MiseOrderSubType> miseOrderSubTypes = miseOrderGameIds.Select(s => s.OrderSubType).Distinct().ToList();
            List<MiseOrderType> miseOrderTypes = miseOrderSubTypes.Select(s => s.OrderType).Distinct().ToList();

            List<OrderTypeInfo> orderTypes = miseOrderTypes.Select(s => new OrderTypeInfo()
            {
                Type = s.Value,
                TypeName = s.Name
            }).ToList();

            List<OrderSubTypeInfo> orderSubTypeInfos = miseOrderSubTypes.Select(s => new OrderSubTypeInfo()
            {
                SubType = s.Value,
                SubTypeName = s.Name,
                Type = s.OrderType.Value,
            }).ToList();

            List<GameIdInfo> gameIdInfos = miseOrderGameIds.Select(s => new GameIdInfo()
            {
                GameId = s.Value,
                GameName = s.Name,
                SubType = s.OrderSubType.Value,
            }).ToList();

            return await Task.FromResult(new ResultModel<GameIdListResponse>
            {
                Success = true,
                Data = new GameIdListResponse
                {
                    OrderTypeInfos = orderTypes,
                    OrderSubTypeInfos = orderSubTypeInfos,
                    GameIdInfos = gameIdInfos,
                }
            });
        }
    }
}
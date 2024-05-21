using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using Microsoft.Extensions.Logging;
using SLPolyGame.Web.BLL;
using SLPolyGame.Web.Controllers.Base;
using SLPolyGame.Web.MSSeal.Models;
using SLPolyGame.Web.MSSeal.Models.OrderResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;

namespace SLPolyGame.Web.Controllers
{
    public class OrderController : BaseApiController
    {
        protected readonly PalyInfo _playInfoService = null;

        protected readonly CurrentLotteryInfo _currentLotteryService = null;

        protected readonly ILogger<OrderController> _logger = null;

        private readonly IPlatformProductService _platformProductService;

        private static readonly MemoryCache _cache = MemoryCache.Default;
        //65一分快三, 66一分快車, 67一分時時彩, 68一分六合彩
        protected readonly int[] specialRuleIds = { 65, 66, 67, 68 };

        public OrderController(PalyInfo playInfoService,
            CurrentLotteryInfo currentLotteryService,
            ILogger<OrderController> logger)
        {
            _playInfoService = playInfoService;
            _currentLotteryService = currentLotteryService;
            _logger = logger;
            _platformProductService = DependencyUtil.ResolveServiceForModel<IPlatformProductService>(JxApplication.FrontSideWeb);
        }

        public ResultModel<NearOpenModel> GetNearOpen(int gameId)
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
                        rawResult = _currentLotteryService.GetLotteryInfos(gameId);
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
                _logger.LogError(ex, $"GetNearOpen fail");
            }
            return result;
        }

        /// <summary>
        /// gameId與訂單類型關聯
        /// </summary>
        [HttpGet]
        public ResultModel<GameIdListResponse> GetGameIdList()
        {
            HashSet<PlatformProduct> productSet = _platformProductService.GetAll().Where(w => w.Value != PlatformProduct.Lottery).ToHashSet();
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

            return new ResultModel<GameIdListResponse>
            {
                Success = true,
                Data = new GameIdListResponse
                {
                    OrderTypeInfos = orderTypes,
                    OrderSubTypeInfos = orderSubTypeInfos,
                    GameIdInfos = gameIdInfos,
                }
            };
        }
    }
}
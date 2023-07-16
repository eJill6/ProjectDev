using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class LotteryInfoService : BaseService, ILotteryInfoService
    {
        private readonly ILotteryInfoRep _lotteryInfoRep;

        private readonly IPlayTypeInfoRep _playTypeInfoRep;

        private readonly IPlayTypeRadioRep _playTypeRadioRep;

        private readonly IPalyInfoRep _palyInfoRep;

        private readonly IJxCacheService _jxCacheService;

        public LotteryInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _lotteryInfoRep = ResolveJxBackendService<ILotteryInfoRep>();
            _playTypeInfoRep = ResolveJxBackendService<IPlayTypeInfoRep>();
            _playTypeRadioRep = ResolveJxBackendService<IPlayTypeRadioRep>();
            _palyInfoRep = ResolveJxBackendService<IPalyInfoRep>();
            _jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
        }

        public virtual List<int> GetExcludingLotteryIDs()
        {
            //這邊回傳int是為了後續linq計算方便
            return new List<int>() { (int)JXLotteryIDs.OMKS };
        }

        public Dictionary<int, string> GetLotteryTypeMap()
        {
            return _jxCacheService.GetCache(
                CacheKey.LotteryTypeMap,
                () =>
                {
                    return _lotteryInfoRep.GetAll().ToDictionary(d => d.LotteryID, d => d.LotteryType);
                });
        }

        public Dictionary<int, string> GetPlayTypeNameMap()
        {
            return _jxCacheService.GetCache(
                CacheKey.PlayTypeNameMap,
                () =>
                {
                    return _playTypeInfoRep.GetAll().ToDictionary(d => d.PlayTypeID, d => d.PlayTypeName);
                });
        }

        public Dictionary<int, string> GetPlayTypeRadioNameMap()
        {
            return _jxCacheService.GetCache(
                CacheKey.PlayTypeRadioNameMap,
                () =>
                {
                    return _playTypeRadioRep.GetAll().ToDictionary(d => d.PlayTypeRadioID, d => d.PlayTypeRadioName);
                });
        }

        public string GetCurrentLotteryNumMapKey(int lotteryID, string issueNo, int? userID)
        {
            if (IsSecHistoryLotteryID(lotteryID))
            {
                return $"{lotteryID}_{issueNo}_{userID}";
            }

            return $"{lotteryID}_{issueNo}";
        }

        private bool IsSecHistoryLotteryID(int lotteryID) => lotteryID == (int)JXLotteryIDs.HSMMCRealtime || lotteryID == (int)JXLotteryIDs.HSMMCPK10;
    }
}
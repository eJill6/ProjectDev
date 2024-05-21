using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class LotteryInfoService : BaseService, ILotteryInfoService
    {
        private readonly Lazy<ILotteryInfoRep> _lotteryInfoRep;

        private readonly Lazy<IPlayTypeInfoRep> _playTypeInfoRep;

        private readonly Lazy<IPlayTypeRadioRep> _playTypeRadioRep;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public LotteryInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _lotteryInfoRep = ResolveJxBackendService<ILotteryInfoRep>();
            _playTypeInfoRep = ResolveJxBackendService<IPlayTypeInfoRep>();
            _playTypeRadioRep = ResolveJxBackendService<IPlayTypeRadioRep>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        public Dictionary<int, string> GetLotteryTypeMap()
        {
            return _jxCacheService.Value.GetCache(
                CacheKey.LotteryTypeMap,
                () =>
                {
                    return _lotteryInfoRep.Value.GetAll().ToDictionary(d => d.LotteryID, d => d.LotteryType);
                });
        }

        public Dictionary<int, string> GetPlayTypeNameMap()
        {
            return _jxCacheService.Value.GetCache(
                CacheKey.PlayTypeNameMap,
                () =>
                {
                    return _playTypeInfoRep.Value.GetAll().ToDictionary(d => d.PlayTypeID, d => d.PlayTypeName);
                });
        }

        public Dictionary<int, string> GetPlayTypeRadioNameMap()
        {
            return _jxCacheService.Value.GetCache(
                CacheKey.PlayTypeRadioNameMap,
                () =>
                {
                    return _playTypeRadioRep.Value.GetAll().ToDictionary(d => d.PlayTypeRadioID, d => d.PlayTypeRadioName);
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
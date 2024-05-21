using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Web;
using Web.Helpers;

namespace Web.Services
{
    public class LotteryService : ILotteryService
    {
        private readonly ISLPolyGameWebSVService _slPolyGameWebSVService = null;

        private readonly ISerTabWebSVService _serTabWebSVService = null;

        private readonly ICacheService _cacheService = null;

        public LotteryService(ISLPolyGameWebSVService slPolyGameWebSVService,
            ISerTabWebSVService serTabWebSVService,
            ICacheService cacheService)
        {
            _slPolyGameWebSVService = slPolyGameWebSVService;
            _serTabWebSVService = serTabWebSVService;
            _cacheService = cacheService;
        }

        public List<LotteryInfo> GetAllLotteryInfo()
        {
            var cookie = HttpContext.Current.Request.Cookies["SubMerchantType"];
            var cacheKey = $"{cookie?.Value ?? string.Empty}#{CacheKeyHelper.LotteryTypeKey}";
            List<LotteryInfo> lotteries = _cacheService.Get<List<LotteryInfo>>(cacheKey);

            if (lotteries == null)
            {
                lotteries = _serTabWebSVService.GetLotteryType();
                _cacheService.Set(cacheKey, lotteries, DateTime.Now.AddMinutes(10));
            }

            return lotteries;
        }

        public CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
        {
            return _slPolyGameWebSVService.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor);
        }

        public CurrentLotteryInfo GetLotteryInfos(int lotteryID, int userID)
        {
            CurrentLotteryInfo result;
            string cacheKey = string.Format("Web#GetLotteryInfos#{0}#{1}", lotteryID, userID);

            // 計算時間
            var cacheKeyGetLotteryInfosDate = string.Format("Web#GetLotteryInfosDate#{0}#{1}", lotteryID, userID);
            string cacheObjGetLotteryInfos = _cacheService.Get<string>(cacheKeyGetLotteryInfosDate);

            if (cacheObjGetLotteryInfos == null)
            {
                DateTime now = DateTime.Now;
                _cacheService.Set(cacheKeyGetLotteryInfosDate, now.ToString("yyyy/MM/dd HH:mm:ss"), DateTime.Now.AddSeconds(5));
            }
            else
            {
                DateTime cacheDate = DateTime.Now;
                string sCacheDate = cacheObjGetLotteryInfos.ToString();
                DateTime.TryParse(sCacheDate, out cacheDate);
                // 清Cache
                if (Math.Abs(new TimeSpan(DateTime.Now.Ticks - cacheDate.Ticks).TotalSeconds) >= 5)
                {
                    _cacheService.Del(cacheKeyGetLotteryInfosDate);
                    _cacheService.Del(cacheKey);
                }
            }

            result = _cacheService.Get<CurrentLotteryInfo>(cacheKey);

            // 取得彩種
            if (result == null)
            {
                result = _serTabWebSVService.GetLotteryInfos(lotteryID);
                _cacheService.Set(cacheKey, result, DateTime.Now.AddSeconds(5));
            }

            if (result != null)
            {
                result.CurrentTime = DateTime.Now;

                if (result.CurrentTime > result.EndTime)
                {
                    result = _serTabWebSVService.GetLotteryInfos(lotteryID);
                    _cacheService.Set(cacheKey, result, DateTime.Now.AddSeconds(5));
                }
            }

            return result;
        }

        public List<LotteryInfo> GetLotteryType()
        {
            var lotteries = _cacheService.Get<List<LotteryInfo>>(CacheKeyHelper.LotteryTypeKey);

            if (lotteries == null)
            {
                lotteries = _serTabWebSVService.GetLotteryType();
                _cacheService.Set(CacheKeyHelper.LotteryTypeKey, lotteries, DateTime.Now.AddMinutes(10));
            }

            return lotteries;
        }

        public List<PlayTypeInfo> GetPlayTypeInfo()
        {
            var playTypes = _cacheService.Get<List<PlayTypeInfo>>(CacheKeyHelper.PlayTypeInfoKey);

            if (playTypes == null)
            {
                playTypes = _serTabWebSVService.GetPlayTypeInfo();
                _cacheService.Set(CacheKeyHelper.PlayTypeInfoKey, playTypes, DateTime.Now.AddMinutes(10));
            }

            return playTypes;
        }

        public List<PlayTypeRadio> GetPlayTypeRadio()
        {
            var playTypeRadios = _cacheService.Get<List<PlayTypeRadio>>(CacheKeyHelper.PlayTypeRadioKey);

            if (playTypeRadios == null)
            {
                playTypeRadios = _serTabWebSVService.GetPlayTypeRadio();
                _cacheService.Set(CacheKeyHelper.PlayTypeRadioKey, playTypeRadios, DateTime.Now.AddMinutes(10));
            }

            return playTypeRadios;
        }

        public TodaySummaryInfo GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count)
        {
            return _serTabWebSVService.GetTodaySummaryInfo(start, end, lotteryID, count);
        }

        /// <summary>
        /// GetTodaySummaryInfo
        /// </summary>
        /// <param name="lotteryID"></param>
        /// <param name="count"></param>
        /// <param name="LoadType">true，从主库读取，false，从备库读取</param>
        /// <returns></returns>
        public TodaySummaryInfo GetTodaySummaryInfo(int lotteryID, int count, bool? LoadType)
        {
            bool isansy = true;

            if (LoadType == true)
            {
                isansy = false;
            }

            return _serTabWebSVService.GetPlayInfoSummaryInfo(lotteryID, count, isansy);
        }

        public List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            return _serTabWebSVService.QueryCurrentLotteryInfo(query);
        }
    }
}
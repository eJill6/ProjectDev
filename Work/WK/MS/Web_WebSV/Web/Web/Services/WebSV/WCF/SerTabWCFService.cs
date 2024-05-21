using JxBackendService.DependencyInjection;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;

namespace Web.Services.WebSV.WCF
{
    public class SerTabWCFService : ISerTabWebSVService
    {
        private readonly SerTabService.ISerTabService _serTabService;

        public SerTabWCFService()
        {
            _serTabService = DependencyUtil.ResolveService<SerTabService.ISerTabService>();
        }

        public CurrentLotteryInfo GetLotteryInfos(int lotteryid)
            => _serTabService.GetLotteryInfos(lotteryid);

        public List<LotteryInfo> GetLotteryType()
            => _serTabService.GetLotteryType();

        public TodaySummaryInfo GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy)
            => _serTabService.GetPlayInfoSummaryInfo(lotteryID, count, isansy);

        public List<PlayTypeInfo> GetPlayTypeInfo()
            => _serTabService.GetPlayTypeInfo();

        public List<PlayTypeRadio> GetPlayTypeRadio()
            => _serTabService.GetPlayTypeRadio();

        public TodaySummaryInfo GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count)
            => _serTabService.GetTodaySummaryInfo(start, end, lotteryID, count);

        public List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
            => _serTabService.QueryCurrentLotteryInfo(query);
    }
}
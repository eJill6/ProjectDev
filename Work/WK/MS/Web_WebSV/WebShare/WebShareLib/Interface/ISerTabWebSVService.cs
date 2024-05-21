using JxBackendService.Model.Entity;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Interface
{
    public interface ISerTabWebSVService
    {
        Task<List<PlayTypeRadio>> GetPlayTypeRadio();

        Task<List<PlayTypeInfo>> GetPlayTypeInfo();

        Task<List<LotteryInfo>> GetLotteryType();

        Task<CurrentLotteryInfo> GetLotteryInfos(int lotteryid);
        Task<CurrentLotteryInfo[]> GetNextIssueNos(string lotteryIds);

        Task<List<CurrentLotteryInfo>> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query);

        Task<TodaySummaryInfo> GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count);

        Task<TodaySummaryInfo> GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy);

        Task<IEnumerable<LiveGameManage>> GetLiveGameManageInfos();
    }
}
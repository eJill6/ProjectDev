using ControllerShareLib.Infrastructure.Jobs;
using JxBackendService.Model.Entity;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Interfaces.Service
{
    public interface ILotteryService
    {
        Task<List<PlayTypeInfo>> GetPlayTypeInfo();

        Task<List<PlayTypeRadio>> GetPlayTypeRadio();

        List<LotteryInfo> GetAllLotteryInfo();

        Task<CurrentLotteryInfo> GetRawLotteryInfos(int lotteryId);
        Task<CurrentIssueNoViewModel> GetLotteryInfos(int lotteryId);
        Task<CurrentIssueNoViewModel[]> GetNextIssueNos(int[] lotteryIds);

        List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query);

        CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor);

        TodaySummaryInfo GetTodaySummaryInfo(int lotteryID, int count, bool? LoadType);

        bool IsFrontsideMenuActive(int lotteryID);

        IEnumerable<LiveGameManage> GetLiveGameManageInfos();
    }
}
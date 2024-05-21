using JxBackendService.DependencyInjection;
using JxBackendService.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.Core.Controllers.Web
{
    public class SerTabServiceController : BaseAuthApiController, ISerTabWebSVService
    {
        private readonly Lazy<ISerTabWebSVService> _serTabService;

        public SerTabServiceController()
        {
            _serTabService = DependencyUtil.ResolveService<ISerTabWebSVService>();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<CurrentLotteryInfo> GetLotteryInfos(int lotteryid)
            => await _serTabService.Value.GetLotteryInfos(lotteryid);

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<LotteryInfo>> GetLotteryType()
            => await _serTabService.Value.GetLotteryType();

        [HttpGet]
        public async Task<TodaySummaryInfo> GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy)
            => await _serTabService.Value.GetPlayInfoSummaryInfo(lotteryID, count, isansy);

        [HttpGet, AllowAnonymous]
        public async Task<List<PlayTypeInfo>> GetPlayTypeInfo()
            => await _serTabService.Value.GetPlayTypeInfo();

        [HttpGet, AllowAnonymous]
        public async Task<List<PlayTypeRadio>> GetPlayTypeRadio()
            => await _serTabService.Value.GetPlayTypeRadio();

        [HttpGet]
        public async Task<TodaySummaryInfo> GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count)
            => await _serTabService.Value.GetTodaySummaryInfo(start, end, lotteryID, count);

        [HttpPost]
        public async Task<List<CurrentLotteryInfo>> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
            => await _serTabService.Value.QueryCurrentLotteryInfo(query);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<LiveGameManage>> GetLiveGameManageInfos()
            => await _serTabService.Value.GetLiveGameManageInfos();

        [AllowAnonymous]
        [HttpGet]
        public async Task<CurrentLotteryInfo[]> GetNextIssueNos(string lotteryIds)
        => await _serTabService.Value.GetNextIssueNos(lotteryIds);
    }
}
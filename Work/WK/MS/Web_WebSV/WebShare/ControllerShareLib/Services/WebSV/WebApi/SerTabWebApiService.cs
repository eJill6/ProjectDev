using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Common.Util;
using JxBackendService.Model.Entity;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System.Web;

namespace ControllerShareLib.Services.WebSV.WebApi
{
    public class SerTabWebApiService : BaseWebSVService, ISerTabWebSVService
    {
        protected override string RemoteControllerName => "SerTabService";

        public async Task<CurrentLotteryInfo> GetLotteryInfos(int lotteryid)
        {
            return await Task.FromResult(GetHttpGetResponse<CurrentLotteryInfo>(nameof(GetLotteryInfos), $"{nameof(lotteryid)}={HttpUtility.UrlEncode(lotteryid.ToString())}"));
        }

        public async Task<CurrentLotteryInfo[]> GetNextIssueNos(string lotteryids)
        {
            return await Task.FromResult(GetHttpGetResponse<CurrentLotteryInfo[]>(nameof(GetNextIssueNos), $"{nameof(lotteryids)}={HttpUtility.UrlEncode(lotteryids)}"));
        }

        public async Task<List<LotteryInfo>> GetLotteryType()
        {
            return await Task.FromResult(GetHttpGetResponse<List<LotteryInfo>>(nameof(GetLotteryType)));
        }

        public async Task<TodaySummaryInfo> GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "lotteryID", lotteryID.ToString() },
                { "count", count.ToString() },
                { "isansy", isansy.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponse<TodaySummaryInfo>(nameof(GetPlayInfoSummaryInfo), queryStringParts));
        }

        public async Task<IEnumerable<LiveGameManage>> GetLiveGameManageInfos()
        {
            return await Task.FromResult(GetHttpGetResponse<IEnumerable<LiveGameManage>>(nameof(GetLiveGameManageInfos)));
        }

        public async Task<List<PlayTypeInfo>> GetPlayTypeInfo()
        {
            return await Task.FromResult(GetHttpGetResponse<List<PlayTypeInfo>>(nameof(GetPlayTypeInfo)));
        }

        public async Task<List<PlayTypeRadio>> GetPlayTypeRadio()
        {
            return await Task.FromResult(GetHttpGetResponse<List<PlayTypeRadio>>(nameof(GetPlayTypeRadio)));
        }

        public async Task<TodaySummaryInfo> GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "start", start.ToFormatDateTimeMillisecondsString() },
                { "end", end.ToFormatDateTimeMillisecondsString() },
                { "lotteryID", lotteryID.ToString() },
                { "count", count.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponse<TodaySummaryInfo>(nameof(GetTodaySummaryInfo), queryStringParts));
        }

        public async Task<List<CurrentLotteryInfo>> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            return await Task.FromResult(GetHttpPostResponse<List<CurrentLotteryInfo>>(nameof(QueryCurrentLotteryInfo), query));
        }
    }
}
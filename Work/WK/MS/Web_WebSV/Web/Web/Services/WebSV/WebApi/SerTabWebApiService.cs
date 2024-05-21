using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using JxBackendService.Common.Util;
using System.Web;
using Web.Services.WebSV.Base;

namespace Web.Services.WebSV.WebApi
{
    public class SerTabWebApiService : BaseWebSVService, ISerTabWebSVService
    {
        protected override string RemoteControllerName => "SerTabService";

        public CurrentLotteryInfo GetLotteryInfos(int lotteryid)
        {
            return GetHttpGetResponse<CurrentLotteryInfo>($"{nameof(lotteryid)}={HttpUtility.UrlEncode(lotteryid.ToString())}");
        }

        public List<LotteryInfo> GetLotteryType()
        {
            return GetHttpGetResponse<List<LotteryInfo>>();
        }

        public TodaySummaryInfo GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "lotteryID", lotteryID.ToString() },
                { "count", count.ToString() },
                { "isansy", isansy.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<TodaySummaryInfo>(queryStringParts);
        }

        public List<PlayTypeInfo> GetPlayTypeInfo()
        {
            return GetHttpGetResponse<List<PlayTypeInfo>>();
        }

        public List<PlayTypeRadio> GetPlayTypeRadio()
        {
            return GetHttpGetResponse<List<PlayTypeRadio>>();
        }

        public TodaySummaryInfo GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "start", start.ToFormatDateTimeMillisecondsString() },
                { "end", end.ToFormatDateTimeMillisecondsString() },
                { "lotteryID", lotteryID.ToString() },
                { "count", count.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<TodaySummaryInfo>(queryStringParts);
        }

        public List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            return GetHttpPostResponse<List<CurrentLotteryInfo>>(query);
        }
    }
}
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Specialized;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class CQ9SLSqliteTokenService : ISqliteTokenService
    {
        private readonly CQ9SLSharedAppSetting _appSetting;

        public CQ9SLSqliteTokenService()
        {
            _appSetting = CQ9SLSharedAppSetting.Instance;
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            //時區的關係，統一用UTC時間計算
            DateTime lastCrawlStartDate = DateTime.UtcNow.AddMinutes(-CQ9SLSharedAppSetting.Instance.MaxSearchRangeMinutes);

            if (requestAndResponse != null && !requestAndResponse.ResponseContent.IsNullOrEmpty())
            {
                var uri = new Uri(new Uri(_appSetting.APIUrl), requestAndResponse.RequestBody);
                NameValueCollection queryStringParams = HttpUtility.ParseQueryString(uri.Query);
                lastCrawlStartDate = DateTimeOffset.Parse(queryStringParams[_appSetting.QueryStringKey.StartTime]).UtcDateTime;
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = lastSearchToken.ToInt64().ToDateTime();
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(_appSetting.MaxSearchRangeMinutes);
            DateTime maxSearchEndDate = DateTime.UtcNow;

            if (nextStartSearchDate > maxSearchEndDate)
            {
                nextStartSearchDate = maxSearchEndDate;
            }

            nextStartSearchDate = nextStartSearchDate.AddMinutes(-_appSetting.OverlapMinutesPerRequest);

            return nextStartSearchDate.ToUnixOfTime().ToString();
        }
    }
}
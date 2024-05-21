using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.AWCSP;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class AWCSPSqliteTokenService : ISqliteTokenService
    {
        public AWCSPSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameAWCSPApiService.MaxSearchRangeMinutes);

            if (requestAndResponse != null && !requestAndResponse.ResponseContent.IsNullOrEmpty())
            {
                string endTime = requestAndResponse.RequestBody.Deserialize<AWCSPGetBetLogRequestModel>().EndTime;
                lastCrawlStartDate = HttpUtility.UrlDecode(endTime).ToDateTime(TPGameAWCSPApiService.AWCSPRequestDateTimeFormat);
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            return lastCrawlStartDate.AddMinutes(-TPGameAWCSPApiService.BackSqliteSearchDateMinutes).ToFormatDateTimeString();
        }
    }
}
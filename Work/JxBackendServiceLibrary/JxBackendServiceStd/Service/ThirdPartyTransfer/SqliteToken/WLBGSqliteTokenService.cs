using System;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.WLBG;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class WLBGSqliteTokenService : ISqliteTokenService
    {
        public WLBGSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now;

            if (requestAndResponse != null && !requestAndResponse.ResponseContent.IsNullOrEmpty())
            {
                lastCrawlStartDate = requestAndResponse.RequestBody.Deserialize<WLBGGetBetLogRequestModel>()
                    .Until.ToDateTime(TPGameWLBGApiService.WLRequestDateTimeFormat);
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            return lastCrawlStartDate.AddMinutes(-TPGameWLBGApiService.BackSqliteSearchDateMinutes).ToFormatDateTimeString();
        }
    }
}
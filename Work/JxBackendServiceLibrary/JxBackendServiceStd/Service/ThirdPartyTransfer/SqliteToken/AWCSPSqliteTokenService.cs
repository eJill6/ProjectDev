using System;
using System.Web;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.AWCSP;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class AWCSPSqliteTokenService : ISqliteTokenService
    {
        public AWCSPSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameAWCSPApiService.MaxSearchRangeMinutes);

            if (dataModel != null && !dataModel.ResponseContent.IsNullOrEmpty())
            {
                string endTime = dataModel.RequestBody.Deserialize<AWCSPGetBetLogRequestModel>().EndTime;
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
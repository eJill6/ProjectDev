using System;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class OBSPSqliteTokenService : ISqliteTokenService
    {
        public OBSPSqliteTokenService()
        {

        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameOBSPApiService.MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameOBSPApiService.MaxSearchRangeMinutes);
            DateTime maxSearchEndDate = DateTime.Now;

            if (nextStartSearchDate > maxSearchEndDate)
            {
                nextStartSearchDate = maxSearchEndDate;
            }

            return nextStartSearchDate.AddMinutes(-TPGameOBSPApiService.BackSqliteSearchDateMinutes).ToFormatDateTimeString();
        }
    }
}

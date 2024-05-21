using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class OBEBSqliteTokenService : ISqliteTokenService
    {
        public OBEBSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameOBEBApiService.MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameOBEBApiService.MaxSearchRangeMinutes);
            DateTime maxSearchEndDate = DateTime.Now;

            if (nextStartSearchDate > maxSearchEndDate)
            {
                nextStartSearchDate = maxSearchEndDate;
            }

            return nextStartSearchDate.AddMinutes(-TPGameOBEBApiService.BackSqliteSearchDateMinutes).ToFormatDateTimeString();
        }
    }
}
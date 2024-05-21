using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class IMKYSqliteTokenService : ISqliteTokenService
    {
        public IMKYSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameIMOneApiService.MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameIMOneApiService.MaxSearchRangeMinutes);
            DateTime maxSearchEndDate = DateTime.Now;

            if (nextStartSearchDate > maxSearchEndDate)
            {
                nextStartSearchDate = maxSearchEndDate;
            }

            return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }
    }
}
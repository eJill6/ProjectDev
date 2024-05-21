using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class EVEBSqliteTokenService : ISqliteTokenService
    {
        public EVEBSqliteTokenService()
        {

        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameEVEBApiService.MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                if (long.TryParse(lastSearchToken, out long timeStamp))
                {
                    lastCrawlStartDate = timeStamp.ToDateTime();
                }
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameEVEBApiService.MaxSearchRangeMinutes);

            if (nextStartSearchDate > DateTime.Now)
            {
                nextStartSearchDate = DateTime.Now;
            }

            return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }
    }
}

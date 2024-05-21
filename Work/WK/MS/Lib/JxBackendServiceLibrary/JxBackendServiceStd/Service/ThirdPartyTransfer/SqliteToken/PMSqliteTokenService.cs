using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class PMSqliteTokenService : ISqliteTokenService
    {
        public PMSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGamePMApiService.MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                if (long.TryParse(lastSearchToken, out long timeStamp))
                {
                    lastCrawlStartDate = timeStamp.ToDateTime();
                }
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGamePMApiService.MaxSearchRangeMinutes);

            //跨日時 要特別處理 從00:01開始 因為後面會-1
            if (lastCrawlStartDate.Day != nextStartSearchDate.Day)
            {
                nextStartSearchDate = nextStartSearchDate.Date.AddMinutes(1);
            }

            if (nextStartSearchDate > DateTime.Now)
            {
                nextStartSearchDate = DateTime.Now;
            }

            return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }
    }
}
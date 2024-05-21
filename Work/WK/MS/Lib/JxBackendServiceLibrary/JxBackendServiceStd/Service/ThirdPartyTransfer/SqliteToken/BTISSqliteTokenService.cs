using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class BTISSqliteTokenService : ISqliteTokenService
    {
        private readonly int _overlapMinutesPerRequest = 60;

        public BTISSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            DateTime lastCrawlStartDate = DateTime.UtcNow.AddMinutes(-TPGameBTISApiService.MaxSearchRangeMinutes);

            if (dataModel != null && !dataModel.ResponseContent.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(dataModel.RequestBody.Deserialize<BettingHistoryRequest>().From);
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameBTISApiService.MaxSearchRangeMinutes);
            DateTime maxSearchEndDate = DateTime.UtcNow.AddMinutes(-TPGameBTISApiService.MaxSearchMinutesAgo);

            if (nextStartSearchDate > maxSearchEndDate)
            {
                nextStartSearchDate = maxSearchEndDate;
            }

            nextStartSearchDate = nextStartSearchDate.AddMinutes(-_overlapMinutesPerRequest); //overlap

            return nextStartSearchDate.ToFormatDateTimeString();
        }
    }
}
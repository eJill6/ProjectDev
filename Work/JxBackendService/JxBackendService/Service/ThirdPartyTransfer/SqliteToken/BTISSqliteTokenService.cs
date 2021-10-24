using System;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class BTISSqliteTokenService : ISqliteTokenService
    {
        public BTISSqliteTokenService()
        {

        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse dataModel)
        {
            DateTime lastCrawlStartDate = DateTime.UtcNow.AddMinutes(-TPGameBTISApiService.MaxSearchRangeMinutes);

            if (dataModel != null && !dataModel.ResponseContent.IsNullOrEmpty())
            {
                lastCrawlStartDate = dataModel.RequestBody.Deserialize<BettingHistoryRequest>().From;
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameBTISApiService.MaxSearchRangeMinutes);
            DateTime maxSearchEndDate = DateTime.UtcNow.AddMinutes(-6);

            if (nextStartSearchDate > maxSearchEndDate)
            {
                nextStartSearchDate = maxSearchEndDate;
            }

            return nextStartSearchDate.AddMinutes(-1).ToFormatDateTimeString();
        }
    }
}

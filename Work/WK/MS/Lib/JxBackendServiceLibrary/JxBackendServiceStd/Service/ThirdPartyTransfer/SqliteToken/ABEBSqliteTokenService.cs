using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class ABEBSqliteTokenService : ISqliteTokenService
    {
        public ABEBSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameABEBApiService.MaxSearchRangeMinutes);

            if (requestAndResponse != null && !requestAndResponse.ResponseContent.IsNullOrEmpty())
            {
                string[] responseContents = requestAndResponse.ResponseContent.Deserialize<string[]>();
                var requestModels = responseContents[0].Deserialize<ABBetLogResponseModel>();

                lastCrawlStartDate = DateTime.Parse(requestModels.Data.StartDateTime);
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                if (long.TryParse(lastSearchToken, out long timeStamp))
                {
                    lastCrawlStartDate = timeStamp.ToDateTime();
                }
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameABEBApiService.MaxSearchRangeMinutes);

            if (nextStartSearchDate > DateTime.Now)
            {
                nextStartSearchDate = DateTime.Now;
            }

            return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }
    }
}
using System;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class IMLotterySqliteTokenService : ISqliteTokenService
    {
        public IMLotterySqliteTokenService()
        {

        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime lastCrawlStartDate = DateTime.Now.AddMinutes(-TPGameIMOneApiService.MaxSearchRangeMinutes);

            if (requestAndResponse != null && !requestAndResponse.RequestBody.IsNullOrEmpty())
            {
                lastCrawlStartDate = GameIMUtil.ToBetRecordDateTime(requestAndResponse.RequestBody.Deserialize<IMGetBetLogRequestModel>().StartDate);
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                if (long.TryParse(lastSearchToken, out long timeStamp))
                {
                    lastCrawlStartDate = timeStamp.ToDateTime();
                }
            }

            DateTime nextStartSearchDate = lastCrawlStartDate.AddMinutes(TPGameIMOneApiService.MaxSearchRangeMinutes);

            if (nextStartSearchDate > DateTime.Now)
            {
                nextStartSearchDate = DateTime.Now;
            }

            return nextStartSearchDate.AddMinutes(-1).ToUnixOfTime().ToString();
        }
    }
}

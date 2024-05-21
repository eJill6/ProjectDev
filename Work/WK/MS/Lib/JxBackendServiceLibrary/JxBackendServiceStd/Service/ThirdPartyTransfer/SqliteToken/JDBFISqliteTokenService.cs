using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums.ThirdParty.JDB;
using JxBackendService.Model.ThirdParty.JDB.JDBFI;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class JDBFISqliteTokenService : ISqliteTokenService
    {
        public JDBFISqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            DateTime serverDate = ToServerDate(DateTime.Now);

            DateTime lastCrawlStartDate = serverDate.AddMinutes(-JDBApiAction.QueryBetLogRecently.SearchRangeMinutes);

            if (requestAndResponse != null && !requestAndResponse.ResponseContent.IsNullOrEmpty())
            {
                lastCrawlStartDate = requestAndResponse.RequestBody.Deserialize<JDBFIGetBetLogRequestModel>()
                    .Endtime.ToDateTime(TPGameJDBFIApiService.JDBRequestDateTimeFormat);
            }
            else if (!lastSearchToken.IsNullOrEmpty())
            {
                lastCrawlStartDate = DateTime.Parse(lastSearchToken);
            }

            return lastCrawlStartDate.ToFormatDateTimeString();
        }

        /// <summary>
        /// +8時間轉成API server時區
        /// </summary>
        /// <param name="chinaDatetime"></param>
        /// <returns></returns>
        private DateTime ToServerDate(DateTime chinaDatetime)
        {
            return chinaDatetime.AddHours(-8).AddHours(TPGameJDBFIApiService.ServerTimeZone);
        }
    }
}
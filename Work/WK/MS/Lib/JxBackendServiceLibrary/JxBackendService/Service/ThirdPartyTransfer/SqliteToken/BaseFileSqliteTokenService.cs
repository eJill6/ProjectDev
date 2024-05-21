using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class BaseFileSqliteTokenService : ISqliteTokenService
    {
        private readonly string InitializationToken = "0";

        public BaseFileSqliteTokenService()
        {

        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            // 初始化 TOKEN 帶 0
            if (lastSearchToken.IsNullOrEmpty())
            {
                return InitializationToken;
            }

            if (requestAndResponse != null && !requestAndResponse.RequestBody.IsNullOrEmpty())
            {
                // 讓下載FTP平台可從RequestBody 取得下次交換TOKEN KEY
                return requestAndResponse.RequestBody;
            }

            throw new System.NotImplementedException();
        }
    }
}

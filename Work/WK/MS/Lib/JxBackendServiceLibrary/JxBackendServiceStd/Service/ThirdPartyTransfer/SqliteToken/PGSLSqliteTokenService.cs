using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class PGSLSqliteTokenService : ISqliteTokenService
    {
        private readonly int _maxSearchDaysBefore = 30;

        private readonly int _initSearchHoursBefore = 2;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        private EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = s_environmentService.Value.Application,
            LoginUser = new BasicUserInfo()
        };

        public PGSLSqliteTokenService()
        {
        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            if (lastSearchToken.IsNullOrEmpty())
            {
                return DateTime.Now.AddHours(-_initSearchHoursBefore).ToUnixOfTime().ToString();
            }

            var pgbetLogResponseModel = requestAndResponse.ResponseContent.Deserialize<PGBetLogResponseModel>();

            if (pgbetLogResponseModel.data.Any())
            {
                return pgbetLogResponseModel.data.Max(x => x.rowVersion).ToString();
            }
            else
            {
                //TOKEN超過30天以上強迫更新為30天前(規格最多可查60天)
                lastSearchToken = ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                {
                    DateTime tokenDate = lastSearchToken.ToInt64().ToDateTime();
                    DateTime lastSearchTokenDate = DateTime.UtcNow.AddDays(-_maxSearchDaysBefore);

                    if (tokenDate < lastSearchTokenDate)
                    {
                        return lastSearchTokenDate.ToUnixOfTime().ToString();
                    }

                    return lastSearchToken;
                });

                return lastSearchToken;
            }
        }
    }
}
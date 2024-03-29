﻿using System;
using System.Linq;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Service.ThirdPartyTransfer.SqliteToken
{
    public class PGSLSqliteTokenService : ISqliteTokenService
    {
        private readonly int _maxSearchDaysBefore = 30;
        private readonly string _initSearchToken = "1";

        private EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = JxApplication.PGSLTransferService,
            LoginUser = new BasicUserInfo() { UserId = 0, UserName = GlobalVariables.SystemOperator }
        };

        public PGSLSqliteTokenService()
        {

        }

        public string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse requestAndResponse)
        {
            //第一次丟1
            if (lastSearchToken.IsNullOrEmpty())
            {
                return _initSearchToken;
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

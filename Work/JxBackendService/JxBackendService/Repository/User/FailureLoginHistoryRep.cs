using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.db;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.User
{
    public class FailureLoginHistoryRep : BaseDbRepository<FailureLoginHistory>, IFailureLoginHistoryRep
    {
        public FailureLoginHistoryRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public int GetFailureLoginTimes(string userName)
        {
            string sql = $@"
                SELECT COUNT(*) 
                FROM {InlodbType.Inlodb}.[dbo].[FailureLoginHistory] WITH(NOLOCK)
                WHERE 
                    UserName = @userName 
                    AND LoginTime >= @StartLoginTime 
                    AND LoginTime < @EndLoginTime ";

            object param = new
            {
                userName = userName.ToNVarchar(50),
                StartLoginTime = DateTime.Now.Date,
                EndLoginTime = DateTime.Now.ToQuerySmallThanTime(DatePeriods.Day)
            };

            return DbHelper.ExecuteScalar<int>(sql, param);
        }

        public void DeleteFailureLoginHistory(string userName)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_DeleteFailureLoginHistory";
            DbHelper.Execute(sql, new { userName = userName.ToNVarchar(50) }, CommandType.StoredProcedure);
        }
    }
}

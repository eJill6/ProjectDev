using Dapper;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository
{
    public class ConfigGroupRep : BaseDbRepository<ConfigGroup>, IConfigGroupRep
    {
        public ConfigGroupRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public ConfigGroup GetSingle(string groupName)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE GroupName = @groupName ";
            return DbHelper.QuerySingleOrDefault<ConfigGroup>(sql, new { groupName });
        }

        public int GetGroupSerial(string groupName)
        {
            return GetSingle(groupName).GroupSerial;
        }
    }
}

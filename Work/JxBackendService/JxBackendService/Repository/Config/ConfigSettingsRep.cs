using Dapper;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository
{
    public class ConfigSettingsRep : BaseDbRepository<ConfigSettings>, IConfigSettingsRep
    {
        public ConfigSettingsRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public List<ConfigSettings> GetList(string groupName, bool? isActive = null)
        {
            string groupFilter = null;

            if (isActive.HasValue)
            {
                groupFilter = " AND GroupActive = " + Convert.ToInt32(isActive.Value).ToString();
            }

            string sql = GetAllQuerySQL(InlodbType.Inlodb) +
                $"WHERE GroupSerial = (SELECT GroupSerial FROM [{InlodbType.Inlodb}].[dbo].[ConfigGroup] WITH(NOLOCK) " +
                $"WHERE GroupName = @groupName {groupFilter})";

            if (isActive.HasValue)
            {
                sql += " AND ItemActive = " + Convert.ToInt32(isActive.Value).ToString();
            }

            return DbHelper.QueryList<ConfigSettings>(sql, new { groupName = groupName.ToNVarchar(50) });
        }

        public ConfigSettings GetSingle(string groupName, int itemKey)
        {
            return GetList(groupName).SingleOrDefault(x => x.ItemKey == itemKey);
        }

        public bool IsActive(ConfigGroupNameEnum groupName, int itemKey)
        {
            string sql = $@"
                SELECT 1 FROM {InlodbType.Inlodb}.dbo.ConfigSettings CS WITH(NOLOCK)
                INNER JOIN {InlodbType.Inlodb}.dbo.ConfigGroup CG WITH(NOLOCK) ON CG.GroupSerial = CS.GroupSerial
                WHERE CG.GroupName = @groupName
                AND CS.ItemKey = @itemKey
                AND CG.GroupActive = 1
                AND CS.ItemActive = 1 ";

            return DbHelper.ExecuteScalar<int?>(sql, new { groupName = groupName.ToString().ToNVarchar(50), itemKey }) == 1;
        }
    }
}

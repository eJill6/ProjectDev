using System.Collections.Generic;
using Dapper;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Entity.VIP.Rule;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.TypeHandler;

namespace JxBackendService.Repository.VIP
{
    public class VIPLevelSettingRep : BaseDbRepository<VIPLevelSetting>, IVIPLevelSettingRep
    {
        static VIPLevelSettingRep()
        {
            SqlMapper.AddTypeHandler(typeof(List<RebateRateRule>), new JsonStringHandler());
            SqlMapper.AddTypeHandler(typeof(ChangeLevelRule), new JsonStringHandler());
            SqlMapper.AddTypeHandler(typeof(MonthlyDepositRule), new JsonStringHandler());
        }

        public VIPLevelSettingRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(
            envLoginUser, dbConnectionType)
        {
        }

        public List<VIPLevelSetting> GetAll()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE VIPLevel > 0";

            return DbHelper.QueryList<VIPLevelSetting>(sql, null);
        }
    }
}
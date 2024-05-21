using System.Data;
using JxBackendService.Interface.Repository.GlobalSystem;
using JxBackendService.Model.Entity.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.DBLog
{
    public class MethodInvocationLogRep : BaseDbRepository<MethodInvocationLog>, IMethodInvocationLogRep
    {
        public MethodInvocationLogRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public BaseReturnModel AddMultipleMethodInvocationLog(ProAddMultipleMethodInvocationLogParam param)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_AddMultipleMethodInvocationLog";

            return DbHelper.QuerySingle<SPReturnModel>(sql, param, CommandType.StoredProcedure).ToBaseReturnModel();
        }
    }
}
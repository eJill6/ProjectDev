using JxBackendService.Interface.Repository.Security;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.Security
{
    public class BlackAreaListRep : BaseDbRepository<BlackAreaList>, IBlackAreaListRep
    {
        public BlackAreaListRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public ExistInBlackAreaListResult GetExistInBlackAreaList(JxIpInformation jxIpInformation, BlackIpType blackIpType)
        {
            string sql = $"SELECT IsExist, Area FROM {InlodbType.Inlodb}.dbo.fn_GetExistInBlackAreaList(@IP, @IPVersion, @IPNum, @IType)";

            return DbHelper.QuerySingleOrDefault<ExistInBlackAreaListResult>(sql,
                new
                {
                    IP = jxIpInformation.DestinationIP.ToVarchar(128),
                    IPVersion = jxIpInformation.DestinationIPVersionNumber,
                    IPNum = jxIpInformation.DestinationIPNumberString.ToVarchar(128),
                    IType = blackIpType.Value
                });
        }
    }    
}

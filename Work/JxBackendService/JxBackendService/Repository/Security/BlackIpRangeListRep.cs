using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Security;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Security
{
    public class BlackIpRangeListRep : BaseDbRepository<BlackIpRangeList>, IBlackIpRangeListRep
    {
        public BlackIpRangeListRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public bool IsActive(JxIpInformation ipInformation, BlackIpType blackIpType)
        {
            string sql = $@"SELECT {InlodbType.Inlodb}.dbo.fn_IsExistInBlackIpRangeList(@IP, @IPVersion, @IPNum, @IType) ";
            object param = new
            {
                IP = ipInformation.DestinationIP.ToVarchar(128),
                IPVersion = ipInformation.DestinationIPVersionNumber,
                IPNum = ipInformation.DestinationIPNumberString.ToVarchar(128),
                IType = blackIpType.Value
            };

            return DbHelper.ExecuteScalar<bool>(sql, param);
        }
    }
}

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
    public class BlackIpListRep : BaseDbRepository<BlackIpList>, IBlackIpListRep
    {
        public BlackIpListRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public BlackIpList GetSingle(string ipAddress, BlackIpType blackIpType)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE Ip = @ipAddress AND IType = @itype ";
            //在db沒uniquekey的情況下，先確保這些單支的方法都不會出錯，改為QueryFirstOrDefault
            //TODO: 之後要加上 BlackIpList 的 uniquekey，以及將多的資料砍掉
            //return DbHelper.QuerySingleOrDefault<BlackIpList>(sql, new { ipAddress = ipAddress.ToVarchar(128), itype = blackIpType.Value });
            return DbHelper.QueryFirstOrDefault<BlackIpList>(sql, new { ipAddress = ipAddress.ToVarchar(128), itype = blackIpType.Value });
        }
    }
}

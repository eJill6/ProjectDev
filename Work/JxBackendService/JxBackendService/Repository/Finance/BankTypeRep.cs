using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;

namespace JxBackendService.Repository.Finance
{
    public class BankTypeRep : BaseDbRepository<BankType>, IBankTypeRep
    {
        public BankTypeRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public List<BankType> GetVisibleList()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE Visible = 1 ";
            return DbHelper.QueryList<BankType>(sql, null);
        }        
    }
}

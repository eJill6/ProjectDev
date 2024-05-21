using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;

namespace JxBackendService.Repository.Game
{
    public class FrontsideMenuTypeRep : BaseDbRepository<FrontsideMenuType>, IFrontsideMenuTypeRep
    {
        public FrontsideMenuTypeRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<FrontsideMenuType> GetAll()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb);

            return DbHelper.QueryList<FrontsideMenuType>(sql, new { });
        }
    }
}
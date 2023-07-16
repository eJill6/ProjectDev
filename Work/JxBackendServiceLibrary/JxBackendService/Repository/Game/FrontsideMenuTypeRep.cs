using JxBackendService.Interface.Repository;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using Entity = JxBackendService.Model.Entity;

namespace JxBackendService.Repository.Game
{
    public class FrontsideMenuTypeRep : BaseDbRepository<Entity.FrontsideMenuType>, IFrontsideMenuTypeRep
    {
        public FrontsideMenuTypeRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<Entity.FrontsideMenuType> GetAll()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb);

            return DbHelper.QueryList<Entity.FrontsideMenuType>(sql, new { });
        }
    }
}
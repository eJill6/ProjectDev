using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;

namespace JxBackendService.Repository.Game
{
    public class FrontsideMenuRep : BaseDbRepository<FrontsideMenu>, IFrontsideMenuRep
    {
        public FrontsideMenuRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<FrontsideMenu> GetAll()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb);
            return DbHelper.QueryList<FrontsideMenu>(sql, new { });
        }

        public List<FrontsideMenu> GetActiveFrontsideMenu()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + " WHERE IsActive = 1 ";
            return DbHelper.QueryList<FrontsideMenu>(sql, null);
        }

        public List<FrontsideMenu> GetAllByType(int type)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + " WHERE Type = @type ";
            return DbHelper.QueryList<FrontsideMenu>(sql, new { type });
        }
    }
}
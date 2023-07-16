using System.Collections.Generic;
using System.Data;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.BackSideUser
{
    public class BWRolePermissionRep : BaseDbRepository<BWRolePermission>, IBWRolePermissionRep
    {
        public BWRolePermissionRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<BWRolePermission> GetRolePermissionList(int roleId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE RoleID = @RoleID ";

            return DbHelper.QueryList<BWRolePermission>(sql, new { RoleID = roleId });
        }

        public BaseReturnModel SaveBWRolePermission(ProSaveBWRolePermission saveParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_SaveBWRolePermission";

            return DbHelper.QuerySingle<SPReturnModel>(sql, saveParam, CommandType.StoredProcedure).ToBaseReturnModel();
        }
    }
}
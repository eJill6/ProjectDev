using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Collections.Generic;

namespace JxBackendService.Repository.BackSideUser
{
    public class BWRoleInfoRep : BaseDbRepository<BWRoleInfo>, IBWRoleInfoRep
    {
        public BWRoleInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public PagedResultModel<BWRoleInfo> GetList(QueryBWRoleInfoParam queryParam)
        {
            queryParam.SortModels = new List<SortModel>
            {
                new SortModel { ColumnName = nameof(BWRoleInfo.RoleID)},
            };

            PagedSqlQueryParamsModel param = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                RequestParam = queryParam
            });

            return DbHelper.PagedSqlQuery<BWRoleInfo>(param);
        }

        public List<BWRoleInfo> GetRoleInfoByRoleName(string roleName)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE RoleName = @RoleName ";

            return DbHelper.QueryList<BWRoleInfo>(sql, new { RoleName = roleName.ToNVarchar(50) });
        }

        public List<BWRoleInfo> GetBWRoleInfos(List<int> roleIds)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE RoleID IN @RoleIDs ";

            return DbHelper.QueryList<BWRoleInfo>(sql, new { RoleIDs = roleIds });
        }

        public List<BWRoleInfo> GetAllBWRoleInfos()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb);

            return DbHelper.QueryList<BWRoleInfo>(sql, null);
        }
    }
}
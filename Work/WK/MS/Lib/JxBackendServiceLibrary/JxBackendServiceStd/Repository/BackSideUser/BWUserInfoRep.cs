using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Repository.BackSideUser
{
    public class BWUserInfoRep : BaseDbRepository<BWUserInfo>, IBWUserInfoRep
    {
        public BWUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public PagedResultModel<BWUserInfo> GetList(QueryBWUserInfoParam queryParam)
        {
            string whereString = QueryString(queryParam.UserName, roleId: null);

            queryParam.SortModels = new List<SortModel>
            {
                new SortModel { ColumnName = nameof(BWUserInfo.UserID)},
            };

            PagedSqlQueryParamsModel param = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                WhereString = whereString,
                Parameters = new
                {
                    UserName = queryParam.UserName.ToNVarchar(50)
                },
                RequestParam = queryParam
            });

            return DbHelper.PagedSqlQuery<BWUserInfo>(param);
        }

        public BWUserInfo GetUserInfoByUserName(string userName)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) +
                " WHERE UserName = @UserName ";

            return DbHelper.QuerySingleOrDefault<BWUserInfo>(sql, new { UserName = userName.ToNVarchar(50) });
        }

        public List<int> GetUserIDsByRoleId(int roleId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string>() { nameof(BWUserInfo.UserID) }) +
                " WHERE RoleID = @roleId ";

            return DbHelper.QueryList<int>(sql, new { roleId });
        }

        public bool IsExistUserName(string userName)
        {
            string sql = QueryIsExistSql(userName, roleId: null);

            return DbHelper.ExecuteScalar<int>(sql, new { UserName = userName.ToNVarchar(50) }) > 0;
        }

        public bool IsExistRoleId(int roleId)
        {
            string sql = QueryIsExistSql(userName: null, roleId);

            return DbHelper.ExecuteScalar<int>(sql, new { RoleID = roleId }) > 0;
        }

        private string QueryIsExistSql(string userName, int? roleId)
        {
            string whereString = QueryString(userName, roleId);

            return $@"
				SELECT COUNT(1)
				{GetFromTableSQL(InlodbType.Inlodb)}
				{whereString}";
        }

        private string QueryString(string userName, int? roleId)
        {
            var whereString = new StringBuilder(" WHERE 1 = 1 ");

            if (!userName.IsNullOrEmpty())
            {
                whereString.Append(" AND UserName = @UserName ");
            }

            if (roleId.GetValueOrDefault() > 0)
            {
                whereString.Append(" AND RoleID = @RoleID ");
            }

            return whereString.ToString();
        }
    }
}
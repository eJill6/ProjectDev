using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Collections.Generic;
using System.Text;

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
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + " WHERE Type = @type ORDER BY Sort";
            return DbHelper.QueryList<FrontsideMenu>(sql, new { type });
        }

        public FrontsideMenu GetByUniqueKey(string productCode, string gameCode, string remoteCode)
        {
            string whereCondition = @" 
                WHERE 
                    ProductCode = @ProductCode 
                    AND GameCode = @GameCode 
                    AND RemoteCode = @RemoteCode ";

            string sql = GetAllQuerySQL(InlodbType.Inlodb) + whereCondition;

            return DbHelper.QuerySingleOrDefault<FrontsideMenu>(sql,
                new
                {
                    ProductCode = productCode.ToNVarchar(50),
                    GameCode = gameCode.ToNVarchar(50),
                    RemoteCode = remoteCode.ToNVarchar(50),
                });
        }

        public PagedResultModel<FrontsideMenu> GetPagedFrontsideMenu(QueryFrontsideMenuParam queryParam)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 ");

            if (!queryParam.TypeValue.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND Type = @Type");
            }

            if (!queryParam.MenuName.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND MenuName LIKE '%' + @MenuName + '%' ");
            }

            if (queryParam.StartDate.HasValue && queryParam.EndDate.HasValue)
            {
                whereString.AppendLine($" AND UpdateDate >= @StartDate AND UpdateDate <= @EndDate ");
            }

            if (queryParam.MinSort.HasValue)
            {
                whereString.AppendLine($" AND Sort >= @MinSort ");
            }

            if (queryParam.MaxSort.HasValue)
            {
                whereString.AppendLine($" AND Sort <= @MaxSort ");
            }

            queryParam.SortModels = new List<SortModel>
            {
                new SortModel { ColumnName = nameof(FrontsideMenu.Type)},
                new SortModel { ColumnName = nameof(FrontsideMenu.Sort)}
            };

            PagedSqlQueryParamsModel param = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                WhereString = whereString.ToString(),
                Parameters = new
                {
                    Type = queryParam.TypeValue.ToInt32(),
                    MenuName = queryParam.MenuName.ToNVarchar(50),
                    queryParam.StartDate,
                    queryParam.EndDate,
                    queryParam.MinSort,
                    queryParam.MaxSort,
                },
                RequestParam = queryParam
            });

            return DbHelper.PagedSqlQuery<FrontsideMenu>(param);
        }
    }
}
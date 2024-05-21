using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Game;
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
    public class GameLobbyListRep : BaseDbRepository<GameLobbyList>, IGameLobbyListRep
    {
        public GameLobbyListRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<GameLobbyList> GetActiveGameLobbyList(string thirtyPartyCode)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + " WHERE IsActive = 1 AND ThirtyPartyCode = @thirtyPartyCode ORDER BY Sort";

            return DbHelper.QueryList<GameLobbyList>(sql,
                new
                {
                    ThirtyPartyCode = thirtyPartyCode.ToVarchar(50)
                });
        }

        public GameLobbyList GetByCodes(string thirdPartyCode, string gameCode)
        {
            string whereCondition = @"
                WHERE
                    ThirtyPartyCode = @ThirtyPartyCode
                    AND WebGameCode = @GameCode ";

            string sql = GetAllQuerySQL(InlodbType.Inlodb) + whereCondition;

            return DbHelper.QuerySingleOrDefault<GameLobbyList>(sql,
                new
                {
                    ThirtyPartyCode = thirdPartyCode.ToNVarchar(50),
                    GameCode = gameCode.ToNVarchar(50),
                });
        }

        public PagedResultModel<GameLobbyList> GetPagedModel(SlotGameManageQueryParam queryParam)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 ");

            if (!queryParam.ThirdPartyCode.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND ThirtyPartyCode = @ThirdPartyCode");
            }

            if (!queryParam.GameName.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND ChineseName LIKE '%' + @GameName + '%' ");
            }

            queryParam.SortModels = new List<SortModel>
            {
                new SortModel { ColumnName = nameof(FrontsideMenu.Sort)}
            };

            PagedSqlQueryParamsModel param = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                WhereString = whereString.ToString(),
                Parameters = new
                {
                    ThirdPartyCode = queryParam.ThirdPartyCode.ToVarchar(50),
                    GameName = queryParam.GameName.ToNVarchar(100),
                },
                RequestParam = queryParam
            });

            return DbHelper.PagedSqlQuery<GameLobbyList>(param);
        }
    }
}
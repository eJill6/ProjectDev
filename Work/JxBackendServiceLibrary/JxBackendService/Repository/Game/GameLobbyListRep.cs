using System.Collections.Generic;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.Game
{
    public class GameLobbyListRep : BaseDbRepository<GameLobbyList>, IGameLobbyListRep
    {
        public GameLobbyListRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<GameLobbyInfo> GetActiveGameLobbyList(string thirtyPartyCode)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + " WHERE IsActive = 1 AND ThirtyPartyCode = @thirtyPartyCode ORDER BY Sort";

            return DbHelper.QueryList<GameLobbyInfo>(sql,
                new
                {
                    ThirtyPartyCode = thirtyPartyCode.ToVarchar(50)
                });
        }
    }
}
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Service.Game
{
    public class GameLobbyListService : BaseService, IGameLobbyListService
    {
        private readonly IGameLobbyListRep _gameLobbyListRep;

        private readonly IJxCacheService _jxCacheService;

        public GameLobbyListService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _gameLobbyListRep = ResolveJxBackendService<IGameLobbyListRep>();
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
        }

        public List<GameLobbyInfo> GetActiveGameLobbyList(GameLobbyType gameLobbyType)
        {
            return _jxCacheService.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.ActiveGameLobbyList(gameLobbyType),
                    IsCloneInstance = false,
                    IsSlidingExpiration = true,
                },
                () => _gameLobbyListRep.GetActiveGameLobbyList(gameLobbyType.Value));
        }

        public Dictionary<string, GameLobbyInfo> GetActiveGameLobbyMap(GameLobbyType gameLobbyType)
        {
            return _jxCacheService.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.ActiveGameLobbyMap(gameLobbyType),
                    IsCloneInstance = false,
                    IsSlidingExpiration = true,
                },
                () => _gameLobbyListRep.GetActiveGameLobbyList(gameLobbyType.Value).ToDictionary(d => d.WebGameCode));
        }
    }
}
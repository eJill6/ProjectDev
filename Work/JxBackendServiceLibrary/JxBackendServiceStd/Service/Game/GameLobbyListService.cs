using Flurl;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System.Collections.Generic;
using System.Linq;

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
                },
                () => GetActiveGameLobbyInfos(gameLobbyType));
        }

        public Dictionary<string, GameLobbyInfo> GetActiveGameLobbyMap(GameLobbyType gameLobbyType)
        {
            return _jxCacheService.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.ActiveGameLobbyMap(gameLobbyType),
                    IsCloneInstance = false,
                },
                () => GetActiveGameLobbyInfos(gameLobbyType).ToDictionary(d => d.WebGameCode));
        }

        private List<GameLobbyInfo> GetActiveGameLobbyInfos(GameLobbyType gameLobbyType)
        {
            List<GameLobbyInfo> infos = _gameLobbyListRep.GetActiveGameLobbyList(gameLobbyType.Value);

            infos.ForEach(info =>
            {
                info.FullImageUrl = !info.ImageUrl.IsNullOrEmpty() 
                ? Url.Combine(SharedAppSettings.BucketCdnDomain, info.ImageUrl) 
                : string.Empty;
            });

            return infos;
        }
    }
}
using Flurl;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Game
{
    public class GameLobbyListService : BaseService, IGameLobbyListService
    {
        private readonly Lazy<IGameLobbyListRep> _gameLobbyListRep;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private readonly Lazy<IHttpWebRequestUtilService> _httpWebRequestUtilService;

        public GameLobbyListService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _gameLobbyListRep = ResolveJxBackendService<IGameLobbyListRep>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _httpWebRequestUtilService = ResolveJxBackendService<IHttpWebRequestUtilService>();
        }

        public List<GameLobbyInfo> GetActiveGameLobbyList(GameLobbyType gameLobbyType)
        {
            return _jxCacheService.Value.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.ActiveGameLobbyList(gameLobbyType),
                    IsCloneInstance = false,
                },
                () => GetActiveGameLobbyInfos(gameLobbyType));
        }

        public Dictionary<string, GameLobbyInfo> GetActiveGameLobbyMap(GameLobbyType gameLobbyType)
        {
            return _jxCacheService.Value.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.ActiveGameLobbyMap(gameLobbyType),
                    IsCloneInstance = false,
                },
                () => GetActiveGameLobbyInfos(gameLobbyType).ToDictionary(d => d.MobileGameCode));
        }

        private List<GameLobbyInfo> GetActiveGameLobbyInfos(GameLobbyType gameLobbyType)
        {
            List<GameLobbyList> gameLobbyList = _gameLobbyListRep.Value.GetActiveGameLobbyList(gameLobbyType.Value);
            var gameLobbyInfos = new List<GameLobbyInfo>();

            foreach (GameLobbyList gameLobby in gameLobbyList)
            {
                var info = gameLobby.CastByJson<GameLobbyInfo>();
                string imageUrl = gameLobby.ImageUrl;
                gameLobby.ImageUrl = imageUrl; //容舊API

                if (!imageUrl.IsNullOrEmpty())
                {
                    info.FullImageUrl = _httpWebRequestUtilService.Value.CombineUrl(
                        SharedAppSettings.BucketCdnDomain,
                        imageUrl);

                    info.AESFullImageUrl = _httpWebRequestUtilService.Value.CombineUrl(
                        SharedAppSettings.BucketAESCdnDomain,
                        imageUrl.ConvertToAESExtension());
                }
                else
                {
                    info.FullImageUrl = string.Empty;
                    info.AESFullImageUrl = string.Empty;
                }

                gameLobbyInfos.Add(info);
            }

            return gameLobbyInfos;
        }
    }
}
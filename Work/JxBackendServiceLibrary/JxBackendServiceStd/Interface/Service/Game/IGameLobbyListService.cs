using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface IGameLobbyListService
    {
        List<GameLobbyInfo> GetActiveGameLobbyList(GameLobbyType gameLobbyType);

        Dictionary<string, GameLobbyInfo> GetActiveGameLobbyMap(GameLobbyType gameLobbyType);
    }
}
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace SLPolyGame.Web.Interface
{
    public interface ISlotApiWebSVService
    {
        List<GameLobbyInfo> GetGameList(string gameLobbyType);

        string GetJackpotAmount(string gameLobbyType);
    }
}
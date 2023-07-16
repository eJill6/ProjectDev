using System.Collections.Generic;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer
{
    public interface ISubGameService
    {
        List<GameLobbyInfo> GetVisibleGameList();
    }

    public interface IIMOneSubGameService : ISubGameService
    {
        string GetJackpotAmount();
    }
}
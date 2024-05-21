using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer
{
    public interface ISubGameService
    {
        List<GameLobbyInfo> GetVisibleGameList();

        string MobileApiBannerImageFileName { get; }

        string AESMobileApiBannerImageFileName { get; }
    }

    public interface IIMOneSubGameService : ISubGameService
    {
        string GetJackpotAmount();
    }
}
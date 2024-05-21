using ControllerShareLib.Models.Game;
using ControllerShareLib.Models.Game.GameLobby;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;

namespace ControllerShareLib.Interfaces.Service.Controller
{
    public interface IGameLobbyControllerService
    {
        string? GetJackpotAmount(GameLobbyType gameLobbyType);

        MobileApiThirdPartyGamesViewModel GetMobileApiThirdPartyGamesViewModel(GameLobbySubGameRequest request);

        WebThirdPartyGamesViewModel GetWebThirdPartyGamesViewModel(GameLobbySubGameRequest request);

        BaseReturnDataModel<AppOpenUrlInfo> LoginGameLobbyGame(LoginGameLobbyGameParam param);
    }
}
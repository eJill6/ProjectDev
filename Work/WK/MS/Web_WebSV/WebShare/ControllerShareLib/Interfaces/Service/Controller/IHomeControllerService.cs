using ControllerShareLib.Models.Game.Menu;
using JxBackendService.Model.ViewModel.Menu;

namespace ControllerShareLib.Interfaces.Service.Controller
{
    public interface IHomeControllerService
    {
        MobileApiGameCenterViewModel GetMobileApiGameLobbyMenu(bool isUseRequestHost, bool isForceRefresh);

        WebGameCenterViewModel GetWebGameLobbyMenu(bool isUseRequestHost);

        List<MobileApiMenuInnerInfo> GetMobileApiMenuInnerInfos(bool isUseRequestHost);
    }
}
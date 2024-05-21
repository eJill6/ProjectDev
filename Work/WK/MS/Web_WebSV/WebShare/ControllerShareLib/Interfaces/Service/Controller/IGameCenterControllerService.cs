using ControllerShareLib.Models.Game;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;

namespace ControllerShareLib.Interfaces.Service.Controller
{
    public interface IGameCenterControllerService
    {
        BaseReturnDataModel<EnterTPGameUrlInfo> GetWebEnterThirdPartyGame(EnterThirdPartyGameParam param);

        BaseReturnDataModel<MobileApiEnterTPGameUrlInfo> GetMobileApiEnterThirdPartyGame(EnterThirdPartyGameParam param);

        BaseReturnDataModel<CommonOpenUrlInfo> GetWebForwardGameUrl(BaseGameCenterLogin baseGameCenterLogin);
        
        BaseReturnDataModel<AppOpenUrlInfo> GetMobileApiForwardGameUrl(BaseGameCenterLogin baseGameCenterLogin);
    }
}
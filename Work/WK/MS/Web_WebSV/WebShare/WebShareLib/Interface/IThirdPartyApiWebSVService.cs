using JxBackendService.Model.Entity;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.Interface
{
    public interface IThirdPartyApiWebSVService
    {
        BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlSVApiParam param);

        FrontsideMenu GetActiveFrontsideMenu(string productCode, string gameCode);

        string GetTPGameLaunchURLHTML(string token);
    }
}
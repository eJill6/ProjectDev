using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;

namespace JxBackendService.Model.ViewModel.Game
{
    public class QueryFrontsideMenuModel : FrontsideMenu, IDataKey
    {
        public string TypeText => FrontsideMenuTypeSetting.GetName(Type);

        public string ThirdPartyName => FrontsideMenuSetting.GetSingle(PlatformProduct.GetSingle(ProductCode), ThirdPartySubGameCodes.GetSingle(GameCode))?.Name;

        public string IsActiveText => IsActive.GetActionName();

        public string KeyContent => No.ToString();
    }
}
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.Menu;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IFrontsideMenuService
    {
        List<FrontsideMenu> GetActiveFrontsideMenus(bool IsForceRefresh = false);

        WebGameCenterViewModel GetWebGameCenterViewModel();

        PagedResultModel<QueryFrontsideMenuModel> GetPagedFrontsideMenu(QueryFrontsideMenuParam queryParam);

        List<GameCenterManageModel> GetModelsByType(FrontsideMenuTypeSetting frontsideMenuTypeSetting);

        List<GameCenterManageDetail> GetAllByProduct(PlatformProduct product);

        List<JxBackendSelectListItem> GetTypes();

        string GetNameFromSetting(FrontsideMenu menu);

        List<MenuInnerInfo> GetMenuInnerInfos();

        void ForceRefreshFrontsideMenus();
    }
}
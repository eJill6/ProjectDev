using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IFrontsideMenuService
    {
        List<FrontsideMenu> GetActiveFrontsideMenus();

        FrontsideMenuViewModel GetFrontsideMenuViewModel();

        PagedResultModel<QueryFrontsideMenuModel> GetPagedFrontsideMenu(QueryFrontsideMenuParam queryParam);

        List<FrontsideMenu> GetAllByType(int type);

        List<GameCenterManageModel> GetModelsByType(int type);

        List<JxBackendSelectListItem> GetTypes();

        string GetNameFromSetting(FrontsideMenu menu);
    }
}
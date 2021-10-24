using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.HomeGameMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IFrontsideMenuService
    {
        List<FrontsideMenuModel> GetFrontsideAllMenu(bool isForceRefresh = false);

        List<FrontsideMenu> GetActiveFrontsideMenu();

        List<GameMenuViewModel> GetGameMenusForApi(string imageDomainName , List<GameMenuRecommendItemViewModel> recommendLotteryItems);

        FrontsideMenu GetSingle(string productCode);

        FrontsideMenu GetSingle(string productCode, string gameCode);

        List<FrontsideMenu> GetAllMenu();

        List<FrontsideMenuTypes> GetAllThirdPartyGameMenuType();

        List<JxBackendSelectListItem> GetFrontsideMenuRadioTagSetting();

        List<FrontsideMenu> GetFrontsideMenuByType(int frontsideMenuType);

        BaseReturnModel UpdateFrontsideMenuByType(int frontsideMenuType, List<FrontsideMenu> modifyFrontSideMenuList);
    }
}

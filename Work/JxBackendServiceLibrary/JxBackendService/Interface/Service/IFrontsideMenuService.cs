using JxBackendService.Model.Entity;
using JxBackendService.Model.ViewModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IFrontsideMenuService
    {
        List<FrontsideMenu> GetActiveFrontsideMenus();

        FrontsideMenuViewModel GetFrontsideMenuViewModel();
    }
}
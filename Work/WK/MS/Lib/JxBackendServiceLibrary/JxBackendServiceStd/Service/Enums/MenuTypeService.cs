using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using System.Collections.Generic;

namespace JxBackendService.Service.Enums
{
    public class MenuTypeService : BaseValueModelService<string, MenuType>, IMenuTypeService
    {
        protected override List<MenuType> CreateAllList()
        {
            List<MenuType> menuTypes = base.CreateAllList();

            if (SharedAppSettings.GetEnvironmentCode() != EnvironmentCode.Development)
            {
                menuTypes.RemoveAll(r => r == MenuType.Demo);
            }

            return menuTypes;
        }
    }
}
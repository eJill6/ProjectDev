using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IFrontsideMenuTypeService : IBaseValueModelService<int, FrontsideMenuTypeSetting>
    {
        List<FrontsideMenuTypeSetting> GetAllAndSortByDbValues(List<FrontsideMenuType> dbEntities);
    }
}
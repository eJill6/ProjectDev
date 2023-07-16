using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Enums
{
    public class FrontsideMenuTypeService : BaseValueModelService<int, FrontsideMenuTypeSetting>, IFrontsideMenuTypeService
    {
        public List<FrontsideMenuTypeSetting> GetAllAndSortByDbValues(List<FrontsideMenuType> dbEntities)
        {
            List<FrontsideMenuTypeSetting> frontsideMenuTypes = GetAll();

            return frontsideMenuTypes.OrderBy(type => dbEntities.Single(entity => entity.Id == type.Value).Sort).ToList();
        }
    }
}
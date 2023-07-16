using JxBackendService.Model.Entity;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IFrontsideMenuTypeRep : IBaseDbRepository<FrontsideMenuType>
    {
        List<FrontsideMenuType> GetAll();
    }
}
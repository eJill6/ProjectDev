using JxBackendService.Model.Entity;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IFrontsideMenuRep : IBaseDbRepository<FrontsideMenu>
    {
        List<FrontsideMenu> GetAll();

        List<FrontsideMenu> GetActiveFrontsideMenu();

        List<FrontsideMenu> GetAllByType(int type);
    }
}
using JxBackendService.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IFrontsideMenuRep : IBaseDbRepository<FrontsideMenu>
    {
        List<FrontsideMenu> GetAll();

        List<FrontsideMenu> GetActiveFrontsideMenu();

        List<FrontsideMenu> GetAllByType(int type);
    }
}

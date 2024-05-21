using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.Param.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface IFrontsideMenuRep : IBaseDbRepository<FrontsideMenu>
    {
        List<FrontsideMenu> GetAll();

        List<FrontsideMenu> GetActiveFrontsideMenu();

        List<FrontsideMenu> GetAllByType(int type);

        FrontsideMenu GetByUniqueKey(string productCode, string gameCode, string remoteCode);

        PagedResultModel<FrontsideMenu> GetPagedFrontsideMenu(QueryFrontsideMenuParam queryParam);

        bool IsFrontsideMainMenuExists(FrontSideMainMenu frontsideMainMenu);
        
        List<FrontsideMenu> GetGameCenterMenusByProduct(string productCode);
    }
}
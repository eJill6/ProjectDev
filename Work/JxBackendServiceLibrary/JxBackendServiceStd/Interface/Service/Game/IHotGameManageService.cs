using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface IHotGameManageService
    {
        BaseReturnModel Create(HotGameManageCreateParam param);

        BaseReturnModel Update(HotGameManageUpdateParam param);
        
        BaseReturnModel Delete(int no);
    }

    public interface IHotGameManageReadService
    {
        List<JxBackendSelectListItem> GetProductSelectListItems();

        List<JxBackendSelectListItem> GetActionSelectListItems();

        FrontsideMenu GetSingle(int no);

        PagedResultModel<HotGameManageModel> GetPagedModel(HotGameManageQueryParam param);
    }
}
using JxBackendService.Model.Common;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface ISlotGameManageService
    {
        BaseReturnModel Create(SlotGameManageCreateParam param);

        BaseReturnModel Update(SlotGameManageUpdateParam param);

        BaseReturnModel Delete(int no);
    }

    public interface ISlotGameManageReadService
    {
        List<JxBackendSelectListItem> GetProductSelectListItems(string defaultText);

        List<JxBackendSelectListItem> GetActionSelectListItems();

        BaseSlotGameManageParam GetManageParam(int no);

        PagedResultModel<SlotGameManageModel> GetPagedModel(SlotGameManageQueryParam param);
    }
}
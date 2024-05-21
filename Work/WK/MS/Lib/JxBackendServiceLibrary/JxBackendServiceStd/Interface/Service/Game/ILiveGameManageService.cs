using JxBackendService.Interface.Model.Param.Game;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface ILiveGameManageService
    {
        BaseReturnModel Create(LiveGameManageCreateParam createParam);

        BaseReturnModel Delete(int no);

        BaseReturnModel Update(LiveGameManageUpdateParam param);
    }

    public interface ILiveGameManageReadService
    {
        LiveGameManage GetDetail(int no);

        PagedResultModel<LiveGameManageModel> GetPagedModel(LiveGameManageQueryParam param);

        IEnumerable<LiveGameManage> GetAll();
        
        void SetDefaultValue(ILiveGameManageSetDefaultParam model);
    }
}
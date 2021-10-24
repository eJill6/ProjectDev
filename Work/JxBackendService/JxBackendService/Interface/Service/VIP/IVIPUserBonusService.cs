using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Model.ViewModel.VIP;

namespace JxBackendService.Interface.Service.VIP
{
    public interface IVIPUserBonusService
    {
        VIPUserBonusInitData GetBacksideWebInitData();

        PagedResultWithAdditionalData<VIPUserBonusModel, decimal> GetList(VIPUserBonusQueryParam param, BasePagingRequestParam pageParam);
    }
}
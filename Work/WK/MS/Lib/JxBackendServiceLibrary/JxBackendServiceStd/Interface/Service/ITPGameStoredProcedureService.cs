using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service
{
    public interface ITPGameStoredProcedureService
    {
        PagedResultModel<TPGameMoneyInfoViewModel> GetMoneyInfoList(PlatformProduct product, SearchTransferType searchTransferType, SearchTPGameMoneyInfoParam param);

        TPGameMoneyInfoViewModel GetMoneyInfo(PlatformProduct product, SearchTransferType searchTransferType, string moneyId);
    }
}
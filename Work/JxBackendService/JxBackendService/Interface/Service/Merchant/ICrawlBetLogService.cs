using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service.Merchant
{
    public interface ICrawlBetLogService
    {
        PlatformProduct Product { get; }
        BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogResult(string lastSearchToken);
    }
}
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.Security
{
    public interface IValidateCodeService
    {
        BaseReturnDataModel<string> GetGraphicValidateCodeImage(WebActionType webActionType, string userIdentityKey,
                                                                bool isForceRefresh = false);

        BaseReturnModel CheckGraphicValidateCode(WebActionType webActionType, string userIdentityKey, string validateCode);
    }
}


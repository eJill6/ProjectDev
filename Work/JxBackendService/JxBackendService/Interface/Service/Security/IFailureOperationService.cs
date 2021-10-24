using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.Security
{
    public interface IFailureOperationService
    {
        BaseReturnDataModel<bool?> AddFailOperation(WebActionType webActionType, SubActionType subActionType);
        BaseReturnDataModel<bool?> AddFailOperation(WebActionType webActionType, SubActionType subActionType, string webActionTypeNameParam);
        
        bool ClearCount(WebActionType webActionType, SubActionType subActionType);
    }
}

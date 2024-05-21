using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;

namespace JxBackendService.Interface.Service.SMS
{
    public interface ISendSMSManagerService : IEnvLoginUserService
    {
        BaseReturnDataModel<ServiceProviderInfo> SendSMS(SendUserSMSParam sendUserSMSParam);
    }
}
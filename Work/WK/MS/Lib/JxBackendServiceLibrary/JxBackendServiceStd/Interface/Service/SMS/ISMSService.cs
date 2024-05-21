using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.SMS
{
    public interface ISMSService
    {
        BaseReturnModel SendSMS(SendUserSMSParam sendUserSMSParam);
    }
}
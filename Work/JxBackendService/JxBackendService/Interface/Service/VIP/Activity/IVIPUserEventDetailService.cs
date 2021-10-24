using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.VIP.Activity
{
    public interface IVIPUserEventDetailService
    {
        BaseReturnModel VIPUserApplyForActivity();

        BaseReturnModel ProcessVIPUserEventAudit(BacksideEventAuditParam auditParam);
    }
}

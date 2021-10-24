using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Repository.VIP
{
    public interface IVIPUserEventDetailRep : IBaseDbRepository<VIPUserEventDetail>
    {
        VIPUserEventAuditStat GetVIPUserEventAuditStat(VIPUserEventQueryParam queryParam);

        bool HasAuditRefIDActivity(CheckUserEventRefIDParam refIDParam);

        bool HasAuditUnprocessedActivity(int userId, int eventTypeID);

        BaseReturnModel ProcessVIPUserEventAudit(ProcessEventAuditParam auditParam);
    }
}
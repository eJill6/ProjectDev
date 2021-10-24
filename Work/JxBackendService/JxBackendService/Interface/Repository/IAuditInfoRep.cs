using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IAuditInfoRep : IBaseDbRepository<AuditInfo>
    {
        bool CheckUnProcessAuditInfo(int auditType, string refID);
        bool CheckUnProcessAuditInfoIsExistData(int auditType, int userId, string checkAuditValue);
        BaseReturnModel Deal(AuditInfoDealParam param);
        PagedResultModel<AuditInfo> GetList(AuditInfoQueryParam param, BasePagingRequestParam pageParam);
    }
}

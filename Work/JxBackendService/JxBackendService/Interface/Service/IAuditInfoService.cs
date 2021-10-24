using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IAuditInfoService
    {
        BaseReturnModel CreateAuditInfo(AuditInfoParam param);
        BaseReturnModel Deal(string id, int auditStatus, int auditorUserId, string auditorUserName, string memo = "");
        PagedResultModel<AuditInfo> GetList(AuditInfoQueryParam param, BasePagingRequestParam pageParam);
    }
}

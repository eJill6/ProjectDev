using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;

namespace JxBackendService.Interface.Repository.BackSideUser
{
    public interface IBWOperationLogRep : IBaseDbRepository<BWOperationLog>
    {
        PagedResultModel<BWOperationLog> GetPagedOperationLog(QueryBWOperationLogParam queryParam);
    }
}
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;

namespace JxBackendService.Interface.Repository.BackSideUser
{
    public interface IBWOperationLogRep : IBaseDbRepository<BWOperationLog>
    {
        PagedResultModel<BWOperationLog> GetPagedOperationLog(QueryBWOperationLogParam queryParam);

        /// <summary>
        /// 根据日志ID查询
        /// </summary>
        /// <param name="OperationID"></param>
        /// <returns></returns>
        BWOperationLog GetOperationLogById(int OperationID);
    }
}
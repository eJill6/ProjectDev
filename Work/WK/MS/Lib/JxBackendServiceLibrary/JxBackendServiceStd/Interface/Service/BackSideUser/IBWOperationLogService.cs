using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.SystemSetting;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWOperationLogService
    {
        BaseReturnModel CreateOperationLog(CreateBWOperationLogParam createParam);

        BaseReturnModel CreateOperationLog(CreateBWOperationLogByTypeParam createParam);
    }

    public interface IBWOperationLogReadService
    {
        PagedResultModel<OperationLogViewModel> GetPagedBWOperationLogs(QueryBWOperationLogParam createParam);

        OperationLogViewModel GetOperationLogById(int OperationID);
    }
}
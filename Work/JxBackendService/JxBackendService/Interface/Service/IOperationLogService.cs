using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service
{
    public interface IOperationLogService
    {
        BaseReturnDataModel<long> InsertModifyMemberOperationLog(InsertModifyMemberOperationLogParam param);

        BaseReturnDataModel<long> InsertModifySystemOperationLog(InsertModifySystemOperationLogParam param);

        BaseReturnDataModel<long> InsertFrontSideOperationLog(InsertFrontSideOperationLogParam param);

        BaseReturnDataModel<long> InsertFrontSideOperationLogWithUserLoginDetails(InsertFrontSideOperationLogParam param);
        
        BaseReturnDataModel<long> InsertFrontSideOperationLog(JxOperationLogCategory jxOperationLogCategory, InsertFrontSideOperationLogParam param);
        
        BaseReturnDataModel<long> InsertFrontSideOperationLogWithUserLoginDetails(JxOperationLogCategory jxOperationLogCategory, InsertFrontSideOperationLogParam param);
    }
}

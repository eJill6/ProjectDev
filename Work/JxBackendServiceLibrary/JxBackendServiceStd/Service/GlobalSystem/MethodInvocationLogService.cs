using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Repository.GlobalSystem;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Entity.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.GlobalSystem
{
    public class MethodInvocationLogService : BaseService, IMethodInvocationLogService
    {
        private readonly IMethodInvocationLogRep _methodInvocationLogRep;

        public MethodInvocationLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _methodInvocationLogRep = ResolveJxBackendService<IMethodInvocationLogRep>();
        }

        public BaseReturnDataModel<long> Create(IInsertMethodInvocationLogParam param)
        {
            string seqId = _methodInvocationLogRep.CreateSEQID();

            var methodInvocationLog = new MethodInvocationLog()
            {
                SEQID = seqId,
                ArgumentsJson = param.Arguments.ToJsonString(),
                CorrelationId = param.CorrelationId,
                ElapsedMilliseconds = param.ElapsedMilliseconds,
                ErrorMsg = param.ErrorMsg,
                MethodName = param.MethodName,
                ReturnValueJson = param.ReturnValue.ToJsonString(),
                UserID = param.UserID
            };

            return _methodInvocationLogRep.CreateByProcedure(methodInvocationLog);
        }
    }
}
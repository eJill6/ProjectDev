using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.GlobalSystem;
using JxBackendService.Interface.Repository.GlobalSystem;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Model.Entity.GlobalSystem;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.GlobalSystem;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.GlobalSystem
{
    public class MethodInvocationLogService : BaseService, IMethodInvocationLogService, IMethodInvocationLogReadService
    {
        private readonly Lazy<IMethodInvocationLogRep> _methodInvocationLogRep;

        private static readonly int s_maxInsertCount = 300;

        private static readonly ConcurrentQueue<IInsertMethodInvocationLogParam> s_logParamQueue = new ConcurrentQueue<IInsertMethodInvocationLogParam>();

        private static readonly int s_jobIntervalMilliseconds = 5000;

        private static readonly int s_jobContinueMilliseconds = 300;

        static MethodInvocationLogService()
        {
            IEnvironmentService environmentService = DependencyUtil.ResolveService<IEnvironmentService>().Value;
            var environmentUser = new EnvironmentUser()
            {
                Application = environmentService.Application,
            };

            Task.Run(() =>
            {
                while (true)
                {
                    int delayMilliseconds = s_jobIntervalMilliseconds;

                    ErrorMsgUtil.DoWorkWithErrorHandle(
                        environmentUser,
                        () =>
                        {
                            var insertParams = new List<IInsertMethodInvocationLogParam>();

                            while (s_logParamQueue.Any())
                            {
                                if (s_logParamQueue.TryDequeue(out IInsertMethodInvocationLogParam param))
                                {
                                    insertParams.Add(param);
                                }

                                if (insertParams.Count >= s_maxInsertCount)
                                {
                                    //queue可能還有資料,縮短等待時間
                                    delayMilliseconds = s_jobContinueMilliseconds;

                                    break;
                                }
                            }

                            if (insertParams.Any())
                            {
                                var methodInvocationLogService = DependencyUtil.ResolveJxBackendService<IMethodInvocationLogService>(
                                 environmentUser,
                                 DbConnectionTypes.Master).Value;

                                methodInvocationLogService.BatchInsertLogs(insertParams);
                            }
                        });

                    TaskUtil.DelayAndWait(delayMilliseconds);
                }
            });
        }

        public MethodInvocationLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _methodInvocationLogRep = ResolveJxBackendService<IMethodInvocationLogRep>();
        }

        public void Enqueue(IInsertMethodInvocationLogParam insertMethodInvocationLogParam)
        {
            s_logParamQueue.Enqueue(insertMethodInvocationLogParam);
        }

        public BaseReturnModel BatchInsertLogs(List<IInsertMethodInvocationLogParam> logParams)
        {
            List<BaseMethodInvocationLog> dbLogContents = logParams
                .Select(s => new BaseMethodInvocationLog()
                {
                    CreateUser = s.UserID.ToString(),
                    CreateDate = s.CreateDate,
                    CorrelationId = s.CorrelationId,
                    UserID = s.UserID,
                    MethodName = s.MethodName,
                    ElapsedMilliseconds = s.ElapsedMilliseconds,
                    ArgumentsJson = s.Arguments.ToJsonString(),
                    ReturnValueJson = s.ReturnValue.ToJsonString(),
                    ErrorMsg = s.ErrorMsg,
                    TypeName = s.TypeName,
                })
                .OrderBy(o => o.CreateDate)
                .ToList();

            var addParam = new ProAddMultipleMethodInvocationLogParam
            {
                BulkAddMethodInvocationLogJson = dbLogContents.ToJsonString(),
            };

            return _methodInvocationLogRep.Value.AddMultipleMethodInvocationLog(addParam);
        }
    }
}
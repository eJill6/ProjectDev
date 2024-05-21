using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.ViewModel;
using System;

namespace JxBackendService.Service.DelayJob
{
    public class MQRetryDoReceiveDelayJobService : BaseDelayJobService<MQRetryDoReceiveJobParam>, IMQRetryDoReceiveDelayJobService
    {
        private static readonly int s_maxRetryCount = 2;

        private static readonly int s_retryIntervalSeconds = 5;

        public MQRetryDoReceiveDelayJobService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public void AddDelayJobParam(MQRetryDoReceiveJobParam param)
        {
            AddDelayJobParam(param, s_retryIntervalSeconds);
        }

        protected override void DoJob(MQRetryDoReceiveJobParam param)
        {
            string errorMessage = null;

            try
            {
                bool isSuccess = param.DoJobAfterReceived.Invoke(param.DoDequeueJobAfterReceivedParam);

                if (isSuccess)
                {
                    return;
                }

                errorMessage = "DoJobAfterReceived return false";
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (param.RetryCount < s_maxRetryCount)
            {
                param.RetryCount++;
                AddDelayJobParam(param, s_retryIntervalSeconds);
            }
            else
            {
                LogUtilService.Error($"{GetType().Name} Error:{param.TaskQueueName.Value}, {param.DoDequeueJobAfterReceivedParam.Message}, {errorMessage}");
            }
        }
    }
}
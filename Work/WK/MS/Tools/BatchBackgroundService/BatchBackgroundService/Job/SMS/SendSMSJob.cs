using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ServiceModel;

namespace BatchService.Job.SMS
{
    public class SendSMSJob : BaseTaskJob, ITaskJob
    {
        private readonly int _workerCount = 3;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        private readonly Lazy<ISendSMSManagerService> _sendSMSManagerService;

        public SendSMSJob()
        {
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
            _sendSMSManagerService = DependencyUtil.ResolveEnvLoginUserService<ISendSMSManagerService>(EnvUser);
        }

        protected override void DoWork(CancellationToken cancellationToken)
        {
            for (int i = 1; i <= _workerCount; i++)
            {
                _internalMessageQueueService.Value.StartNewDequeueJob(TaskQueueName.SendSMS, DoJobAfterReceived);
            }
        }

        private bool DoJobAfterReceived(DoDequeueJobAfterReceivedParam doDequeueJobAfterReceivedParam)
        {
            SendUserSMSParam sendUserSMSParam;

            try
            {
                sendUserSMSParam = doDequeueJobAfterReceivedParam.Message.Deserialize<SendUserSMSParam>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"{GetType().Name} DoJobAfterReceived:{doDequeueJobAfterReceivedParam.Message}");
                ex.ErrorHandle(EnvUser);

                return true;
            }

            BaseReturnDataModel<ServiceProviderInfo> baseReturnDataModel = _sendSMSManagerService.Value.SendSMS(sendUserSMSParam);

            if (!baseReturnDataModel.IsSuccess)
            {
                JobErrorHandle($"簡訊發送失敗,Message={baseReturnDataModel.Message},param={sendUserSMSParam.ToJsonString()}");
            }

            return true;
        }
    }
}
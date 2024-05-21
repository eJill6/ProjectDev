using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ViewModel;
using System;

namespace JxBackendService.Service.DelayJob
{
    public class MessageQueueDelayJobService : BaseDelayJobService<MessageQueueParam>, IMessageQueueDelayJobService
    {
        private readonly Lazy<IMessageQueueService> _messageQueueService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        public MessageQueueDelayJobService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>();
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
        }

        protected override void DoJob(MessageQueueParam param)
        {
            if (param is BWUserLogoutMessage)
            {
                _messageQueueService.Value.SendBackSideWebUserLogoutMessage(param as BWUserLogoutMessage);
            }
            else if (param is BWUserChangePasswordMessage)
            {
                _messageQueueService.Value.SendBackSideWebUserChangePasswordMessage(param as BWUserChangePasswordMessage);
            }
            else if (param is UpdateTPGameUserScoreParam)
            {
                _internalMessageQueueService.Value.EnqueueUpdateTPGameUserScoreMessage(param as UpdateTPGameUserScoreParam);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
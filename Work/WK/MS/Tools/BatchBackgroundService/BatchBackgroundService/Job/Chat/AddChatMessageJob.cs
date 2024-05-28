using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxMsgEntities;

namespace BatchService.Job.Chat
{
    public class AddChatMessageJob : BaseTaskJob, ITaskJob
    {
        private static readonly int s_workerCount = 50;

        private static readonly int s_maxNotificationMessageLength = 20;

        private readonly Lazy<IMessageQueueService> _messageQueueService;
        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;
        private readonly Lazy<IChatMessageService> _chatMessageService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public AddChatMessageJob()
        {
            _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>();
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
            _chatMessageService = DependencyUtil.ResolveEnvLoginUserService<IChatMessageService>(EnvUser);
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        protected override void DoWork(CancellationToken cancellationToken)
        {
            for (int i = 1; i <= s_workerCount; i++)
            {
                _internalMessageQueueService.Value.StartNewDequeueJob(TaskQueueName.AddChatMessage, DoJobAfterReceived);
            }
        }

        private bool DoJobAfterReceived(DoDequeueJobAfterReceivedParam doDequeueJobAfterReceivedParam)
        {
            SendAddMessageQueueParam sendAddMessageQueueParam;

            try
            {
                sendAddMessageQueueParam = doDequeueJobAfterReceivedParam.Message.Deserialize<SendAddMessageQueueParam>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"{GetType().Name} DoJobAfterReceived:{doDequeueJobAfterReceivedParam.Message}");
                ex.ErrorHandle(EnvUser);

                return true;
            }

            object lastMessageInfolock = _jxCacheService.Value.GetCache(
                new SearchCacheParam
                {
                    Key = CacheKey.LastMessageInfokey(sendAddMessageQueueParam.OwnerUserID, sendAddMessageQueueParam.PublishUserID),
                    CacheSeconds = 60,
                    IsCloneInstance = false,
                },
                () => new object());

            lock (lastMessageInfolock)
            {
                BaseReturnModel baseReturnModel = _chatMessageService.Value.AddChatMessage(sendAddMessageQueueParam);

                if (!baseReturnModel.IsSuccess)
                {
                    JobErrorHandle(baseReturnModel.Message);

                    return true;
                }
            }

            //這邊不再另外做notification的background service, 直接發推播
            var newMessageNotificationParam = new ChatNotificationParam()
            {
                RoutingKey = sendAddMessageQueueParam.RoomID,
                ChatNotificationInfo = new ChatNotificationInfo()
                {
                    ChatNotificationType = ChatNotificationTypes.NewMessage,
                    RoomID = sendAddMessageQueueParam.OwnerUserID.ToString(), //1 on 1的時候要使用發訊息的用戶ID
                    MessageID = sendAddMessageQueueParam.MessageID,
                    PublishTimestamp = sendAddMessageQueueParam.PublishTimestamp,
                    Message = sendAddMessageQueueParam.Message.ToShortString(s_maxNotificationMessageLength),
                }
            };

            SendResult sendResult = _messageQueueService.Value.SendChatNotification(newMessageNotificationParam);

            if (!sendResult.IsSuccess)
            {
                JobErrorHandle(sendResult.Message);

                return true;
            }

            return true;
        }
    }
}
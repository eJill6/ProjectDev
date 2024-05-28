using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Param.Chat;
using JxMsgEntities;

namespace BatchService.Job.Chat
{
    public class DeleteChatMessageJob : BaseTaskJob, ITaskJob
    {
        private readonly int _workerCount = 10;

        private readonly Lazy<IMessageQueueService> _messageQueueService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        private readonly Lazy<IOneOnOneChatService> _oneOnOneChatService;

        private readonly Lazy<IOneOnOneChatReadService> _oneOnOneChatReadService;

        private static readonly int s_deleteQuantity = 300;

        private static readonly int s_fetchCount = 100000;

        private static readonly int s_deleteMessageWaitMilliseconds = 300;

        public DeleteChatMessageJob()
        {
            _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>();
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
            _oneOnOneChatReadService = DependencyUtil.ResolveJxBackendService<IOneOnOneChatReadService>(EnvUser, DbConnectionTypes.IMsgSlave);
            _oneOnOneChatService = DependencyUtil.ResolveJxBackendService<IOneOnOneChatService>(EnvUser, DbConnectionTypes.IMsgMaster);
        }

        protected override void DoWork(CancellationToken cancellationToken)
        {
            for (int i = 1; i <= _workerCount; i++)
            {
                _internalMessageQueueService.Value.StartNewDequeueJob(TaskQueueName.DeleteChatMessage, DoJobAfterReceived);
            }
        }

        private bool DoJobAfterReceived(DoDequeueJobAfterReceivedParam doDequeueJobAfterReceivedParam)
        {
            DeleteMessageQueueParam deleteMessageQueueParam;

            try
            {
                deleteMessageQueueParam = doDequeueJobAfterReceivedParam.Message.Deserialize<DeleteMessageQueueParam>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"{GetType().Name} DoJobAfterReceived:{doDequeueJobAfterReceivedParam.Message}");
                ex.ErrorHandle(EnvUser);

                return true;
            }

            var queryLastMessagesParam = new QueryLastMessagesParam
            {
                OwnerUserID = deleteMessageQueueParam.OwnerUserID,
                RoomID = deleteMessageQueueParam.RoomID,
                PublishTimestamp = deleteMessageQueueParam.SmallEqualThanTimestamp
            };

            MSIMLastMessageInfo? lastMessageInfo = _oneOnOneChatReadService.Value.GetMSIMLastMessageInfos(queryLastMessagesParam, int.MaxValue).SingleOrDefault();

            if (lastMessageInfo == null)
            {
                return true;
            }

            // 一對一砍聊天訊息
            var queryOneOnOneMessageParam = new QueryOneOnOneMessageParam
            {
                BothChatUserIDParam = new BothChatUserIDParam
                {
                    OwnerUserID = lastMessageInfo.OwnerUserID,
                    DialogueUserID = lastMessageInfo.RoomID.ToInt32(),
                },
                PublishTimestamp = lastMessageInfo.PublishTimestamp
            };

            List<MSIMLastMessageKey> lastMessageKeys = _oneOnOneChatReadService.Value.GetLastMessageKeys(new List<long> { lastMessageInfo.MessageID });

            while (lastMessageKeys.Any())
            {
                _oneOnOneChatService.Value.DeleteLastMessages(lastMessageKeys.Take(s_deleteQuantity).ToList());

                lastMessageKeys.RemoveRangeByFit(0, s_deleteQuantity);

                TaskUtil.DelayAndWait(s_deleteMessageWaitMilliseconds);
            }

            while (true)
            {
                List<MSIMOneOnOneChatMessageKey> oneOnOneChatMessageKeys = _oneOnOneChatReadService.Value.GetOneOnOneChatMessageKeys(queryOneOnOneMessageParam, s_fetchCount);

                if (!oneOnOneChatMessageKeys.Any())
                {
                    break;
                }

                while (oneOnOneChatMessageKeys.Any())
                {
                    _oneOnOneChatService.Value.DeleteChatMessages(oneOnOneChatMessageKeys.Take(s_deleteQuantity).ToList());

                    oneOnOneChatMessageKeys.RemoveRangeByFit(0, s_deleteQuantity);

                    TaskUtil.DelayAndWait(s_deleteMessageWaitMilliseconds);
                }
            }

            // 若雙方用戶都在線上時，就需要通知前端畫面砍相關訊息
            DeleteMessageNotification(lastMessageInfo.OwnerUserID.ToString(), lastMessageInfo.RoomID.ToString(), lastMessageInfo.MessageID);
            DeleteMessageNotification(lastMessageInfo.RoomID, lastMessageInfo.OwnerUserID.ToString(), lastMessageInfo.MessageID);

            return true;
        }

        private void DeleteMessageNotification(string routingKey, string roomID, long messageID)
        {
            //這邊不再另外做notification的background service, 直接發推播
            var deleteMessageNotificationParam = new ChatNotificationParam()
            {
                RoutingKey = routingKey,
                ChatNotificationInfo = new ChatNotificationInfo()
                {
                    ChatNotificationType = ChatNotificationTypes.DeleteChat,
                    RoomID = roomID, //1 on 1的時候要使用發訊息的用戶ID
                    MessageID = messageID
                }
            };

            SendResult sendResult = _messageQueueService.Value.SendChatNotification(deleteMessageNotificationParam);

            if (!sendResult.IsSuccess)
            {
                JobErrorHandle(sendResult.Message);
            }
        }
    }
}
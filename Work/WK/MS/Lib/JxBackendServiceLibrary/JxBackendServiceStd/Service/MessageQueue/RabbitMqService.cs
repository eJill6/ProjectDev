using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxMsgEntities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JxBackendService.Service.MessageQueue
{
    public class RabbitMqService : BaseRabbitMqService, IMessageQueueService
    {
        private static readonly ConcurrentDictionary<string, RabbitMQClientManager> s_rabbitMQClientManagerMap;

        static RabbitMqService()
        {
            var endUserRabbitMqInitService = DependencyUtil.ResolveService<IEndUserRabbitMqInitService>().Value;
            s_rabbitMQClientManagerMap = endUserRabbitMqInitService.Init();
        }

        public RabbitMqService()
        {
        }

        protected override ConcurrentDictionary<string, RabbitMQClientManager> GetRabbitMQClientManagerMap()
        {
            return s_rabbitMQClientManagerMap;
        }

        public SendResult SendBackSideWebTransferMessage(string routingKey, TransferMessage transferMessage)
        {
            return SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.TransferNotice,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_BACKSIDEWEB_EXCHANGE,
                SendRoutKey = routingKey,
                SendContent = transferMessage
            });
        }

        public SendResult SendBackSideWebUserLogoutMessage(BWUserLogoutMessage bwUserLogoutMessage)
        {
            var messageEntity = new MessageEntity
            {
                MessageType = RabbitMessageType.JXManagement,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_BACKSIDEWEB_EXCHANGE,
                SendRoutKey = bwUserLogoutMessage.UserID.ToString(),
                SendContent = new BackSideWebUserMessage()
                {
                    BackSideWebUserActionType = Model.Enums.BackSideWeb.Common.BackSideWebUserActionTypes.Logout,
                    Message = MessageElement.BWUserInfoChangedAndLoginAgain
                }
            };

            return SendMessage(messageEntity);
        }

        public SendResult SendBackSideWebUserChangePasswordMessage(BWUserChangePasswordMessage bwUserLogoutMessage)
        {
            var messageEntity = new MessageEntity
            {
                MessageType = RabbitMessageType.JXManagement,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_BACKSIDEWEB_EXCHANGE,
                SendRoutKey = bwUserLogoutMessage.UserID.ToString(),
                SendContent = new BackSideWebUserMessage()
                {
                    BackSideWebUserActionType = Model.Enums.BackSideWeb.Common.BackSideWebUserActionTypes.ChangePassword,
                    Message = MessageElement.PleaseChangePasswordBeforeOperate
                }
            };

            return SendMessage(messageEntity);
        }

        public SendResult SendChatNotification(ChatNotificationParam newMessageNotificationParam)
        {
            var messageEntity = new MessageEntity
            {
                MessageType = RabbitMessageType.Chat,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_MISELIVE_CHAT_EXCHANGE,
                SendRoutKey = newMessageNotificationParam.RoutingKey,
                SendContent = newMessageNotificationParam.ChatNotificationInfo
            };

            return SendMessage(messageEntity);
        }

        private SendResult SendMessage(MessageEntity messageEntity) => SendMessage(GetRabbitMQClientManagerMap().Values, messageEntity);

        /// <summary>針對單一RabbitMQClientManager發送訊息</summary>
        private SendResult SendMessage(string clientProvidedName, MessageEntity messageEntity)
        {
            ConcurrentDictionary<string, RabbitMQClientManager> managerMap = GetRabbitMQClientManagerMap();

            if (!managerMap.TryGetValue(clientProvidedName, out RabbitMQClientManager rabbitMQClientManager))
            {
                return new SendResult()
                {
                    IsSuccess = false,
                    Message = $"{clientProvidedName} not found"
                };
            }

            return SendMessage(new RabbitMQClientManager[] { rabbitMQClientManager }, messageEntity);
        }

        private SendResult SendMessage(ICollection<RabbitMQClientManager> rabbitMQClientManagers, MessageEntity messageEntity)
        {
            SendResult sendResult = null;
            EnvironmentUser environmentUser = CreateEnvUser();

            //因為不知道用戶監聽哪一台，所以發送給用戶的時候需要全部發送
            foreach (RabbitMQClientManager rabbitMQClientManager in rabbitMQClientManagers)
            {
                SendResult currentSendResult = ErrorMsgUtil.DoWorkWithErrorHandle(
                    environmentUser,
                    () =>
                    {
                        if (!rabbitMQClientManager.IsOpen)
                        {
                            return null;
                        }

                        return rabbitMQClientManager.SendMessage(messageEntity);
                    });

                if (sendResult == null)
                {
                    sendResult = currentSendResult;
                }
                else if (!currentSendResult.IsSuccess)
                {
                    //讓沒有成功的結果往回傳
                    sendResult = currentSendResult;
                }
            }

            return sendResult;
        }
    }
}
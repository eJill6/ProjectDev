using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Resource.Element;
using JxMsgEntities;
using RabbitMqUtil;
using System;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class RabbitMqService : IMessageQueueService
    {
        private static readonly string _refreshUserInfoCode = "refreshuserinfo";
        private readonly CommonRabbitmqManager _commonRabbitmqManager;

        public RabbitMqService(JxApplication jxApplication)
        {
            var appSettingService = DependencyUtil.ResolveKeyedForModel<IAppSettingService>(jxApplication, SharedAppSettings.PlatformMerchant);
            RabbitMQSetting rabbitMQSetting = appSettingService.GetRabbitMQSetting();
            _commonRabbitmqManager = new CommonRabbitmqManager(rabbitMQSetting.HostName, rabbitMQSetting.Port, rabbitMQSetting.UserName, rabbitMQSetting.Password);
        }

        public void SendRefreshUserInfoMessage(int userId)
        {
            SendTransferMessage(userId, 0, _refreshUserInfoCode);
        }

        public void SendTransferMessage(int userId, decimal amount, string summary)
        {
            SendTransferMessage(userId, amount, summary, 0);
        }

        public void SendTransferMessage(int userId, decimal amount, string summary, int delaySendSecond)
        {
            if (delaySendSecond > 0)
            {
                Thread.Sleep(delaySendSecond * 1000);
            }

            var noticeEntity = new TransferNoticeEntity()
            {
                UserId = userId.ToString(),
                Amount = amount,
                Summary = summary
            };

            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.TransferNotice,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_WCF_EXCHANGE,
                SendRoutKey = RQSettings.RQ_WCF_ROUTKEY,
                SendContent = noticeEntity
            });
        }

        /// <summary>
        /// [容舊搬移]前台站內信在做任何動作的時候都會發這則MQ，先搬到這裡來
        /// </summary>
        /// <param name="childUserId"></param>
        public void SendUpdateLettersGroupMessage(int childUserId)
        {
            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.UpdateLettersGroup,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_DIRECT_MESSAGE,
                SendRoutKey = childUserId.ToString(),
                SendContent = string.Empty
            });
        }

        public void SendGroupChatRoomDetailMessage(int chatRoomId, string messageContent, int userId, string userName, int avatarId)
        {
            var noticeEntity = new GroupChatRoomDetailNoticeEntity()
            {
                BelongGroupId = chatRoomId,
                MessageContent = messageContent,
                PublishUserId = userId,
                PublishUserName = userName,
                PublishUserAvatarId = avatarId
            };

            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_GROUPS_MESSAGE,
                SendRoutKey = chatRoomId.ToString(),
                SendContent = noticeEntity
            });
        }

        public void SendPublishGroupMessage(int belongGroupId, object obj)
        {
            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_GROUPS_MESSAGE,
                SendRoutKey = belongGroupId.ToString(),
                SendContent = obj
            });
        }

        public void SendGroupChatRoomActionControlMessage(int actionType, int chatRoomId, string chatRoomName, int userId)
        {
            var noticeEntity = new BaseGroupChatRoomActionControlNoticeEntity()
            {
                ActionType = actionType,
                GroupId = chatRoomId,
                GroupName = chatRoomName
            };

            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_GROUPS_ACTION,
                SendRoutKey = userId.ToString(),
                SendContent = noticeEntity
            });
        }

        public void SendGroupChatRoomUpdatedSendPermissionMessage(int chatRoomId, string chatRoomName, int userId, bool enableSendMessage)
        {
            var noticeEntity = new GroupChatRoomActionControlSendPermissionNoticeEntity()
            {
                ActionType = GroupChatRoomActionControlTypes.BeUpdatedSendMessagePermission.Value,
                GroupId = chatRoomId,
                GroupName = chatRoomName,
                EnableSendMessage = enableSendMessage
            };

            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_GROUPS_ACTION,
                SendRoutKey = userId.ToString(),
                SendContent = noticeEntity
            });
        }

        public void SendGroupChatRoomBeJoinedMessage(int chatRoomId, string chatRoomName, int userId, int totalMemberCount)
        {
            var noticeEntity = new GroupChatRoomActionControlBeJoinedNoticeEntity()
            {
                ActionType = GroupChatRoomActionControlTypes.BeJoinedNewChatRoom.Value,
                GroupId = chatRoomId,
                GroupName = chatRoomName,
                TotalMemberCount = totalMemberCount
            };

            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_GROUPS_ACTION,
                SendRoutKey = userId.ToString(),
                SendContent = noticeEntity
            });
        }

        public void SendTransferToChildMessage(int childUserId, decimal transferAmount)
        {
            var noticeEntity = new TransferNoticeEntity()
            {
                UserId = childUserId.ToString(),
                Amount = transferAmount,
                Summary = string.Format(MessageElement.YourParentTransferMoneyToYou, transferAmount)            
            };

            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.TransferNotice,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_WCF_EXCHANGE,
                SendRoutKey = RQSettings.RQ_WCF_ROUTKEY,
                SendContent = noticeEntity
            });
        }

        public void SendPubStationLetterMessageNoExpire(int rcUserId, object obj)
        {
            _commonRabbitmqManager.SendMessageNoExpir(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_DIRECT_MESSAGE,
                SendRoutKey = rcUserId.ToString(),
                SendContent = obj
            });
        }

        public void SendPublishSingleMessage(int rcUserId, object obj)
        {
            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.Receive,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_DIRECT_MESSAGE,
                SendRoutKey = rcUserId.ToString(),
                SendContent = obj
            });
        }

        public void UpdateUserActiveStatus(int userId)
        {
            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.LeaveWhisper,
                SendType = RQSettings.RQ_DIRECT_TYPE,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_WCF_EXCHANGE,
                SendRoutKey = RQSettings.RQ_WCF_ROUTKEY,
                SendContent = userId.ToString() + "#@%6$"
            });
        }

        public void SendWithDrawMessage(object obj)
        {
            _commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.NEWS,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_HEC_LOTTERY_INDRAWTIP,
                SendRoutKey = string.Empty,
                SendContent = obj
            });
        }
    }
}

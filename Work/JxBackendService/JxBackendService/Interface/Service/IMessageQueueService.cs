using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IMessageQueueService
    {
        void SendRefreshUserInfoMessage(int userId);
        
        void SendTransferMessage(int userId, decimal amount, string summary);
        
        void SendTransferMessage(int userId, decimal amount, string summary, int delaySendSecond);

        void SendUpdateLettersGroupMessage(int childUserId);

        void SendGroupChatRoomDetailMessage(int chatRoomId, string messageContent, int userId, string userName, int avatarId);

        void SendGroupChatRoomActionControlMessage(int actionType, int chatRoomId, string chatRoomName, int userId);

        void SendGroupChatRoomUpdatedSendPermissionMessage(int chatRoomId, string chatRoomName, int userId, bool enableSendMessage);

        void SendGroupChatRoomBeJoinedMessage(int chatRoomId, string chatRoomName, int userId, int totalMemberCount);
        
        void SendTransferToChildMessage(int childUserId, decimal transferAmount);

        void SendPubStationLetterMessageNoExpire(int rcUserId, object obj);
        
        void SendPublishGroupMessage(int belongGroupId, object obj);
        
        void SendPublishSingleMessage(int rcUserId, object obj);
        
        void UpdateUserActiveStatus(int userId);

        void SendWithDrawMessage(object obj);
    }
}

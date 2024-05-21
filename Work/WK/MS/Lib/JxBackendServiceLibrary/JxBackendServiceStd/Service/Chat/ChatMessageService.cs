using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Chat;
using JxBackendService.Interface.Repository.MM;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Entity.MM;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Chat;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Chat;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Chat
{
    public class ChatMessageService : BaseEnvLoginUserService, IChatMessageService, IChatMessageReadService
    {
        private static readonly int s_fetchCount = 50;

        private static readonly int s_lastMessageMaxLength = 22;

        private readonly Lazy<IOneOnOneChatService> _msimOneOnOneChatMessageService;

        private readonly Lazy<IOneOnOneChatReadService> _msimOneOnOneChatMessageReadService;

        private readonly Lazy<IMSIMLastMessageInfoRep> _msimLastMessageInfoReadRep;

        private readonly Lazy<IMSIMLastMessageInfoRep> _msimLastMessageInfoRep;

        private readonly Lazy<IMMUserInfoRep> _mmUserInfoReadRep;

        public ChatMessageService(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
            _msimOneOnOneChatMessageService = ResolveJxBackendService<IOneOnOneChatService>(DbConnectionTypes.IMsgMaster);
            _msimOneOnOneChatMessageReadService = ResolveJxBackendService<IOneOnOneChatReadService>(DbConnectionTypes.IMsgSlave);
            _msimLastMessageInfoReadRep = ResolveJxBackendService<IMSIMLastMessageInfoRep>(DbConnectionTypes.IMsgSlave);
            _msimLastMessageInfoRep = ResolveJxBackendService<IMSIMLastMessageInfoRep>(DbConnectionTypes.IMsgMaster);
            _mmUserInfoReadRep = ResolveJxBackendService<IMMUserInfoRep>(DbConnectionTypes.MimiSlave);
        }

        public BaseReturnModel AddChatMessage(SendAddMessageQueueParam param)
        {
            var msimOneOnOneChatMessage = new MSIMOneOnOneChatMessage()
            {
                OwnerUserID = param.OwnerUserID,
                DialogueUserID = param.RoomID.ToInt32(),
                PublishUserID = param.PublishUserID,
                MessageID = param.MessageID,
                MessageType = param.MessageTypeValue,
                Message = param.Message,
                PublishTimestamp = param.PublishTimestamp
            };

            return _msimOneOnOneChatMessageService.Value.AddChatMessage(msimOneOnOneChatMessage);
        }

        public BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryMessageParam)
        {
            //這邊未來也許會有Group Message
            BaseReturnDataModel<List<ChatMessageViewModel>> returnDataModel = _msimOneOnOneChatMessageReadService
                .Value
                .GetRoomMessages(queryMessageParam, s_fetchCount);

            return returnDataModel;
        }

        public BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageViewModels(QueryLastMessagesParam queryLastMessagesParam)
        {
            List<MSIMLastMessageInfo> msimLastMessageInfos = _msimOneOnOneChatMessageReadService
                .Value
                .GetMSIMLastMessageInfos(queryLastMessagesParam, s_fetchCount);

            //這邊目前只有一對一聊天，所以一定是UserID
            List<int> userIds = msimLastMessageInfos.Select(s => s.RoomID.ToInt32()).ToList();
            Dictionary<int, BasicMMUserInfo> mmUserInfoMap = _mmUserInfoReadRep.Value.GetBasicMMUserInfos(userIds).ToDictionary(d => d.UserId);

            List<LastMessageViewModel> lastMessageViewModels = msimLastMessageInfos.Select(s =>
            {
                string roomName = null;
                string avatarUrl = null;

                if (mmUserInfoMap.TryGetValue(s.RoomID.ToInt32(), out BasicMMUserInfo basicMMUserInfo))
                {
                    roomName = basicMMUserInfo.Nickname;
                    avatarUrl = basicMMUserInfo.AvatarUrl;
                }

                var lastMessageViewModel = new LastMessageViewModel()
                {
                    RoomID = s.RoomID,
                    RoomName = roomName,
                    AvatarUrl = avatarUrl,
                    MessageID = s.MessageID,
                    MessageType = s.MessageType,
                    Message = s.Message,
                    PublishTimestamp = s.PublishTimestamp,
                    UnreadCount = s.UnreadCount,
                };

                if (lastMessageViewModel.MessageType == MessageType.Text)
                {
                    lastMessageViewModel.Message = lastMessageViewModel.Message.ToShortString(s_lastMessageMaxLength);
                }

                return lastMessageViewModel;
            }).ToList();

            return new BaseReturnDataModel<List<LastMessageViewModel>>(ReturnCode.Success, lastMessageViewModels);
        }

        public BaseReturnModel ClearUnreadCount(RoomParam roomParam)
        {
            return _msimLastMessageInfoRep.Value.ClearUnreadCount(EnvLoginUser.LoginUser.UserId, roomParam.RoomID);
        }

        public BaseReturnDataModel<bool> HasUnreadMessage()
        {
            bool hasUnreadMessage = _msimLastMessageInfoReadRep.Value.HasUnreadMessage(EnvLoginUser.LoginUser.UserId);

            return new BaseReturnDataModel<bool>(ReturnCode.Success, hasUnreadMessage);
        }
    }
}
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Chat;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Chat;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Chat;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Chat
{
    public class OneOnOneChatService : BaseService, IOneOnOneChatService, IOneOnOneChatReadService
    {
        private readonly Lazy<IMSIMOneOnOneChatMessageRep> _msimOneOnOneChatMessageRep;

        private readonly Lazy<IMSIMLastMessageInfoRep> _msimLastMessageInfoRep;

        public OneOnOneChatService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _msimOneOnOneChatMessageRep = ResolveJxBackendService<IMSIMOneOnOneChatMessageRep>();
            _msimLastMessageInfoRep = ResolveJxBackendService<IMSIMLastMessageInfoRep>();
        }

        public BaseReturnModel AddChatMessage(MSIMOneOnOneChatMessage msimOneOnOneChatMessage)
        {
            var proSaveOneOnOneChatMessageParam = msimOneOnOneChatMessage.CastByJson<ProSaveOneOnOneChatMessageParam>();

            return _msimOneOnOneChatMessageRep.Value.SaveOneOnOneChatMessage(proSaveOneOnOneChatMessageParam);
        }

        public BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryMessageParam, int fetchCount)
        {
            List<MSIMOneOnOneChatMessage> msimOneOnOneChatMessages = _msimOneOnOneChatMessageRep.Value.GetRoomMessages(
                EnvLoginUser.LoginUser.UserId,
                queryMessageParam,
                fetchCount)
                .OrderBy(o => o.MessageID)
                .ToList();

            List<ChatMessageViewModel> chatMessageViewModels = msimOneOnOneChatMessages.Select(s => new ChatMessageViewModel()
            {
                PublishUserID = s.PublishUserID,
                MessageID = s.MessageID,
                MessageType = s.MessageType,
                Message = s.Message,
                PublishTimestamp = s.PublishTimestamp
            }).ToList();

            return new BaseReturnDataModel<List<ChatMessageViewModel>>(ReturnCode.Success, chatMessageViewModels);
        }

        public List<MSIMLastMessageKey> GetLastMessageKeys(List<long> messageIDs)
        {
            return _msimLastMessageInfoRep.Value.GetLastMessageKeys(messageIDs.ToJsonString());
        }

        public void DeleteLastMessages(List<MSIMLastMessageKey> lastMessageKeys)
        {
            _msimLastMessageInfoRep.Value.DeleteLastMessages(lastMessageKeys.ToJsonString());
        }

        public void DeleteChatMessages(List<MSIMOneOnOneChatMessageKey> oneOnOneChatMessageKeys)
        {
            _msimOneOnOneChatMessageRep.Value.DeleteChatMessages(oneOnOneChatMessageKeys.ToJsonString());
        }

        public List<MSIMLastMessageInfo> GetMSIMLastMessageInfos(QueryLastMessagesParam queryLastMessagesParam, int fetchCount)
            => _msimLastMessageInfoRep.Value.GetMSIMLastMessageInfos(queryLastMessagesParam, fetchCount);

        public List<MSIMOneOnOneChatMessageKey> GetOneOnOneChatMessageKeys(QueryOneOnOneMessageParam queryParam, int fetchCount)
            => _msimOneOnOneChatMessageRep.Value.GetOneOnOneChatMessageKeys(queryParam, fetchCount);
    }
}
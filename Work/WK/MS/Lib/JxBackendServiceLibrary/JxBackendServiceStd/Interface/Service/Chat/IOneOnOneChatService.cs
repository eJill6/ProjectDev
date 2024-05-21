using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Chat
{
    public interface IOneOnOneChatService
    {
        BaseReturnModel AddChatMessage(MSIMOneOnOneChatMessage msimOneOnOneChatMessage);

        void DeleteLastMessages(List<MSIMLastMessageKey> lastMessageKeys);

        void DeleteChatMessages(List<MSIMOneOnOneChatMessageKey> oneOnOneChatMessageKeys);
    }

    public interface IOneOnOneChatReadService
    {
        List<MSIMLastMessageKey> GetLastMessageKeys(List<long> messageIDs);

        List<MSIMLastMessageInfo> GetMSIMLastMessageInfos(QueryLastMessagesParam queryLastMessagesParam, int fetchCount);

        List<MSIMOneOnOneChatMessageKey> GetOneOnOneChatMessageKeys(QueryOneOnOneMessageParam queryParam, int fetchCount);

        BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryMessageParam, int fetchCount);
    }
}
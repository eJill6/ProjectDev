using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Chat
{
    public interface IChatMessageService : IEnvLoginUserService
    {
        BaseReturnModel AddChatMessage(SendAddMessageQueueParam param);

        BaseReturnModel ClearUnreadCount(RoomParam roomParam);
    }

    public interface IChatMessageReadService : IEnvLoginUserService
    {
        BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageViewModels(QueryLastMessagesParam queryLastMessagesParam);

        BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryMessageParam);

        BaseReturnDataModel<bool> HasUnreadMessage();
    }
}
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using System.Collections.Generic;

namespace SLPolyGame.Web.Interface
{
    public interface IChatApiWebSVService
    {
        BaseReturnModel ClearUnreadCount(RoomParam roomParam);

        BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageViewModels(QueryLastMessagesParam queryLastMessagesParam);

        BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryRoomMessageParam);

        BaseReturnDataModel<SendMessageResult> SendMessageToQueue(SendMessageParam sendMessageParam);

        BaseReturnDataModel<bool> HasUnreadMessage();
    }
}
using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using SLPolyGame.Web.Interface;

namespace ControllerShareLib.Services.WebSV.WebApi
{
    public class ChatWebApiService : BaseWebSVService, IChatApiWebSVService
    {
        protected override string RemoteControllerName => "ChatApiService";

        public BaseReturnModel ClearUnreadCount(RoomParam roomParam)
        {
            return GetHttpPostResponse<BaseReturnModel>(nameof(ClearUnreadCount), roomParam);
        }

        public BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageViewModels(QueryLastMessagesParam queryLastMessagesParam)
        {
            return GetHttpPostResponse<BaseReturnDataModel<List<LastMessageViewModel>>>(nameof(GetLastMessageViewModels), queryLastMessagesParam);
        }

        public BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryRoomMessageParam)
        {
            return GetHttpPostResponse<BaseReturnDataModel<List<ChatMessageViewModel>>>(nameof(GetRoomMessages), queryRoomMessageParam);
        }

        public BaseReturnDataModel<bool> HasUnreadMessage()
        {
            return GetHttpGetResponse<BaseReturnDataModel<bool>>(nameof(HasUnreadMessage), queryStringParts: null);
        }

        public BaseReturnDataModel<SendMessageResult> SendMessageToQueue(SendMessageParam sendMessageParam)
        {
            return GetHttpPostResponse<BaseReturnDataModel<SendMessageResult>>(nameof(SendMessageToQueue), sendMessageParam);
        }
    }
}
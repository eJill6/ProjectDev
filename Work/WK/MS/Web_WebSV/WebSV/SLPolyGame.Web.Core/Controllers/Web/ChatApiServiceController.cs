using JxBackendService.DependencyInjection;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Interface;

namespace SLPolyGame.Web.Core.Controllers.Web
{
    public class ChatApiServiceController : BaseAuthApiController, IChatApiWebSVService
    {
        private readonly Lazy<IChatApiWebSVService> _chatApiWebSVService;

        public ChatApiServiceController()
        {
            _chatApiWebSVService = DependencyUtil.ResolveService<IChatApiWebSVService>();                
        }

        [HttpPost]
        public BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryRoomMessageParam)
        {            
            return _chatApiWebSVService.Value.GetRoomMessages(queryRoomMessageParam);
        }

        [HttpPost]
        public BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageViewModels(QueryLastMessagesParam queryLastMessagesParam)
        {
            return _chatApiWebSVService.Value.GetLastMessageViewModels(queryLastMessagesParam);
        }

        [HttpPost]
        public BaseReturnDataModel<SendMessageResult> SendMessageToQueue(SendMessageParam sendMessageParam)
        {
            return _chatApiWebSVService.Value.SendMessageToQueue(sendMessageParam);
        }

        [HttpPost]
        public BaseReturnModel ClearUnreadCount(RoomParam roomParam)
        {
            return _chatApiWebSVService.Value.ClearUnreadCount(roomParam);
        }

        [HttpGet]
        public BaseReturnDataModel<bool> HasUnreadMessage()
        {
            return _chatApiWebSVService.Value.HasUnreadMessage();
        }
    }
}
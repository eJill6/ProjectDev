using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Interface;
using Web.Core.Controllers.Api.Base;

namespace Web.Core.Controllers.Api
{
    /// <summary>聊天控制器</summary>
    public class ChatController : BaseAuthApiController
    {
        private readonly Lazy<IChatApiWebSVService> _chatApiWebSVService;

        /// <summary>聊天控制器 ctor</summary>
        public ChatController()
        {
            _chatApiWebSVService = DependencyUtil.ResolveService<IChatApiWebSVService>();
        }

        /// <summary>傳送聊天訊息</summary>
        [HttpPost]
        public BaseReturnDataModel<SendMessageResult> SendMessage([FromBody] SendMessageParam sendMessageParam)
        {
            int ownerUserID = HttpContextUserService.GetUserId();

            if (ownerUserID.ToString().Equals(sendMessageParam.RoomID, StringComparison.OrdinalIgnoreCase))
            {
                return new BaseReturnDataModel<SendMessageResult>(ReturnCode.ParameterIsInvalid);
            }

            return _chatApiWebSVService.Value.SendMessageToQueue(sendMessageParam);
        }

        /// <summary>取得房間內訊息</summary>
        [HttpPost]
        public BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages([FromBody] QueryRoomMessageParam queryMessageParam)
        {
            return _chatApiWebSVService.Value.GetRoomMessages(queryMessageParam);
        }

        /// <summary> 聊天最後訊息列表,可查詢特定RoomID或全部列表 </summary>
        [HttpPost]
        public BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageInfos([FromBody] BaseQueryLastMessagesParam baseQueryLastMessagesParam)
        {
            var queryLastMessagesParam = baseQueryLastMessagesParam.CastByJson<QueryLastMessagesParam>();
            queryLastMessagesParam.OwnerUserID = HttpContextUserService.GetUserId();

            return _chatApiWebSVService.Value.GetLastMessageViewModels(queryLastMessagesParam);
        }

        /// <summary> 清除未讀數量 </summary>
        [HttpPost]
        public BaseReturnModel ClearUnreadCount([FromBody] RoomParam roomParam)
        {
            return _chatApiWebSVService.Value.ClearUnreadCount(roomParam);
        }

        /// <summary> 是否還有未讀訊息 </summary>
        [HttpGet]
        public BaseReturnModel HasUnreadMessage()
        {
            return _chatApiWebSVService.Value.HasUnreadMessage();
        }
    }
}
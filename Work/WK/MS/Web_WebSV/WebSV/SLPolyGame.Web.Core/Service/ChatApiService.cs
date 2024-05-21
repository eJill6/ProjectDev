using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Chat;
using SLPolyGame.Web.Interface;
using WebApiImpl;

namespace SLPolyGame.Web.Core.Service
{
    public class ChatApiService : BaseWebApiService, IChatApiWebSVService
    {
        private readonly Lazy<IChatMessageReadService> _chatMessageReadService;

        private readonly Lazy<IChatMessageService> _chatMessageService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        private readonly Lazy<IIdGeneratorService> _idGeneratorService;

        public ChatApiService()
        {
            _chatMessageReadService = DependencyUtil.ResolveEnvLoginUserService<IChatMessageReadService>(EnvLoginUser);
            _chatMessageService = DependencyUtil.ResolveEnvLoginUserService<IChatMessageService>(EnvLoginUser);
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
            _idGeneratorService = DependencyUtil.ResolveJxBackendService<IIdGeneratorService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        public BaseReturnModel ClearUnreadCount(RoomParam roomParam)
        {
            return _chatMessageService.Value.ClearUnreadCount(roomParam);
        }

        public BaseReturnDataModel<List<LastMessageViewModel>> GetLastMessageViewModels(QueryLastMessagesParam queryLastMessagesParam)
        {
            return _chatMessageReadService.Value.GetLastMessageViewModels(queryLastMessagesParam);
        }

        public BaseReturnDataModel<List<ChatMessageViewModel>> GetRoomMessages(QueryRoomMessageParam queryRoomMessageParam)
        {
            return _chatMessageReadService.Value.GetRoomMessages(queryRoomMessageParam);
        }

        public BaseReturnDataModel<bool> HasUnreadMessage()
        {
            return _chatMessageReadService.Value.HasUnreadMessage();
        }

        public BaseReturnDataModel<SendMessageResult> SendMessageToQueue(SendMessageParam sendMessageParam)
        {
            long messageId = _idGeneratorService.Value.CreateId();
            long publishTimestamp = DateTime.UtcNow.ToUnixOfTime();

            BaseReturnModel enqueueResult = _internalMessageQueueService.Value.EnqueueChatMessage(new SendAddMessageQueueParam()
            {
                OwnerUserID = EnvLoginUser.LoginUser.UserId,
                RoomID = sendMessageParam.RoomID,
                PublishUserID = EnvLoginUser.LoginUser.UserId,
                MessageID = messageId,
                MessageTypeValue = sendMessageParam.MessageType.Value,
                Message = sendMessageParam.Message,
                PublishTimestamp = publishTimestamp
            });

            if (!enqueueResult.IsSuccess)
            {
                return new BaseReturnDataModel<SendMessageResult>(enqueueResult.Message);
            }

            var sendMessageResult = new SendMessageResult()
            {
                MessageID = messageId,
                PublishTimestamp = publishTimestamp
            };

            return new BaseReturnDataModel<SendMessageResult>(ReturnCode.Success, sendMessageResult);
        }
    }
}
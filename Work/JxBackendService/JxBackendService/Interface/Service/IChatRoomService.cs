using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ChatRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IChatRoomService
    {
        BaseReturnDataModel<MemberChatRoomListViewModel> GetMemberChatRoomListForApi(int userId, List<int> existedUsers);

        BaseReturnDataModel<GroupChatRoomListViewModel> GetGroupChatRoomListForApi(int userId);

        BaseReturnDataModel<List<GroupChatRoomChildCreateModeViewModel>> GetCreateModeGroupChatRoomChildListForApi(int userId, 
                                                                                                                   int queryType, 
                                                                                                                   int? chatRoomId);

        BaseReturnDataModel<List<GroupChatRoomChildEditModeViewModel>> GetEditModeGroupChatRoomChildListForApi(int userId, int chatRoomId);

        BaseReturnDataModel<List<GroupChatRoomMessageViewModel>> GetGroupChatRoomMessageListForApi(int userId, int chatRoomId, int queryType, long? messageId);

        BaseReturnModel SendGroupChatRoomMessageForApi(int userId, string userName, string messageContent, int chatRoomId);

        BaseReturnDataModel<GroupChatRoomViewModel> CreateGroupChatRoomForApi(int userId, string chatRoomName, List<int> childUserIdList);

        BaseReturnModel AddGroupChatRoomChildForApi(int userId, int chatRoomId, List<int> childUserIdList);

        BaseReturnModel UpdateGroupChatRoomChildEnableSendMessageForApi(int userId, int chatRoomId, int childUserId, bool enableSendMessage);

        BaseReturnModel DeleteGroupChatRoomChildForApi(int userId, int chatRoomId, int childUserId);

        BaseReturnModel UpdateGroupChatRoomNameForApi(int userId, int chatRoomId, string newChatRoomName);

        BaseReturnModel DeleteGroupChatRoomForApi(int userId, int chatRoomId);

        BaseReturnDataModel<int> GetAllUnReadMessageCountForApi(int userId);

        BaseReturnModel UpdateAllUnReadMessageToRead(int userId);

        BaseReturnModel CheckSendMessageCountIsInMaxRange(int userId);
    }
}

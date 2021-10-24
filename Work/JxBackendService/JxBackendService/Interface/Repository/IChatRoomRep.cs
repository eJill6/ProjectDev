using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.ChatRoom;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ChatRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IChatRoomRep
    {
        /// <summary>
        /// 取得[人員]聊天室列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isNeedSpecialMembers"></param>
        /// <param name="excludeUserIds"></param>
        /// <returns></returns>
        BaseReturnDataModel<MemberChatRoomSqlResultModel> GetMemberChatRoomList(int userId, bool isNeedSpecialMembers, int[] excludeUserIds);

        /// <summary>
        /// 取得[群聊]聊天室資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        BaseReturnDataModel<List<GroupChatRoomSqlRawModel>> GetGroupChatRoomList(int userId);

        /// <summary>
        /// 取得[群聊]聊天室中所有下級列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        BaseReturnDataModel<GroupChatRoomChildListResultModel> GetGroupChatRoomChildList(int userId, int chatRoomId);

        /// <summary>
        /// 確定群聊聊天室是否存在
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        bool CheckChatRoomIsExist(int chatRoomId);

        /// <summary>
        /// 取得當前ChatRoom的訊息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <param name="queryType"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        BaseReturnDataModel<GroupChatRoomMessageListResultModel> GetGroupChatRoomMessageList(int userId, int chatRoomId, int queryType, long? messageId);

        /// <summary>
        /// 發送[群聊]聊天室訊息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="messageContent"></param>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        BaseReturnDataModel<GroupChatRoomMessageListResultModel> SendGroupChatRoomMessage(int userId, string userName, string messageContent, int chatRoomId);

        /// <summary>
        /// 建立[群聊]新聊天室
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomName"></param>
        /// <param name="childUserIds"></param>
        /// <returns></returns>
        BaseReturnDataModel<int> CreateGroupChatRoom(int userId, string chatRoomName, int[] childUserIds);

        /// <summary>
        /// 新增[群聊]聊天室下級
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <param name="chatRoomName"></param>
        /// <param name="childUserIds"></param>
        /// <returns></returns>
        BaseReturnModel AddGroupChatRoomChild(int userId, int chatRoomId, string chatRoomName, int[] childUserIds);

        /// <summary>
        /// 調整[群聊]聊天室中單一下級發話權限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <param name="chatRoomName"></param>
        /// <param name="childUserId"></param>
        /// <param name="enableSendMessage"></param>
        /// <returns></returns>
        BaseReturnModel UpdateGroupChatRoomChildEnableSendMessage(int userId, int chatRoomId, string chatRoomName, int childUserId, bool enableSendMessage);

        /// <summary>
        /// 刪除[群聊]聊天室中單一下級
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <param name="chatRoomName"></param>
        /// <param name="childUserId"></param>
        /// <returns></returns>
        BaseReturnModel DeleteGroupChatRoomChild(int userId, int chatRoomId, string chatRoomName, int childUserId);

        /// <summary>
        /// 更新[群聊]聊天室名稱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <param name="newChatRoomName"></param>
        /// <returns></returns>
        BaseReturnModel UpdateGroupChatRoomName(int userId, int chatRoomId, string newChatRoomName);

        /// <summary>
        /// 解散[群聊]聊天室
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        BaseReturnModel DeleteGroupChatRoom(int userId, int chatRoomId);

        /// <summary>
        /// 取得[人員] + [群聊] 總未讀訊息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        BaseReturnDataModel<int> GetAllUnReadMessageCount(int userId);

        /// <summary>
        /// 全部 [人員] + [群聊] 訊息全部設為已讀
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int UpdateAllUnReadMessageToRead(int userId);

        /// <summary>
        /// 取得聊天室設定
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        LettersGroupSetting GetGroupChatRoomSetting(int chatRoomId);

        /// <summary>
        /// 取得站內信全域設置
        /// </summary>
        /// <returns></returns>
        BaseReturnDataModel<GroupChatRoomConfigSettingSqlRawModel> GetChatRoomConfigSetting();
    }
}


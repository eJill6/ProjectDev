using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class GroupChatRoomChildListQueryTypes : BaseIntValueModel<GroupChatRoomChildListQueryTypes>
    {
        /// <summary> 當前用戶的所有下級 </summary>
        public static readonly GroupChatRoomChildListQueryTypes AllChildList = new GroupChatRoomChildListQueryTypes()
        { Value = 0 };

        /// <summary> 當前群組未進入的其他下級 </summary>
        public static readonly GroupChatRoomChildListQueryTypes NotInGroupChatRoomAnotherChild = new GroupChatRoomChildListQueryTypes()
        { Value = 1 };

        /// <summary> 當前群組的所有下級 </summary>
        public static readonly GroupChatRoomChildListQueryTypes InGroupChatRoomChild = new GroupChatRoomChildListQueryTypes()
        { Value = 2 };
    }

    /// <summary>
    /// 對應前台的ScrollBarDirection ，因為需要融舊共用，Value按照現在前台的
    /// </summary>
    public class GroupChatRoomMessageQueryTypes : BaseIntValueModel<GroupChatRoomMessageQueryTypes>
    {
        /// <summary> 取最新的 </summary>
        public static readonly GroupChatRoomMessageQueryTypes Newest = new GroupChatRoomMessageQueryTypes()
        { Value = 1 };

        /// <summary> 取較舊的 </summary>
        public static readonly GroupChatRoomMessageQueryTypes Previous = new GroupChatRoomMessageQueryTypes()
        { Value = 2 };

        /// <summary> 取較新的 </summary>
        public static readonly GroupChatRoomMessageQueryTypes After = new GroupChatRoomMessageQueryTypes()
        { Value = 3 };
    }

    /// <summary>
    /// 對應Sp Pro_UpdateLettersGroupSetting 
    /// DECLARE @OperatorStatusWithOthers INT = 0  --非新增移除，而是更新其他設定
	/// DECLARE @OperatorStatusWithAdding INT = 1  --新增成員至群組的狀態
    /// DECLARE @OperatorStatusWithDelete INT = 2  --移除會員離開群組的狀態
    /// </summary>
    public class GroupChatRoomChildUpdateTypes : BaseIntValueModel<GroupChatRoomChildUpdateTypes>
    {
        /// <summary> 取最新的 </summary>
        public static readonly GroupChatRoomChildUpdateTypes Others = new GroupChatRoomChildUpdateTypes()
        { Value = 0 };

        /// <summary> 取較舊的 </summary>
        public static readonly GroupChatRoomChildUpdateTypes Add = new GroupChatRoomChildUpdateTypes()
        { Value = 1 };

        /// <summary> 取較新的 </summary>
        public static readonly GroupChatRoomChildUpdateTypes Delete = new GroupChatRoomChildUpdateTypes()
        { Value = 2 };
    }

    /// <summary>
    /// 發送MQ時，控制App畫面上動作的ActionType
    /// </summary>
    public class GroupChatRoomActionControlTypes : BaseIntValueModel<GroupChatRoomActionControlTypes>
    {
        /// <summary> 更新聊天室訊息數量 </summary>
        public static readonly GroupChatRoomActionControlTypes UpdateMessage = new GroupChatRoomActionControlTypes()
        { Value = 1 };

        /// <summary> 被上級加入群組 </summary>
        public static readonly GroupChatRoomActionControlTypes BeJoinedNewChatRoom = new GroupChatRoomActionControlTypes()
        { Value = 2 };

        /// <summary> 已被上級調整禁言權限 </summary>
        public static readonly GroupChatRoomActionControlTypes BeUpdatedSendMessagePermission = new GroupChatRoomActionControlTypes()
        { Value = 3 };

        /// <summary> 已被上級踢出群組 </summary>
        public static readonly GroupChatRoomActionControlTypes BeDeleted = new GroupChatRoomActionControlTypes()
        { Value = 4 };

        /// <summary> 群組名稱已修改 </summary>
        public static readonly GroupChatRoomActionControlTypes ChatRoomNameBeUpdated = new GroupChatRoomActionControlTypes()
        { Value = 5 };

        /// <summary> 群組已解散 </summary>
        public static readonly GroupChatRoomActionControlTypes ChatRoomBeDeleted = new GroupChatRoomActionControlTypes()
        { Value = 6 };
    }
}

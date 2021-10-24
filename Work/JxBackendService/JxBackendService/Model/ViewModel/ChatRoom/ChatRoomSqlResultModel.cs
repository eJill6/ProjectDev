using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ChatRoom
{
    /// <summary>
    /// 用於取得[人員]聊天室列表的Sp回傳Model
    /// </summary>
    public class MemberChatRoomSqlResultModel
    {
        public List<MemberChatRoomSqlRawModel> ParentChatRoomList { get; set; }
        public List<MemberChatRoomSqlRawModel> ChildChatRoomList { get; set; }
    }

    /// <summary>
    /// 用於取得[人員]聊天室列表的Sp回傳Model
    /// TODO: 將前台的 LettersPersonInformation 參考改為參考這個
    /// </summary>
    public class MemberChatRoomSqlRawModel
    {
        public int PubUserId { get; set; } = 0;

        public string PubUserName { get; set; } = string.Empty;

        public int UnReadTotalCount { get; set; } = 0;

        public DateTime LastPubishDateTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        public bool IsUnReadMember;
    }

    /// <summary>
    /// 用於取得[群聊]聊天室列表的Sp回傳Model
    /// TODO: 將前台的 LettersGroupUnReadInfo 參考改為參考這個
    /// </summary>
    public class GroupChatRoomSqlRawModel
    {
        public int GroupId { get; set; } = 0;

        public string GroupName { get; set; } = string.Empty;

        public int CreatedUserId { get; set; } = 0;

        public DateTime LastReadingDateTime { get; set; }

        public int AmountOfJoinGroup { get; set; } = 0;

        public int UnReadTotalCount { get; set; } = 0;

        public bool EnablePublishAuthority { get; set; }
    }

    public class GroupChatRoomChildListResultModel
    {
        public GroupChatRoomSettingSqlRawModel GroupChatRoomSetting { get; set; }
        public List<GroupChatRoomChildSqlRawModel> GroupChatRoomChildList { get; set; }
    }

    /// <summary>
    /// 用於取得[群聊]聊天室下級列表的Sp回傳Model
    /// TODO: 將前台的 LettersGroupMember 參考改為參考這個
    /// </summary>
    public class GroupChatRoomChildSqlRawModel
    {
        /// <summary>
        /// 新增時站內信群聊成員時所產生的序列號的 Id
        /// </summary>
        public int SerialId { get; set; } = 0;

        /// <summary>
        /// 所參考的 GroupId
        /// </summary>
        public int GroupId { get; set; } = 0;

        /// <summary>
        /// 該 GroupId 的成員 UserId
        /// </summary>
        public int UserIdInGroup { get; set; } = 0;

        /// <summary>
        /// 該 GroupId 的成員 User 名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 該成員最後閱讀該群組站內信的的時間戳記
        /// </summary>
        public DateTime LastReadingDateTime { get; set; } = SqlDateTime.MinValue.Value;

        /// <summary>
        /// 是否有發言的權限
        /// </summary>
        public bool EnablePublishAuthority { get; set; }

        /// <summary>
        /// 非資料庫欄位，操作的狀態：0:非新增移除，而是更新其他設定; 1:新增成員至群組; 2:移除成員離開群組
        /// </summary>
        public int OperatorStatus { get; set; } = 0;

        /// <summary>
        /// 非資料表欄位，是否有編輯權限
        /// </summary>
        public bool HaveEditPermission { get; set; }

        /// <summary>
        /// 非資料表欄位，上級的 UserId
        /// </summary>
        public int ParentUserId { get; set; }
    }

    /// <summary>
    /// 站內信群聊的設定
    /// TODO: 將前台的 LettersGroupSetting 參考改為參考這個
    /// </summary>
    public class GroupChatRoomSettingSqlRawModel
    {
        /// <summary>
        /// 新增時站內信群聊時所產生的序列號的 Id
        /// </summary>
        public int GroupId { get; set; } = 0;

        /// <summary>
        /// 站內信群聊的名稱
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// 建立該群聊的 UserId
        /// </summary>
        public int CreateUserId { get; set; } = 0;

        /// <summary>
        /// 更新該群聊的 UserId
        /// </summary>
        public int UpdateUserId { get; set; } = 0;

        /// <summary>
        /// 是否已啟用
        /// </summary>
        public Boolean IsActive { get; set; } = true;

        /// <summary>
        /// 參加群聊的總人數
        /// </summary>
        public int AmountOfJoinGroup { get; set; } = 0;
    }

    public class GroupChatRoomMessageListResultModel
    {
        public GroupChatRoomChildSqlRawModel GroupChatRoomChildInfo { get; set; }
        public List<GroupChatRoomMessageSqlRawModel> GroupChatRoomMessageList { get; set; }
    }

    /// <summary>
    /// 站內信群聊的訊息
    /// TODO: 將前台的 LettersGroupMessage 參考改為參考這個
    /// </summary>
    public class GroupChatRoomMessageSqlRawModel
    {
        /// <summary>
        /// 發送時站內信群聊訊息時所產生的序列號的 Id
        /// </summary>
        public Int64 MessageId { get; set; } = 0;

        /// <summary>
        /// 所屬的站內信群聊 GroupId
        /// </summary>
        public int BelongGroupId { get; set; } = 0;

        /// <summary>
        /// 發送訊息的 UserId
        /// </summary>
        public int PublishUserId { get; set; } = 0;

        /// <summary>
        /// 發送訊息的 User Name
        /// </summary>
        public string PublishUserName { get; set; } = string.Empty;

        /// <summary>
        /// 發送訊息的時間
        /// </summary>
        public DateTime PublishDateTime { get; set; }

        /// <summary>
        /// 呈現站內信訊息得彙整內容
        /// </summary>
        public string LettersInformation { get; set; }

        /// <summary>
        /// 發送訊息的內容
        /// </summary>
        public string MessageContent { get; set; } = string.Empty;
    }

    /// <summary>
    /// 站內信全域設定目前使用到的Model
    /// TODO: 將前台的 LettersConfigSettings 參考改為參考這個
    /// </summary>
    public class GroupChatRoomConfigSettingSqlRawModel
    {
        /// <summary>
        /// 系統設定值的每人最大建立群聊數量
        /// </summary>
        public int MaxCreateLettersGroupCount { get; set; }

        /// <summary>
        /// 系統設定值的每分鐘最大發信次數
        /// </summary>
        public int MaxPublishLettersPerMinute { get; set; }

        /// <summary>
        /// 系統設定值單個群聊最大容納數量
        /// </summary>
        public int MaxPersonCountInPerGroup { get; set; }
    }
}

using MS.Core.MMModel.Models.Chat.Enum;

namespace MS.Core.MMModel.Models.Chat
{
    public class QueryRoomMessageParam
    {
        /// <summary>房間號</summary>
        public string RoomID { get; set; }

        /// <summary>前端最後一則訊息的訊息ID</summary>
        public long? LastMessageID { get; set; }

        /// <summary>前端最後一則訊息的發送時間</summary>
        public long? LastPublishTimestamp { get; set; }

        /// <summary>搜尋方向</summary>
        public int SearchDirectionTypeValue { get; set; } = (int)SearchDirectionType.Forward;
    }
}

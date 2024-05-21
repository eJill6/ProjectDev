using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.Chat;

namespace JxBackendService.Model.Param.Chat
{
    public class QueryRoomMessageParam
    {
        /// <summary>房間號</summary>
        [CustomizedRequired]
        public string RoomID { get; set; }

        /// <summary>前端最後一則訊息的訊息ID</summary>
        public long? LastMessageID { get; set; }

        /// <summary>前端最後一則訊息的發送時間</summary>
        public long? LastPublishTimestamp { get; set; }

        /// <summary>搜尋方向</summary>
        public int SearchDirectionTypeValue { get; set; } = SearchDirectionType.Forward.Value;
    }
}
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Param.Chat
{
    public class RoomParam
    {
        /// <summary>房間號</summary>
        [CustomizedRequired]
        public string RoomID { get; set; }
    }

    public class OwnerRoomParam : RoomParam
    {
        public int OwnerUserID { get; set; }
    }
}
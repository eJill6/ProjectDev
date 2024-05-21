namespace JxBackendService.Model.Param.Chat
{
    public class DeleteMessageQueueParam
    {
        public int OwnerUserID { get; set; }

        public string RoomID { get; set; }

        public long SmallEqualThanTimestamp { get; set; }
    }
}
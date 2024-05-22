namespace FakeMSSeal.Models.Messages
{
    /// <summary>
    /// 投注訊息
    /// </summary>
    public class BetingMessageRequest : MessageRequest<BetingContent>
    {
    }

    /// <summary>
    /// 投注內容
    /// </summary>
    public class BetingContent
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 投注金額
        /// </summary>
        public int BetAmount { get; set; }

        /// <summary>
        /// 彩種編號
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// 彩種名稱
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 房間編號
        /// </summary>
        public int RoomNo { get; set; }
    }
}

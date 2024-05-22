namespace FakeMSSeal.Models
{
    /// <summary>
    /// 扣款請求
    /// </summary>
    public class DeductRequest : BaseRequest
    {
        /// <summary>
        /// App名稱
        /// </summary>
        public string App { get; set; }

        /// <summary>
        /// 注单id
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNumber { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 游戏id
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 游戏描述
        /// </summary>
        public string GameDetail { get; set; }

        public override bool IsSkipSignSalt() => false;
    }
}

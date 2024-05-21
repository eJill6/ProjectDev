namespace ControllerShareLib.Models.LongResult
{
    /// <summary>
    /// 长龙資料
    /// </summary>
    public class LongData
    {
        /// <summary>
        /// 當期期號
        /// </summary>
        public string CurrentIssueNo { get; set; }

        /// <summary>
        /// 彩種名稱代碼
        /// </summary>
        public int GameTypeId { get; set; }

        /// <summary>
        /// 彩种ID
        /// </summary>
        public int LotteryID { get; set; }

        /// <summary>
        /// 彩種名稱
        /// </summary>
        public string LotteryTypeName { get; set; }

        /// <summary>
        /// 長龍數據
        /// </summary>
        public LongInfo[] LongInfo { get; set; } = new LongInfo[] { };

        /// <summary>
        /// 開獎時間 (拿來比對資料新舊用)
        /// </summary>
        public DateTime LotteryTime { get; set; }

        /// <summary>
        /// 玩法大類名稱
        /// </summary>
        public string GameTypeName { get; set; }
    }
}
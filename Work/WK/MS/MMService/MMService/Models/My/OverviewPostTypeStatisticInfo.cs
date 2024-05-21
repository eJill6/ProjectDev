using MS.Core.MMModel.Models.Post.Enums;

namespace MMService.Models.My
{
    /// <summary>
    /// 發佈贴子類型統計
    /// </summary>
    public class OverviewPostTypeStatisticInfo
    {
        /// <summary>
        /// 贴子類型
        /// </summary>
        public PostType Type { get; set; }

        /// <summary>
        /// 已發贴
        /// </summary>
        public int PublishedCount { get; set; }

        /// <summary>
        /// 解鎖次數
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 總收益(累計收益)
        /// </summary>
        public string TotalIncome { get; set; } = string.Empty;
    }
}
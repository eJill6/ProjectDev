using MS.Core.MMModel.Models.User.Enums;

namespace MMService.Models.My
{
    /// <summary>
    /// 總覽頁資訊
    /// </summary>
    public class Overview
    {
        /// <summary>
        /// 頭像
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 商場開關
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 用戶等級
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 該贴用戶身份 (二期新加)
        /// </summary>
        public IdentityType UserIdentity { get; set; }

        /// <summary>
        /// 卡的類型
        /// </summary>
        public int[] CardType { get; set; } = Array.Empty<int>();

        /// <summary>
        /// 保證金 (三期拿掉)
        /// </summary>
        public string CautionMoney { get; set; } = string.Empty;

        /// <summary>
        /// 保證金
        /// </summary>
        public string EarnestMoney { get; set; } = string.Empty;

        /// <summary>
        /// 本月收益
        /// </summary>
        public string Income { get; set; } = string.Empty;

        /// <summary>
        /// 暫鎖收益
        /// </summary>
        public string FreezeIncome { get; set; } = string.Empty;

        /// <summary>
        /// 累積發佈上限
        /// </summary>
        public int PublishLimit { get; set; }

        /// <summary>
        /// 剩餘發佈次數
        /// </summary>
        public int RemainPublish { get; set; }
        /// <summary>
        /// 免费解锁次数
        /// </summary>
        public int RemainingFreeUnlock { get; set; }

        /// <summary>
        /// 積分
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        /// 發贴統計
        /// </summary>
        public OverviewPostTypeStatisticInfo[] Statistic { get; set; } = new OverviewPostTypeStatisticInfo[0];
    }
}
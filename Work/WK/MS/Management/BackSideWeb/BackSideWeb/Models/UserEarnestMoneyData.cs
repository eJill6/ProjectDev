namespace BackSideWeb.Models
{
    public class UserEarnestMoneyData
    {
        /// <summary>
        /// 调整记录ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ExamineTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public string EarnestMoneyText => EarnestMoney.ToString("0");
    }
}

namespace MS.Core.MM.Models.Wallets
{
    public class ResWalletInfo
    {
        /// <summary>
        /// 覓餘額
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 鑽石餘額
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 暫鎖收益
        /// </summary>
        public string FreezeIncome { get; set; } = string.Empty;

        /// <summary>
        /// 本月收益
        /// </summary>
        public string Income { get; set; } = string.Empty;
    }
}

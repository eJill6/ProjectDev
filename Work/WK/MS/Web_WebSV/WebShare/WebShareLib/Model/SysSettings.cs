namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// SysSettings:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public class SysSettings
    {
        public SysSettings()
        { }

        #region Model

        /// <summary>
        /// ID
        /// </summary>
        public int SettingsID { set; get; }

        /// <summary>
        /// 每次下单最大注数
        /// </summary>
        public int? MaxBetCount { set; get; }

        /// <summary>
        /// 单注下单分数最大值
        /// </summary>
        public decimal? MaxOneBetMoney { set; get; }

        /// <summary>
        /// 单注下单分数最小值
        /// </summary>
        public decimal? MinOneBetMoney { set; get; }

        /// <summary>
        /// 最大中奖金额
        /// </summary>
        public decimal? MaxBonusMoney { set; get; }

        /// <summary>
        /// 最小充值金额
        /// </summary>
        public decimal? MinMoneyIn { set; get; }

        /// <summary>
        /// 最高配额值，总号使用
        /// </summary>
        public decimal? MaxUserRebatePro { set; get; }

        #endregion Model
    }
}
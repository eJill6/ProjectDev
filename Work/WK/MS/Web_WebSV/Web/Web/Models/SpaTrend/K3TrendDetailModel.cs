namespace Web.Models.SpaTrend
{
    public class K3TrendDetailModel
    {
        public string Issue { get; set; }

        public string[] Numbers { get; set; }

        /// <summary>
        /// 跨位
        /// </summary>
        public string KuaWei { get; set; }

        /// <summary>
        /// 百位
        /// </summary>
        public string Bai { get; set; }

        /// <summary>
        /// 十位
        /// </summary>
        public string Shi { get; set; }

        /// <summary>
        /// 各位
        /// </summary>
        public string Ge { get; set; }

        /// <summary>
        /// 和值分布
        /// </summary>
        public string HeZhiFenBu { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public string DaXiao { get; set; }

        /// <summary>
        /// 大?
        /// </summary>
        public bool IsDa { get; set; }

        /// <summary>
        /// 单双
        /// </summary>
        public string DanShuang { get; set; }

        /// <summary>
        /// 单?
        /// </summary>
        public bool IsDan { get; set; }
    }
}
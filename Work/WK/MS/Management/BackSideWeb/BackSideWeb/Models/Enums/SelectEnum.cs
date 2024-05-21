using System.ComponentModel;

namespace BackSideWeb.Models.Enums
{
    /// <summary>
    /// 下拉選單類型
    /// </summary>
    public enum SelectEnum : byte
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 1,

        /// <summary>
        /// 请选择
        /// </summary>
        [Description("请选择")]
        Choose = 2
    }
}

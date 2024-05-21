using System.ComponentModel;

namespace MS.Core.MMModel.Models.Lottery.Enum
{
    public enum IsActiveEnum
    {
        /// <summary>
        /// 不启用
        /// </summary>
        [Description("不启用")]
        Close = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Open = 1,
    }
}

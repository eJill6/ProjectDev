using System.ComponentModel;

namespace MS.Core.MMModel.Models.Vip.Enums
{
    public enum VipType : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 白银月卡
        /// </summary>
        [Description("白银月卡")]
        Silver = 1,
        /// <summary>
        /// 黄金季卡
        /// </summary>
        [Description("黄金季卡")]
        Gold = 2,

        /// <summary>
        /// AB神卡
        /// </summary>
        [Description("AB神卡")]
        Diamond = 3,
    }
}

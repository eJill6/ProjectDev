using System.ComponentModel;

namespace MS.Core.MMModel.Models.Post.Enums
{
    /// <summary>
    /// 贴子類型
    /// </summary>
    public enum PostType : byte
    {
        /// <summary>
        /// 广场
        /// </summary>
        [Description("广场")]
        Square = 1,

        /// <summary>
        /// 寻芳阁(原為中介)
        /// </summary>
        [Description("寻芳阁")]
        Agency = 2,

        /// <summary>
        /// 官方
        /// </summary>
        [Description("官方")]
        Official = 3,

        /// <summary>
        /// 体验
        /// </summary>
        [Description("体验")]
        Experience = 4
    }
}
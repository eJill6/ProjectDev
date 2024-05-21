using System.ComponentModel;

namespace JxBackendService.Model.Enums.MM
{
    /// <summary>
    /// 帖子類型
    /// </summary>
    public enum PostType : byte
    {
        /// <summary>
        /// 廣場
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
        /// 體驗
        /// </summary>
        [Description("体验")]
        Experience = 4
    }
}
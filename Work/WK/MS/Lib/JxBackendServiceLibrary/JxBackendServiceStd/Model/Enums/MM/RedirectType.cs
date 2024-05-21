using System.ComponentModel;

namespace JxBackendService.Model.Enums.MM
{
    /// <summary>
    /// 秘色转导页面类型
    /// </summary>
    public enum RedirectType : byte
    {
        /// <summary>
        /// 官方
        /// </summary>
        [Description("官方")]
        Official = 1,

        /// <summary>
        /// 寻芳阁
        /// </summary>
        [Description("寻芳阁")]
        Agency = 2,

        /// <summary>
        /// 广场
        /// </summary>
        [Description("广场")]
        Square = 3,

        /// <summary>
        /// 店铺
        /// </summary>
        [Description("店铺")]
        Shop = 4,

        /// <summary>
        /// 官方帖子
        /// </summary>
        [Description("官方帖子")]
        OfficialPost = 5,

        /// <summary>
        /// 广场寻芳阁帖子
        /// </summary>
        [Description("广场寻芳阁帖子")]
        SquareOrAgencyPost = 6
    }
}
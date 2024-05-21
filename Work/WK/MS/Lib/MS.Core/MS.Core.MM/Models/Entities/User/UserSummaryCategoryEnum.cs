using System.ComponentModel;

namespace MS.Core.MM.Models.Entities.User
{
    public enum UserSummaryCategoryEnum : byte
    {
        /// <summary>
        /// 1.广场 
        /// </summary>
        [Description("广场")]
        Square = 1,

        /// <summary>
        /// 2.担保(原為中介) 现在为寻芳阁
        /// </summary>
        [Description("担保")]
        Agency = 2,

        /// <summary>
        /// 3.官方
        /// </summary>
        [Description("官方")]
        Official = 3,

        /// <summary>
        /// 体验
        /// </summary>
        [Description("体验")]
        Experience = 4,

        /// <summary>
        /// 致富银卡
        /// </summary>
        [Description("致富银卡")]
        Silver = 11,
        /// <summary>
        /// 致富金卡
        /// </summary>
        [Description("致富金卡")]
        Gold = 12,
        /// <summary>
        /// 致富钻卡
        /// </summary>
        [Description("致富钻卡")]
        Diamond = 13,
        /// <summary>
        /// 官方店铺（收藏数）
        /// </summary>
        [Description("官方店铺")]
        Shop =14,
    }
}
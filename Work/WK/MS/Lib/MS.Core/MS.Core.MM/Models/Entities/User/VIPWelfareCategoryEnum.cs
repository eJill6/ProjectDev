using System.ComponentModel;

namespace MS.Core.MM.Models.Entities.User
{
    public enum VIPWelfareCategoryEnum
    {
        /// <summary>
        /// 广场
        /// </summary>
        [Description("广场")]
        Square = 1,

        /// <summary>
        /// 担保(原為中介)
        /// </summary>
        [Description("担保")]
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
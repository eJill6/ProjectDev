using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.My.Enums
{
    public enum MyUserFavoriteCategoryEnum
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
        /// 官方店铺（收藏数）
        /// </summary>
        [Description("官方店铺")]
        Shop = 14,
    }
}

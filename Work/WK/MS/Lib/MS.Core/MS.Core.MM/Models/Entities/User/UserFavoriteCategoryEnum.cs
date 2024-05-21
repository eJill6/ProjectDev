using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.User
{
    public enum UserFavoriteCategoryEnum
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
        /// 店铺
        /// </summary>
        [Description("店铺")]
        Shop =14,
    }
}

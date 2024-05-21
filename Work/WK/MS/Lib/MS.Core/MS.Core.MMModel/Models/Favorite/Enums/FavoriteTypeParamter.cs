using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Favorite.Enums
{
    public enum FavoriteTypeParamter
    {
        [Description("全部")]
        All =0,
        [Description("广场")]
        SquarePost = 1,
        [Description("寻芳阁")]
        XFGPost = 2,
        [Description("店铺")]
        Shop =3,
    }
}

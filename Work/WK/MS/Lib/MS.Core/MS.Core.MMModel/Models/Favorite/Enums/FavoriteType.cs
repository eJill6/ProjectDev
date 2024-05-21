using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Favorite.Enums
{
    public enum FavoriteType
    {
        [Description("全部")]
        Post=1,
        [Description("店铺")]
        Shop =2,
    }
}

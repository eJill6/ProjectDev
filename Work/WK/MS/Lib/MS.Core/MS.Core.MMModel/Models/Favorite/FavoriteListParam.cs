using MS.Core.MMModel.Models.Favorite.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Favorite
{
    public class FavoriteListParam: PageParam
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// 收藏内容类型1帖子,2店铺
        /// </summary>
        public int? FavoriteType { get; set; }
    }
}

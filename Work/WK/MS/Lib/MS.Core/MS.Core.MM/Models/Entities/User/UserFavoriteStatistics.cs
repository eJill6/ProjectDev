using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.User
{
    public class UserFavoriteStatistics
    {
        /// <summary>
        /// 收藏种类
        /// </summary>
        public UserFavoriteCategoryEnum FavoriteEnum { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public int Favorites { get; set; }
    }
}

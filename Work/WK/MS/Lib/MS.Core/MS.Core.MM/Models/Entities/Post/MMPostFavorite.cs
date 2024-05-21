using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;
using System.Text.Json.Serialization;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMPostFavorite : BaseDBModel
    {

        /// <summary>
        /// FavoriteId
        /// </summary>
        [PrimaryKey]
        public string FavoriteId { get; set; }

        /// <summary>
        /// 帖子/店铺ID
        /// </summary>

        public string PostId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 类型。1：帖子、2：店铺
        /// </summary>
        public int Type { get; set; }
    }
}
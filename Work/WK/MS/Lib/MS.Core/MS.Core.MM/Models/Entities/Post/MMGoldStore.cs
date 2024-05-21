using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;
using System.Text.Json.Serialization;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMGoldStore : BaseDBModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Top
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>

        public int? UserId { get; set; }

        /// <summary>
        /// 操作人員
        /// </summary>
        public string Operator { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 类型：1：官方推荐 2：金牌店铺
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [JsonIgnore]
        public DateTime? UpdateTime { get; set; }
    }
}
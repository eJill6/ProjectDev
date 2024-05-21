using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;
using System.Text.Json.Serialization;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMPostWeight : BaseDBModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }
        /// <summary>
        /// 贴 Id
        /// </summary>
        [EntityType(DbType.String)]
        public string PostId { get; set; } = string.Empty;
        /// <summary>
        /// 權重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 操作人員
        /// </summary>
        public string Operator { get; set; } = string.Empty;
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        [JsonIgnore]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 贴子類型。1：廣場、2：中介、3：官方、4：體驗
        /// </summary>
        public byte PostType { get; set; }
    }
}

using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    /// <summary>
    /// MMPost Table
    /// </summary>
    public class MMPostServiceMapping : BaseDBModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 贴 Id
        /// </summary>
        [EntityType(DbType.String)]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 服務項目id
        /// </summary>
        public int ServiceId { get; set; }
    }
}
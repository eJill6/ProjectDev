using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMPostContact : BaseDBModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 贴子 id
        /// </summary>
        [EntityType(DbType.String)]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡方式。1：微信、2：QQ、3：手機號
        /// </summary>
        public byte ContactType { get; set; }

        /// <summary>
        /// 聯繫方式
        /// </summary>
        public string Contact { get; set; } = string.Empty;
    }
}
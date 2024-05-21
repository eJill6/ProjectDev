using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    /// <summary>
    /// 贴子解鎖
    /// </summary>
    public class MMPostTransactionModel : BaseDBModel
    {
        /// <summary>
        /// 贴子交易ID
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string Id { get; set; }
        /// <summary>
        /// 贴子Id
        /// </summary>
        public string PostId { get; set; }
        /// <summary>
        /// 贴子Type
        /// </summary>
        public PostType PostType { get; set; }
        /// <summary>
        /// 付費用戶Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 消費時間
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 投訴單Id
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 發贴人
        /// </summary>
        public int PostUserId { get; set; }

    }
}

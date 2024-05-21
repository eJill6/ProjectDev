using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    /// <summary>
    /// 舉報表
    /// </summary>
    public class MMPostReport : BaseDBModel
    {
        /// <summary>
        /// 舉報 id
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string ReportId { get; set; } = string.Empty;

        /// <summary>
        /// 舉報原因。0：騙子、1：廣告騷擾、2：貨不對版、3：無效聯絡方式
        /// </summary>
        public byte ReportType { get; set; }

        /// <summary>
        /// 投訴人 UserId
        /// </summary>
        public int ComplainantUserId { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        [EntityType(DbType.String)]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 解鎖單編號
        /// </summary>
        [EntityType(DbType.String)]
        public string PostTranId { get; set; } = string.Empty;

        /// <summary>
        /// 贴子類型。1：廣場、2：担保(原為中介)、3：官方、4：體驗
        /// </summary>
        public byte PostType { get; set; }

        /// <summary>
        /// 狀態。0：審核中、1：審核通過、2：未通過
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 舉報內容
        /// </summary>
        public string Describe { get; set; } = string.Empty;

        /// <summary>
        /// 檢舉時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string? ExamineMan { get; set; } = string.Empty;

        /// <summary>
        /// ExamineTime
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string? Memo { get; set; } = string.Empty;
    }
}
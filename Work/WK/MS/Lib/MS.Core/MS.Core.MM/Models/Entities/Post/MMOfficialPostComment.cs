using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMOfficialPostComment : BaseDBModel
    {
        /// <summary>
        /// 評論 Id
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string CommentId { get; set; } = string.Empty;

        /// <summary>
        /// 預約單 Id
        /// </summary>
        [EntityType(DbType.String)]
        public string BookingId { get; set; } = string.Empty;

        /// <summary>
        /// 贴子 Id
        /// </summary>
        [EntityType(DbType.String)]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 評論當下頭像連結
        /// </summary>
        public string? AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 評論人 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 評論當下暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 評論狀態。0：審核中、1：已評論(通過)、2：審核不通過
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 顏值
        /// </summary>
        public int FacialScore { get; set; }

        /// <summary>
        /// 服務品質
        /// </summary>
        public int ServiceQuality { get; set; }

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string? ExamineMan { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 審核未通過原因
        /// </summary>
        public string? Memo { get; set; }
    }
}
using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.User
{
    public class MMApplyRefund : BaseDBModel
    {
        /// <summary>
        /// 退款編號
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string RefundId { get; set; } = string.Empty;

        /// <summary>
        /// 用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///預約單號
        /// </summary>
        public string BookingId { get; set; } = string.Empty;

        /// <summary>
        /// 退費理由類型。1：欺騙、2：貨不對版
        /// </summary>
        public byte ReasonType { get; set; }

        /// <summary>
        /// 退費理由
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 審核狀態。0：審核中、1：通過、2：不通過
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string? ExamineMan { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Memo { get; set; }
    }
}
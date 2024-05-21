using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminIncomeListParam : PageParam
    {
        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 收益单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 收益会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 贴子區域
        /// </summary>
        public PostType? PostType { get; set; }

        /// <summary>
        /// 锁定状态(0: 到期, 1: 未到期)
        /// </summary>
        public int? LockType { get; set; }

        /// <summary>
        /// 收益单状态(1: 审核入账, 2: 暂锁, 3: 入账, 4: 审核不入账)
        /// </summary>
        public IncomeExpenseStatusEnum? Status { get; set; }

        /// <summary>
        /// 0: 解锁时间
        /// 1: 应入账时间
        /// </summary>
        public int DateTimeType { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 预约支付方式
        /// </summary>
        public int BookingPaymentType { get; set; }

        /// <summary>
        /// 预约单id或解锁单id
        /// </summary>
        public string[] SourceIds { get; set; } = Array.Empty<string>();
        /// <summary>
        /// 收益会员身份
        /// </summary>
        public int? ApplyIdentity { get; set; }
    }
}

using JxBackendService.Model.Paging;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryAdminIncomeParam : BasePagingRequestParam
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public string? PostId { get; set; }

        /// <summary>
        /// 收益单ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 收益会员ID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 帖子區域
        /// </summary>
        public int? PostType { get; set; }

        /// <summary>
        /// 锁定状态(0: 到期, 1: 未到期)
        /// </summary>
        public int? LockType { get; set; }

        /// <summary>
        /// 收益单状态(0: 暂锁, 1: 异常, 2: 入账, 3: 审核不入账, 4: 审核入账)
        /// </summary>
        public int? Status { get; set; }

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
        /// 收益会员身份查询
        /// </summary>
        public int? ApplyIdentity { get; set; }
    }
}

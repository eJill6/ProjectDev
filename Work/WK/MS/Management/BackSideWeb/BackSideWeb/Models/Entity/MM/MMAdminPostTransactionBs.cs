using JxBackendService.Model.Entity.Base;
using MS.Core.Models.Models;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMAdminPostTransactionBs : BaseEntityModel
    {
        /// <summary>
        /// 解锁单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        public string CategoryText { get; set; }

        /// <summary>
        /// 解锁会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Rebate { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public string RebateText { get; set; }

        /// <summary>
        /// 解锁价格
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 解锁价格
        /// </summary>
        public string AmountText { get; set; }

        /// <summary>
        /// 解锁方式
        /// </summary>
        public string UnlockType { get; set; }

        public string DiscountType { get; set; }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        public string RealExpenseAmount { get; set; }

        /// <summary>
        /// 收益单ID
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 投诉退款
        /// </summary>
        public string RefundMemo { get; set; }
    }
}
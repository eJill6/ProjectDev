using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using MS.Core.Models.Models;

namespace BackSideWeb.Models.ViewModel
{
    public class QueryAdminPostTransactionViewModel : IDataKey
    {
        public string KeyContent => Id.ToString();

        /// <summary>
        /// 解锁单ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionId), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string Id { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionPostId), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string PostId { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionCategoryText), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string CategoryText { get; set; }

        /// <summary>
        /// 解锁会员ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionUserId), ResourceType = typeof(DisplayElement), Sort = 4)]
        public int UserId { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionCreateTimeText), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Rebate { get; set; }



        /// <summary>
        /// 解锁钻石
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 解锁钻石
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionAmount), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string AmountText { get; set; }

        /// <summary>
        /// 解锁方式
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionUnlockType), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string UnlockType { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public string RebateText { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionRebate), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string DiscountType { get; set; }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionRealExpenseAmount), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string RealExpenseAmount { get; set; }

        /// <summary>
        /// 收益单ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionTargetId), ResourceType = typeof(DisplayElement), Sort = 10)]
        public string IncomeExpenseId { get; set; }

        /// <summary>
        /// 投诉退款
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminPostTransactionRefundMemo), ResourceType = typeof(DisplayElement), Sort = 11)]
        public string RefundMemo { get; set; }

    }
}

using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminPostTransaction
{
    public class AdminPostTransactionList
    {
        /// <summary>
        /// 解锁单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public string CategoryText => ((PostType)Category).GetDescription();

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
        public string RebateText => Rebate.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 解锁价格
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 解锁价格
        /// </summary>
        public string AmountText => Amount.ToString("0");

        /// <summary>
        /// 解锁方式
        /// </summary>
        public string UnlockType
        {
            get
            {
                if (Rebate == 1.0M)
                {
                    return "一般";
                }
                else if (Rebate == 0.0M)
                {
                    return "免费";
                }
                return "打折";
            }
        }

        /// <summary>
        /// 折扣
        /// </summary>
        public string DiscountType
        {
            get
            {
                if (Rebate == 1.0M)
                {
                    return "-";
                }
                else if (Rebate == 0.0M)
                {
                    return "免费";
                }

                return $"{Rebate:0%}";
            }
        }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        public string RealExpenseAmount => (Amount * Rebate).ToString("0");

        /// <summary>
        /// 收益单ID
        /// </summary>
        public string IncomeExpenseId { get; set; }

        /// <summary>
        /// 投诉退款
        /// </summary>
        public string RefundMemo { get; set; } = "-";

    }
}

using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerUserCardsList
    {
        /// <summary>
        /// 订单号
        /// </summary>
        /// 
        public string Id { get; set; }

        /// <summary>
        /// 會員卡
        /// </summary>
        public int VipId { get; set; }

        /// <summary>
        /// 會員卡
        /// </summary>
        public string VipName { get; set; }

        /// <summary>
        /// 购买会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayTypeText => PayType.GetDescription();

        /// <summary>
        /// 支付方式
        /// </summary>
        public IncomeExpensePayType PayType { get; set; }

        /// <summary>
        /// 消费余额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 消费余额
        /// </summary>
        public string AmountText => Amount.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 购买时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);


        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public string EffectiveTimeText => EffectiveTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime EffectiveTime { get; set; }
    }
}
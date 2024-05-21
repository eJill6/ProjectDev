using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerUserCardsListParam : PageParam
    {
        /// <summary>
        /// 訂單號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 购买会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 会员卡
        /// </summary>
        public int? VipId { get; set; }


        /// <summary>
        /// 支付方式
        /// </summary>
        public IncomeExpensePayType? PayType { get; set; }


        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
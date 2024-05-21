using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using MS.Core.MMModel.Models.AdminUserManager;
using JxBackendService.Model.Paging;

namespace JxBackendService.Model.Param.User
{
    public class SearchIncomeExpensesParam: BasePagingRequestParam
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 收付行为(0: 贴子解锁, 1: 支付预约金, 2: 取消预约金, 3: 购买会员卡, 4: 贴子收益, 5:收益退款，6.支付全额,7.退回全额)
        /// </summary>
        public AdminIncomeExpensesCategory? Category { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public PostType? PostType { get; set; }

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

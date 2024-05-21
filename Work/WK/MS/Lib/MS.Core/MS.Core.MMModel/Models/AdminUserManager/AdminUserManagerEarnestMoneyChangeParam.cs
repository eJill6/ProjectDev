using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerEarnestMoneyChangeParam
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 原保证金
        /// </summary>
        public decimal OriginalEarnestMoney { get; set; }

        public string OriginalEarnestMoneyText => OriginalEarnestMoney.ToString(GlobalSettings.AmountToIntString);

        /// <summary>
        /// 保证金
        /// </summary>
        [Required(ErrorMessage = "编辑保证金不可为空")]
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string ExamineMan { get; set; }
    }
}

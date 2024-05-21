using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerEarnestMoneyHisParam
    {

        /// <summary>
        /// 购买会员ID
        /// </summary>
        public int UserId { get; set; }

       public int MaxCount { get; set; } = 5;
    }
}
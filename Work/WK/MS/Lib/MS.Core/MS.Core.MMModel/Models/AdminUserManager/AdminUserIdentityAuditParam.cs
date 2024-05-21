using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserIdentityAuditParam
    {
        /// <summary>
        /// 审核ID
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 缴交保证金
        /// </summary>
        public decimal? EarnestMoney { get; set; }

        /// <summary>
        /// 增加发贴次数
        /// </summary>
        public int? ExtraPostCount { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 审核状态，1：通过，2：未通过
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 身份备注
        /// </summary>
        public string Memo { get; set; }
    }
}

using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerIdentityApplyListParam : PageParam
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 当前身份
        /// </summary>
        public IdentityType? OriginalIdentity { get; set; }

        /// <summary>
        /// 申请身份
        /// </summary>
        public IdentityType? ApplyIdentity { get; set; }

        /// <summary>
        /// 申请开始時間
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 申请結束時間
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
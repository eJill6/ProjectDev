using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminUserManagerUsersListParam : PageParam
    {
        /// <summary>
        /// 收益会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 多個使用者編號，在搜尋會員卡時會用到
        /// </summary>
        public int[] UserIds { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public int? UserIdentity { get; set; }

        /// <summary>
        /// 会员卡
        /// </summary>
        public int? VipId { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否营业
        /// </summary>
        public bool? IsOpen { get; set; }
    }
}
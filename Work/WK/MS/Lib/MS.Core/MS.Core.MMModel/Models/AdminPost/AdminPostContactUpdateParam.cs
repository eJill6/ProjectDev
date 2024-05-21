using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    /// <summary>
    /// 批量修改联系
    /// </summary>
    public class AdminPostContactUpdateParam
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; } = "";

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; } = "";

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; } = "";
    }
}
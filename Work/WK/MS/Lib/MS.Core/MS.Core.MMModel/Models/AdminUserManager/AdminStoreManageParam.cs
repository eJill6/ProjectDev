using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminStoreManageParam : PageParam
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// 店铺状态
        /// </summary>
        public int? IsOpen { get; set; }
    }
}
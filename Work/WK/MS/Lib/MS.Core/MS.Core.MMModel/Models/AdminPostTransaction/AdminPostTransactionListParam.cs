using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminPostTransaction
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminPostTransactionListParam : PageParam
    {
        /// <summary>
        /// 贴子編號
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 贴子區域
        /// </summary>
        public PostType? PostType { get; set; }

        /// <summary>
        /// 解锁方式(0: 一般, 1: 免費, 2:打折)
        /// </summary>
        public int? UnlockType { get; set; }

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

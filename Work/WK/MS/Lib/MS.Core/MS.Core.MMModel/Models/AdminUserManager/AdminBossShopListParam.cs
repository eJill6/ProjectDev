using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminUserManager
{
	/// <summary>
	/// 列表搜尋參數
	/// </summary>
	public class AdminBossShopListParam : PageParam
	{
		/// <summary>
		/// 会员ID
		/// </summary>
		public int? UserId { get; set; }

		/// <summary>
		/// 開始時間
		/// </summary>
		public DateTime? BeginDate { get; set; }

		/// <summary>
		/// 結束時間
		/// </summary>
		public DateTime? EndDate { get; set; }
	}
}
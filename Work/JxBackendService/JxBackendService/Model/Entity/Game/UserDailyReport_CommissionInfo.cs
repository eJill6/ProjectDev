using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Game
{
	public class UserDailyReport_CommissionInfo
	{
		public int TID { get; set; }

		[ExplicitKey]
		public int UserID { get; set; }
		public string UserName { get; set; }
		public int ParentID { get; set; }
		public string ParentName { get; set; }
		public decimal ProfitLossMoney { get; set; }
		public decimal PrizeMoney { get; set; }
		public decimal DownlineWinMoney { get; set; }
		public decimal Contribute { get; set; }
		public decimal TotalContribute { get; set; }
		public double CommissionPercent { get; set; }
		public decimal CommissionAmount { get; set; }
		public decimal downlineCommissionAmount { get; set; }
		public decimal SelfCommissionAmount { get; set; }
		public DateTime CreateTime { get; set; }
	}
}

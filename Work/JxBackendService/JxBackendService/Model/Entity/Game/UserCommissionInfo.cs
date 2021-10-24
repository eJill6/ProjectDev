using System;

namespace JxBackendService.Model.Entity.Game
{
    public partial class UserCommissionInfo
    {
		public int UCID { get; set; }
		public int UserID { get; set; }
		public string UserName { get; set; }
		public int? ParentID { get; set; }
		public string ParentName { get; set; }
		public string CommissionType { get; set; }
		public decimal? ProfitLossMoney { get; set; }
		public decimal? PrizeMoney { get; set; }
		public decimal? DownlineWinMoney { get; set; }
		public decimal? Contribute { get; set; }
		public decimal? TotalContribute { get; set; }
		public double? CommissionPercent { get; set; }
		public decimal? CommissionAmount { get; set; }
		public decimal? DownlineCommissionAmount { get; set; }
		public decimal? SelfCommissionAmount { get; set; }
		public byte? IsMinus { get; set; }
		public byte AuditStatus { get; set; }
		public int ProcessMonth { get; set; }
		public DateTime? ProcessTime { get; set; }
		public DateTime? CreateTime { get; set; }
		public bool? IsSystem { get; set; }
		public decimal? CZProfitLossMoney { get; set; }
		public decimal? FinalCZProfitLossMoney { get; set; }
		public decimal CZProfitLossMoneyPercent { get; set; }
		public decimal? DepositFee { get; set; }
	}
}

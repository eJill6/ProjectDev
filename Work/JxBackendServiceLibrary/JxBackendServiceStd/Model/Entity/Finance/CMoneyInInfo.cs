using System;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.Finance
{
	public class CMoneyInInfo : BaseEntityModel
	{
		[ExplicitKey]
		public string MoneyInID { get; set; }

		public decimal Amount { get; set; }

		[NVarcharColumnInfo(50)]
		public string OrderID { get; set; }

		public int UserID { get; set; }

		/// <summary>訂單時間</summary>
		public DateTime OrderTime { get; set; }

		public int DealType { get; set; }

		[NVarcharColumnInfo(50)]
		public string Handler { get; set; }

		[NVarcharColumnInfo(1024)]
		public string Memo { get; set; }

		[VarcharColumnInfo(10)]
		public string ProductCode { get; set; }
	}
}
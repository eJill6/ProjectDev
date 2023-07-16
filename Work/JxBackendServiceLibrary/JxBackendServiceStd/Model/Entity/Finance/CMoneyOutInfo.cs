using System;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.Finance
{
	public class BasicCMoneyOutInfo : BaseEntityModel
	{
		[ExplicitKey]
		public string MoneyOutID { get; set; }

		public int UserID { get; set; }

		public int DealType { get; set; }

		[NVarcharColumnInfo(50)]
		public string OrderID { get; set; }
	}

	public class CMoneyOutInfo : BasicCMoneyOutInfo
	{
		public decimal Amount { get; set; }

		public DateTime OrderTime { get; set; }

		[NVarcharColumnInfo(50)]
		public string Handler { get; set; }

		[NVarcharColumnInfo(1024)]
		public string Memo { get; set; }

		[VarcharColumnInfo(10)]
		public string ProductCode { get; set; }
	}
}
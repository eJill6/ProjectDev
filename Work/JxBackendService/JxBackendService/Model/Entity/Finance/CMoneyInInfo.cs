using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Entity.Finance
{
    public class CMoneyInInfo
	{
		[Key]
		public int MoneyInID { get; set; }

		public double Amount { get; set; }

		[NVarcharColumnInfo(50)]
		public string OrderID { get; set; }

		public int UserID { get; set; }

		public DateTime? OrderTime { get; set; }

		public int IsDeal { get; set; }

		[NVarcharColumnInfo(50)]
		public string CardUser { get; set; }

		[NVarcharColumnInfo(50)]
		public string BankCard { get; set; }

		[NVarcharColumnInfo(50)]
		public string BankTypeName { get; set; }

		[NVarcharColumnInfo(50)]
		public string UserName { get; set; }

		[NVarcharColumnInfo(50)]
		public string Handle { get; set; }

		[NVarcharColumnInfo(500)]
		public string Memo { get; set; }

		[VarcharColumnInfo(100)]
		public string UniqueID { get; set; }

		public DateTime? InDate { get; set; }
	}
}

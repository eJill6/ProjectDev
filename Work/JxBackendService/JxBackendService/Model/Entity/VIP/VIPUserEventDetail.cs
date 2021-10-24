using System;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.VIP
{
    public class VIPUserEventDetail : BaseEntityModel
	{
		[ExplicitKey, VarcharColumnInfo(32)]
		public string SEQID { get; set; }

		public int EventTypeID { get; set; }

		public int UserID { get; set; }

		[NVarcharColumnInfo(50)]
		public string UserName { get; set; }

		public int AuditStatus { get; set; }

		[NVarcharColumnInfo(50)]
		public string Auditor { get; set; }

		[NVarcharColumnInfo(200)]
		public string AuditMemo { get; set; }

		public DateTime? AuditTime { get; set; }

		public int CurrentLevel { get; set; }

		public decimal ApplyAmount { get; set; }

		public decimal EventAmount { get; set; }

		public decimal BonusAmount { get; set; }

		public decimal FlowMultiple { get; set; }

		public decimal FinancialFlowAmount { get; set; }

		[NVarcharColumnInfo(32)]
		public string RefID { get; set; }

		[NVarcharColumnInfo(2000)]
		public string MemoJson { get; set; }
	}
}
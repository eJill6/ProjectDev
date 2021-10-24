using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.User
{
	public class UserInfoAdditional
	{
		[ExplicitKey]
		public int UserID { get; set; }
		
		public string CreateUser { get; set; }
		
		public DateTime CreateDate { get; set; }
		
		public string UpdateUser { get; set; }
		
		public DateTime UpdateDate { get; set; }
		
		public decimal CZProfitLossMoneyPercent { get; set; }
		
		public bool? IsAllowSetTransferByParent { get; set; }

		public DateTime? Birthday { get; set; }

		public string RealName { get; set; }
	}
}

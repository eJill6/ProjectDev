using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Security
{
	public class FailureOperationCount
	{
		[ExplicitKey]
		public int UserID { get; set; }
		
		[ExplicitKey]
		public int WebActionType { get; set; }

		[ExplicitKey]
		public string SubActionType { get; set; }
		
		public int TotalCount { get; set; }
		
		public string CreateUser { get; set; }
		
		public DateTime CreateDate { get; set; }
		
		public string UpdateUser { get; set; }
		
		public DateTime UpdateDate { get; set; }
	}
}

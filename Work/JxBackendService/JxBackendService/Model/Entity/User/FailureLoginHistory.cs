using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.User
{
	public class FailureLoginHistory
	{
		[Key]
		public int ID { get; set; }
		public string UserName { get; set; }
		public DateTime LoginTime { get; set; }
		public string LoginIp { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Security
{
	public class BlackAreaList
	{
		public string Area { get; set; }
		
		public string Remark { get; set; }
		
		public bool IsWork { get; set; }
		
		public int IType { get; set; }

		[Key]
		public int ID { get; set; }
		
		public string CreateUser { get; set; }
		
		public DateTime CreateDate { get; set; }
	}
}

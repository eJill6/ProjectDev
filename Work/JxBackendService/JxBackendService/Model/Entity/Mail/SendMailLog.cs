using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Mail
{
    public class SendMailLog
    {
        [Key]
		public long SerialNo { get; set; }
		
		public string ProviderTypeName { get; set; }
        
		public decimal ElapsedSeconds { get; set; }
		
		public string RefInfoJson { get; set; }
		
		public string CreateUser { get; set; }
		
		public DateTime CreateDate { get; set; }
		
		public string UpdateUser { get; set; }
		
		public DateTime UpdateDate { get; set; }
    }
}

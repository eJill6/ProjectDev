using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Config
{
    public class ConfigGroup
    {
        [Key]
        public int GroupSerial { get; set; }
        public string GroupName { get; set; }
        public string GroupDesc { get; set; }
        public int GroupActive { get; set; }
        public int? SubGroupSeria { get; set; }
        public int UpdateUserID { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDateTime { get; set; }
    }

}

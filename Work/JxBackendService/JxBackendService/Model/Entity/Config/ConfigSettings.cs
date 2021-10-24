using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Config
{
    public class ConfigSettings
    {
        [ExplicitKey]
        public int GroupSerial { get; set; }
        [ExplicitKey]
        public int ItemKey { get; set; }
        public string ItemValue { get; set; }
        public string ItemDesc { get; set; }
        public int? ItemSort { get; set; }
        public int ItemActive { get; set; }
        public int UpdateUserID { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDateTime { get; set; }
    }

}

using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Base
{
    public class BaseEntityModel
    {
        [NVarcharColumnInfo(50)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [NVarcharColumnInfo(50)]
        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        [IgnoreRead]
        [Write(false)]
        public string CreateDateText => CreateDate.ToFormatDateTimeString();

        [IgnoreRead]
        [Write(false)]
        public string UpdateDateText => UpdateDate.ToFormatDateTimeString();

    }
}

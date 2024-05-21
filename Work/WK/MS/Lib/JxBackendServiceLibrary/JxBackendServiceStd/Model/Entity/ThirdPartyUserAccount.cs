using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity
{
    public class ThirdPartyUserAccount
    {
        [ExplicitKey]
        public int UserID { get; set; }

        [ExplicitKey]
        [VarcharColumnInfo(10)]
        public string ThirdPartyType { get; set; }

        [NVarcharColumnInfo(50)]
        public string Account { get; set; }

        public DateTime CreateTime { get; set; }

        [NVarcharColumnInfo(50)]
        public string Creator { get; set; }

        public DateTime? UpdateTime { get; set; }

        [NVarcharColumnInfo(50)]
        public string Updater { get; set; }

        [NVarcharColumnInfo(50)]
        public string Password { get; set; }
    }
}
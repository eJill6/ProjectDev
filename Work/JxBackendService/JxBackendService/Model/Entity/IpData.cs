using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Entity
{
    public class IpData
    {
        [VarcharColumnInfo(128)]
        public string StartIP { get; set; }

        public BigInteger SartIPNum { get; set; }

        [VarcharColumnInfo(128)]
        public string EndIP { get; set; }

        public BigInteger EndIPNum { get; set; }

        [NVarcharColumnInfo(128)]
        public string Area { get; set; }

        [NVarcharColumnInfo(512)]
        public string Remark { get; set; }
    }
}
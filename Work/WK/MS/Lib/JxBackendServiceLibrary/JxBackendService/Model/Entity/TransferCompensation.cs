using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity
{
    public class TransferCompensation : BaseEntityModel
    {
        [ExplicitKey, VarcharColumnInfo(32)]
        public string TransferID { get; set; }

        public int UserID { get; set; }

        [ExplicitKey, VarcharColumnInfo(10)]
        public string ProductCode { get; set; }

        [ExplicitKey, VarcharColumnInfo(30)]
        public string Type { get; set; }

        public bool IsProcessed { get; set; }
    }
}
using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity.Base
{
    public interface IBaseEntityModel
    {
        string CreateUser { get; set; }

        DateTime CreateDate { get; set; }

        string UpdateUser { get; set; }

        DateTime? UpdateDate { get; set; }
    }

    public class BaseEntityModel : IBaseEntityModel
    {
        [NVarcharColumnInfo(50)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        [NVarcharColumnInfo(50)]
        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        [IgnoreRead]
        [Write(false)]
        public string CreateDateText
        {
            get => CreateDate.ToFormatDateTimeString();
            set { }
        }

        [IgnoreRead]
        [Write(false)]
        public string UpdateDateText
        {
            get => UpdateDate.ToFormatDateTimeString();
            set { }
        }
    }
}
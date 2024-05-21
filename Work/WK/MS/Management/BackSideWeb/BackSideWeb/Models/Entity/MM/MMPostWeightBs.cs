using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;
using MS.Core.MMModel.Models.Post.Enums;
using System.ComponentModel;
using System.Reflection;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMPostWeightBs : BaseEntityModel
    {
        public int Id { get; set; }
        public string? PostId { get; set; }
        public string? Weight { get; set; }
        public string Operator { get; set; }=string.Empty;
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Status { get; set; }
        public string CreateTimeText => CreateTime.ToFormatDateTimeString();
        public PostType PostType { get; set; }
        public string PostTypeText 
        {
            get
            {
                return PostType.GetEnumDescription();
            }
        }
    }
}

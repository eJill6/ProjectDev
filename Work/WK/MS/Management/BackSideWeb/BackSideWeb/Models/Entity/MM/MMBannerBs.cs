using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMBannerBs : BaseEntityModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Sort { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; }
        public string ModifyUser { get; set; } = string.Empty;
        public byte Type { get; set; }
        public byte LinkType { get; set; }
        public string? RedirectUrl { get; set; }
        public Media Media { get; set; }
        public string StartDateText => StartDate.ToFormatDateTimeString();
        public string EndDateText => EndDate.ToFormatDateTimeString();
        public string ModifyDateText => ModifyDate.ToFormatDateTimeString();
        public int? LocationType { get; set; }
    }
}
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.HomeAnnouncement
{
    public class MyAnnouncementViewModel
    {
        public int Id { get; set; }
        public string HomeContent { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public DateTime? CreateDate { get; set; }
        public string CreateDateText => CreateDate.HasValue ? ((DateTime)CreateDate).ToString(GlobalSettings.DateTimeFormat) : DateTime.Now.ToString(GlobalSettings.DateTimeFormat);
        public DateTime? ModifyDate { get; set; }
        public string Title { get; set; }
    }
}

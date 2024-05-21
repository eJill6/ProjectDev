using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel.PublishRecord
{
    public class ReportRecordOptionViewModel
    {
        /// <summary>
        /// 帖子区域
        /// </summary>
        public List<SelectListItem>? PostRegionalListItem { get; set; }
        /// <summary>
        /// 投诉原因
        /// </summary>
        public List<SelectListItem>? ReportTypeListItem { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public List<SelectListItem>? ReportStatusListItem { get; set; }
    }
}

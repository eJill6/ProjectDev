using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel.PublishRecord
{
    public class EvaluateRecordOptionViewModel
    {
        /// <summary>
        /// 帖子区
        /// </summary>
        public List<SelectListItem>? PostRegionalListItem { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public List<SelectListItem>? TimeTypeListItem { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public List<SelectListItem>? CommentStatusListItem { get; set; }
    }
}

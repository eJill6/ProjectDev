using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel.PublishRecord
{
    /// <summary>
    /// 发表记录的下拉列表框的数据集合
    /// </summary>
    public class PublishRecordOptionViewModel
    {
        /// <summary>
        /// 帖子区
        /// </summary>
        public List<SelectListItem>? PostRegionalListItem { get; set; }

        /// <summary>
        /// 身份
        /// </summary>
        public List<SelectListItem>? IdentityItems { get; set; }

        /// <summary>
        /// 会员卡
        /// </summary>
        public List<SelectListItem>? MemberCar { get; set; }

        /// <summary>
        /// 帖子状态
        /// </summary>
        public List<SelectListItem>? PostStatus { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>
        public List<SelectListItem>? TimeType { get; set; }
        public List<SelectListItem>? IdentityTypeItems { get; set; }


    }
}

using JxBackendService.Model.Enums.MM;
using JxBackendService.Model.Paging;
using MS.Core.MMModel.Models.Post.Enums;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryHomeAnnouncementParam : BasePagingRequestParam
    {
        /// <summary>
        /// 公告类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
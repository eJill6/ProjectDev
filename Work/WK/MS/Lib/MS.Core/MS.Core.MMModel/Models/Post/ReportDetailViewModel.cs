using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Post
{
    public class ReportDetailViewModel
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 时间格式化
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 详情描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 未通过的原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 被举报的帖子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 举报编号
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 投诉原因
        /// </summary>
        public string ReportTypeText { get; set; }
        public PostType PostType { get; set; }
        /// <summary>
        /// 帖子状态
        /// </summary>
        public ReviewStatus PostStatus { get; set; }
        /// <summary>
        /// 帖子是否下架
        /// </summary>
        public bool PostIsDelete { get; set; }

        /// <summary>
        /// 截图证据
        /// </summary>
        public string[] PhotoIds { get; set; }
    }
}

using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Entity.PublishRecord
{
    /// <summary>
    /// 投诉
    /// </summary>
    public class MMPostReportModel: BaseEntityModel
    {
        public string ReportId { get; set; }
        /// <summary>
        /// 举报原因舉報原因。0：騙子、1：廣告騷擾、2：貨不對版、3：無效聯絡方式
        /// </summary>
        public int ReportType { get; set; }
        /// <summary>
        /// 投诉人ID
        /// </summary>
        public int ComplainantUserId { get; set; }
        /// <summary>
        /// 解锁单的ID
        /// </summary>
        public string PostTranId { get; set; }
        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }
        /// <summary>
        /// 帖子分类
        /// </summary>
        public int PostType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 举报内容
        /// </summary>
        public string Describe { get;set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string ExamineMan { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ExamineTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

    }
}

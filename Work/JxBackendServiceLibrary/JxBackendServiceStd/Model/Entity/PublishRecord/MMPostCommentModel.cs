using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Entity.PublishRecord
{
    /// <summary>
    /// 帖子评价信息
    /// </summary>
    public class MMPostCommentModel: BaseEntityModel
    {
        /// <summary>
        /// 评价ID
        /// </summary>
        public string CommentId { get; set; }
        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }
        /// <summary>
        /// 帖子类型
        /// </summary>
        public int PostType { get; set; }
        /// <summary>
        /// 当下头像
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 评论人ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 评论状态 0：審核中、1：已評論(通過)、2：審核不通過
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 消费时间
        /// </summary>
        public DateTime SpentTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string ExamineMan { get; set; }
        /// <summary>
        ///  审核时间
        /// </summary>
        public DateTime ExamineTime { get; set; }
        /// <summary>
        /// 审核未通过原因
        /// </summary>
        public string Memo { get; set; }
    }
}

using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerStoreEditorReviewList
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        public string ExamineTimeText => ExamineTime == null ? "" : ExamineTime.Value.ToString(GlobalSettings.DateTimeFormat);
    }
}
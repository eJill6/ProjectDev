using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyOfficialPostList
    {
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 封面照片
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 發贴人 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 發贴人當下暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 狀態。0：審核中、1：審核通過、2：未通過
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 编辑锁定 0 禁用 1 启用
        /// </summary>
        public bool LockStatus { get; set; }

        /// <summary>
        /// 預約次數
        /// </summary>
        public int AppointmentCount { get; set; }

        /// <summary>
        /// 信息標題
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        public string UpdateTimeText => UpdateTime.HasValue ? UpdateTime.Value.ToString(GlobalSettings.DateTimeFormat) : "";
        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }
        public string ExamineTimeText => ExamineTime.HasValue ? ExamineTime.Value.ToString(GlobalSettings.DateTimeFormat) : "";
        public DateTime? CreateTime { get; set; }

        public string CreateTimeText => CreateTime.HasValue ? CreateTime.Value.ToString(GlobalSettings.DateTimeFormat) : "";
        public int UnlockCount { get; set; }

        /// <summary>
        /// 前端用戶是否刪除。0：未刪除、1：刪除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}

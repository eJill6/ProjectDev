using MS.Core.MMModel.Models.Post.Enums;
using System;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 更新資料
    /// </summary>
    public class AdminOfficialPostData
    {
        /// <summary>
        /// 贴子編號
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 信息標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

        /// <summary>
        /// 詳細地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 小编打分
        /// </summary>
        public int? FacialScore { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// 首页贴
        /// </summary>
        public bool IsHomePost { get; set; }

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public int PostStatus { get; set; }

        /// <summary>
        /// w未通过原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 後台傳進來的 UserId
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }
    }
}
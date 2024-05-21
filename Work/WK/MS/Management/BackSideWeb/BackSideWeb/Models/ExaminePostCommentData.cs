using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace BackSideWeb.Models
{
    public class ExaminePostCommentData
    {
        /// <summary>
        /// 評價編號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        public string? Memo { get; set; }
    }
}
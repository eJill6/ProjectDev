using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Extensions;

namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 官方贴子清單
    /// </summary>
    public class OfficialPostList
    {
        /// <summary>
        ///  官方贴 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 發贴類型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 封面照片
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; } = string.Empty;

        /// <summary>
        /// 年齡
        /// </summary>
        public string Age { get; set; } = string.Empty;

        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; } = string.Empty;

        /// <summary>
        /// 地區編碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 最低價格
        /// </summary>
        public string LowPrice { get; set; } = string.Empty;

        /// <summary>
        /// 顏值
        /// </summary>
        public string FacialScore { get; set; } = string.Empty;

        /// <summary>
        /// 是否營業中。0：休息、1：營業中
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public string? UpdateTime { get; set; }

        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 预约次数
        /// </summary>
        public string? AppointmentCount { get; set; }
    }
}
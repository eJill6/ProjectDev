using JxBackendService.Model.Entity.PublishRecord;
using System;

namespace BackSideWeb.Models.ViewModel.PublishRecord
{
    public class PublishRecordViewModel
    {
        /// <summary>
        /// 帖子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

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
        /// 服務項目
        /// </summary>
        public string[] ServiceItem { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 職業
        /// </summary>
        public string Job { get; set; } = string.Empty;

        /// <summary>
        /// 地區編碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 是否為精選
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 期望收入
        /// </summary>
        public string UnlockAmount { get; set; } = string.Empty;

        /// <summary>
        /// 收藏數
        /// </summary>
        public string Favorites { get; set; } = string.Empty;

        /// <summary>
        /// 評論數
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// 觀看數
        /// </summary>
        public string Views { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime { get; set; } = string.Empty;
    }
}

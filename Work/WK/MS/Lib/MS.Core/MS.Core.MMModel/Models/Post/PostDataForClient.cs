﻿using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// MMPost Table
    /// </summary>
    public class PostDataForClient
    {
        /// <summary>
        /// 贴子類型
        /// </summary>
        public int PostType { get; set; }

        /// <summary>
        /// 信息類型
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 申請解鎖調價
        /// </summary>
        public decimal? ApplyAmount { get; set; }

        /// <summary>
        /// 信息標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public string Quantity { get; set; } = string.Empty;

        /// <summary>
        /// 年齡(歲)
        /// </summary>
        public AgeDefined Age { get; set; }

        /// <summary>
        /// 身高(cm)
        /// </summary>
        public BodyHeightDefined Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public CupDefined Cup { get; set; }

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

        /// <summary>
        /// 服務種類 Id
        /// </summary>
        public int[] ServiceIds { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// 最高價格
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// 詳細地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 聯繫資訊
        /// </summary>
        public ContactInfoForClient[] ContactInfos { get; set; }

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 照片 id list
        /// </summary>
        public string[] PhotoIds { get; set; }

        /// <summary>
        /// 視頻 id list
        /// </summary>
        public string[] VideoIds { get; set; }
    }
}
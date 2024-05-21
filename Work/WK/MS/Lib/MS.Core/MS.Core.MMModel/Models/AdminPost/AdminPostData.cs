using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 更新資料
    /// </summary>
    public class AdminPostData
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
        /// 是否同意申請調價
        /// </summary>
        public bool IsApplyAdjustPrice { get; set; }

        /// <summary>
        /// 精選
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public ReviewStatus PostStatus { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 信息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHours { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfos { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        public string ServiceDescribe { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// 首页贴
        /// </summary>
        public bool IsHomePost { get; set; }

        /// <summary>
        /// 信息類型
        /// </summary>
        public int? MessageId { get; set; }

        /// <summary>
        /// 申請解鎖調價
        /// </summary>
        public decimal? ApplyAmount { get; set; }

        /// <summary>
        /// 是否会员申请调价
        /// </summary>
        public bool? IsApply { get; set; }

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
        public int Age { get; set; }

        /// <summary>
        /// 身高(cm)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public int Cup { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        public decimal? LowPrice { get; set; }

        /// <summary>
        /// 最高價格
        /// </summary>
        public decimal? HighPrice { get; set; }

        public string ServiceIds { get; set; }

        /// <summary>
        /// 後台傳進來的 UserId
        /// </summary>
        public int? UserId { get; set; }

        public string PhotoIds { get; set; }
        public string VideoIds { get; set; }
        public int PostType { get; set; }

        public decimal? UnlockAmount { get; set; }

        /// <summary>
        /// 解锁基础值
        /// </summary>
        public int? UnlockBaseCount { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }
        /// <summary>
        /// 认证
        /// </summary>
        public bool? IsCertified { get; set; }
    }
}
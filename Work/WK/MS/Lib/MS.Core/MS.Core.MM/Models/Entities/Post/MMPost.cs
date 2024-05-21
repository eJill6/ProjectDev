using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    public class QueryPostTypeCount
    {
        public ReviewStatus Status { get; set; }
        public PostType PostType { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// MMPost Table
    /// </summary>
    public class MMPost : BaseDBModel
    {
        /// <summary>
        /// 贴 Id
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 贴子類型。1：廣場、2：担保(原為中介)、3：官方、4：體驗
        /// </summary>
        public PostType PostType { get; set; }

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
        /// 狀態。0：審核中、1：核準 (即上架)、2：未通過
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 預約次數
        /// </summary>
        public int AppointmentCount { get; set; }

        /// <summary>
        /// 信息類型
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 解鎖價格
        /// </summary>
        public decimal UnlockAmount { get; set; }

        /// <summary>
        /// 申請解鎖價格
        /// </summary>
        public decimal ApplyAmount { get; set; }

        /// <summary>
        /// 申請調價
        /// </summary>
        public bool ApplyAdjustPrice { get; set; }

        /// <summary>
        /// 信息標題
        /// </summary>
        [EntityType(DbType.String, 20, false)]
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
        /// 年齡(歲) 18~28+。+ 用99代替
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// 身高(cm) 150~175+。+ 用999 代替
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 罩杯 A~E+。1~6
        /// </summary>
        public byte Cup { get; set; }

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

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
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 收藏數
        /// </summary>
        public int Favorites { get; set; }

        /// <summary>
        /// 評論數
        /// </summary>
        public long Comments { get; set; }

        /// <summary>
        /// 觀看數
        /// </summary>
        public long Views { get; set; }

        /// <summary>
        /// 解鎖次數
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 熱度
        /// </summary>
        public int? Heat { get; set; }

        /// <summary>
        /// 是否精選
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// 建贴時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string? ExamineMan { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string? Memo { get; set; } = string.Empty;

        /// <summary>
        /// 編輯且在審核中的時候，前端顯示舊編輯資料
        /// </summary>
        public string? OldViewData { get; set; } = string.Empty;

        /// <summary>
        /// 解锁基础值
        /// </summary>
        public int? UnlockBaseCount { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 本次发帖计数
        /// </summary>
        public int? PostCount { get; set; }

        /// <summary>
        /// 当前发帖身份
        /// </summary>
        public int? CurrentIdentity { get; set; }

        /// <summary>
        /// 已認證。0：未認證、1：已認證
        /// </summary>
        public bool? IsCertified { get; set; }
    }
}
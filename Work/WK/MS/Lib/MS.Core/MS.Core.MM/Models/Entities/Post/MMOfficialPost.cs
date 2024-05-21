using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMOfficialPost : BaseDBModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
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
        /// 编辑锁定 0 解锁 1 锁定
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
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 年齡(歲) 18~28+。+ 用99代替
        /// </summary>
        public byte Age { get; set; }

        /// <summary>
        /// 身高(cm) 150~175+。+ 用999 代替
        /// </summary>
        public short Height { get; set; }

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
        /// 顏值總計
        /// </summary>
        public decimal TotalFacialScore { get; set; }

        /// <summary>
        /// 服務質量總計
        /// </summary>
        public decimal TotalServiceQuality { get; set; }

        /// <summary>
        /// 顏值
        /// </summary>
        public int? FacialScore { get; set; }

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
        /// 審核未通過原因
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        /// 編輯且在審核中的時候，前端顯示舊編輯資料
        /// </summary>
        public string? OldViewData { get; set; }

        /// <summary>
        /// 前端用戶是否刪除。0：未刪除、1：刪除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 本次发帖计数
        /// </summary>
        public int? PostCount { get; set; }
    }
}
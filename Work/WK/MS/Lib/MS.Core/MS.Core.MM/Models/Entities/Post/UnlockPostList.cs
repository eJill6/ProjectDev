using MMService.DBTools;
using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.Post
{
    public class UnlockPostList
    {
        /// <summary>
        /// 審核狀態
        /// </summary>
        public ReviewStatus Status { get; set; }
        /// <summary>
        /// 观看基础值
        /// </summary>
        public int ViewBaseCount { get; set; }
        /// <summary>
        /// 解锁基础值
        /// </summary>
        public int UnlockBaseCount { get; set; }

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
        /// 預約次數
        /// </summary>
        public int AppointmentCount { get; set; }

        /// <summary>
        /// 信息類型
        /// </summary>
        public int MessageId { get; set; }

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

    }
}

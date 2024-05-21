using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.User
{
    public class MMBoss : BaseDBModel
    {
        /// <summary>
        /// 覓老闆 Id
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string BossId { get; set; } = string.Empty;

        /// <summary>
        /// 店鋪名稱
        /// </summary>
        public string ShopName { get; set; } = string.Empty;

        /// <summary>
        /// 妹子數量
        /// </summary>
        public string Girls { get; set; } = string.Empty;

        /// <summary>
        /// 服務價格 - 最低價格
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// 服務價格 - 最高價格
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// 聯繫方式
        /// </summary>
        public string Contact { get; set; } = string.Empty;

        /// <summary>
        /// 申請編號
        /// </summary>
        public string ApplyId { get; set; } = string.Empty;

        /// <summary>
        /// 建單時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 店龄
        /// </summary>
        public int? ShopYears { get; set; }

        /// <summary>
        /// 成交订单
        /// </summary>
        public int? DealOrder { get; set; }

        /// <summary>
        /// 自评人气
        /// </summary>
        public int? SelfPopularity { get; set; }

        /// <summary>
        /// 店铺观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; } = string.Empty;

        /// <summary>
        /// 营业时段
        /// </summary>
        public string BusinessDate { get; set; } = string.Empty;

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHour { get; set; } = string.Empty;

        /// <summary>
        /// TG群ID
        /// </summary>
        public string TelegramGroupId { get; set; } = string.Empty;
        /// <summary>
        /// 平台分账比
        /// </summary>
        public int? PlatformSharing { get; set; }
    }
}
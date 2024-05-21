using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.BossShop
{
    public class MMBossShop : BaseDBModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string Id { get; set; } = string.Empty;

        public string ApplyId { get; set; } = string.Empty;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 关联boss 表得ID
        /// </summary>
        public string BossId { get; set; } = string.Empty;
        /// <summary>
        /// 联系软件
        /// </summary>
        public string ContactApp { get; set; } = string.Empty;
        /// <summary>
        /// 联系号码
        /// </summary>
        public string ContactInfo { get; set; } = string.Empty;
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; } = string.Empty;
        /// <summary>
        /// 女孩数量
        /// </summary>
        public int Girls { get; set; }
        /// <summary>
        /// 店龄
        /// </summary>
        public int ShopYears { get; set; }
        /// <summary>
        /// 成交订单数
        /// </summary>
        public int DealOrder { get; set; }
        /// <summary>
        /// 自评分数
        /// </summary>
        public int SelfPopularity { get; set; }
        /// <summary>
        /// 店铺介绍
        /// </summary>
        public string Introduction { get; set; } = string.Empty;
        /// <summary>
        /// 店铺照片封面
        /// </summary>
        public string ShopAvatarSource { get; set; } = string.Empty;
        /// <summary>
        /// 店铺照片
        /// </summary>
        public string BusinessPhotoSource { get; set; } = string.Empty;

        /// <summary>
        /// 狀態。0：審核中、1：核準 2：未通過
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 新增時間
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
        /// 备注
        /// </summary>
        public string Memo { get; set; } = string.Empty;
    }
}
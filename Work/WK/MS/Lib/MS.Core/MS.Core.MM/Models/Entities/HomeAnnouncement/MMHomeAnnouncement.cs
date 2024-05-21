using MS.Core.MMModel.Models.Post.Enums;
using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.HomeAnnouncement
{
    public class MMHomeAnnouncement : BaseDBModel
    {
        [AutoKey]
        public int Id { get; set; }

        public string HomeContent { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Operator { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 类型。1：公告、2：首页盖台
        /// </summary>
        public int Type { get; set; }
    }
}
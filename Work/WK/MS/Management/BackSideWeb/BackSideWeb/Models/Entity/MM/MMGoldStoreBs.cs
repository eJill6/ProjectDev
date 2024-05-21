using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;
using MS.Core.MMModel.Models.Post.Enums;
using System.ComponentModel;
using System.Reflection;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMGoldStoreBs : BaseEntityModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int Top { get; set; }
        public string Operator { get; set; } = string.Empty;
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 类型：1：官方推荐 2：金牌店铺
        /// </summary>
        public int Type { get; set; }

        public string CreateTimeText => CreateTime.ToFormatDateTimeString();
    }
}
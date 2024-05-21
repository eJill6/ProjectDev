using MS.Core.Attributes;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.Models;
using System.Text.Json.Serialization;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMOptions : BaseDBModel
    {
        /// <summary>
        /// 型態Id
        /// </summary>
        [AutoKey]
        public int OptionId { get; set; }

        /// <summary>
        /// 型態名稱
        /// </summary>
        public string OptionContent { get; set; } = string.Empty;

        /// <summary>
        /// 區域id
        /// </summary>
        public byte OptionType { get; set; }

        /// <summary>
        /// 贴子類型。1：廣場、2：担保(原為中介)、3：官方、4：體驗
        /// </summary>
        public byte PostType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; } = string.Empty;

        /// <summary>
        /// 是否開啟
        /// </summary>
        public bool IsActive { get; set; }
    }
}
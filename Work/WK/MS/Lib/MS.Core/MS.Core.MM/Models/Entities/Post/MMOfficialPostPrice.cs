using MS.Core.Attributes;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMOfficialPostPrice : BaseDBModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 贴子Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 套餐序數。0、1、2、3…
        /// </summary>
        public byte Ordinal { get; set; }

        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string ComboName { get; set; } = string.Empty;

        /// <summary>
        /// 套餐價格
        /// </summary>
        public decimal ComboPrice { get; set; }

        /// <summary>
        /// 服務時間、次數或包含項目
        /// </summary>
        public string Service { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
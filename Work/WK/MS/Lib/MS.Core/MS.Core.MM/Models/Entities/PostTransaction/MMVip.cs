using MS.Core.Attributes;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class MMVip : BaseDBModel
    {
        /// <summary>
        /// VIP Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 類型(廣場、官方...)
        /// </summary>
        public VipType Type { get; set; }
        /// <summary>
        /// 售價
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 狀態(上架、下架、刪除)
        /// </summary>
        public VipStatus Status { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string? Memo { get; set; }
        /// <summary>
        /// 新增人
        /// </summary>
        public string CreateUser { get; set; } = null!;
        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateUser { get; set; } = null!;
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 會員天數
        /// </summary>
        public int Days { get; set; }
    }
}

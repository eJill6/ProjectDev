using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.Vip
{
    public class ResVip
    {
        /// <summary>
        /// VIP Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 售價
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string? Memo { get; set; }
        /// <summary>
        /// 類型(廣場、官方...)
        /// </summary>
        public VipType Type { get; set; }
        public int Days { get; set; }
        public VipStatus Status { get; set; }
    }
}

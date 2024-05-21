namespace MS.Core.MMModel.Models.Vip
{
    public class VipViewModel
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
        public string Memo { get; set; }
        /// <summary>
        /// 類型(廣場、官方...)
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 天數
        /// </summary>
        public int Days { get; set; }
    }
}

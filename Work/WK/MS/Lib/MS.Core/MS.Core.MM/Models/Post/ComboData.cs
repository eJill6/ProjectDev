namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 套餐資料
    /// </summary>
    public class ComboData
    {
        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string ComboName { get; set; } = string.Empty;

        /// <summary>
        /// 套餐價格
        /// </summary>
        public decimal ComboPrice { get; set; }

        /// <summary>
        /// 服務時間，次數或包含項目
        /// </summary>
        public string Service { get; set; } = string.Empty;
    }
}
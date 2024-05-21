namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 官方贴編輯資訊
    /// </summary>
    public class OfficialPostEditData
    {
        /// <summary>
        /// 信息標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 年齡(歲)
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 身高(cm)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public int Cup { get; set; }

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

        /// <summary>
        /// 詳細地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 服務種類 Id
        /// </summary>
        public int[] ServiceIds { get; set; } = Array.Empty<int>();

        /// <summary>
        /// 照片來源
        /// </summary>
        public Dictionary<string, string> PhotoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 視頻來源
        /// </summary>
        public Dictionary<string, string>? VideoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 聯繫資訊
        /// </summary>
        public ComboData[] Combo { get; set; } = new ComboData[0];
    }
}
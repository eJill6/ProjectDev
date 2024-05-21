using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// MMPost Table
    /// </summary>
    public class OfficialPostDataForClient
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
        public AgeDefined Age { get; set; }

        /// <summary>
        /// 身高(cm)
        /// </summary>
        public BodyHeightDefined Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public CupDefined Cup { get; set; }

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

        /// <summary>
        /// 服務種類 Id
        /// </summary>
        public int[] ServiceIds { get; set; } = new int[0];

        /// <summary>
        /// 詳細地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 照片 id list
        /// </summary>
        public string[] PhotoIds { get; set; } = new string[0];

        /// <summary>
        /// 視頻 id list
        /// </summary>
        public string[] VideoIds { get; set; } = new string[0];

        /// <summary>
        /// 套餐設定
        /// </summary>
        public ComboDataForClient[] Combo { get; set; } = new ComboDataForClient[0];

        /// <summary>
        /// 後台傳進來的 UserId
        /// </summary>
        public int? UserId { get; set; }
    }
}